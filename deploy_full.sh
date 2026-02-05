#!/bin/bash

# Configuration
SERVER_IP="102.210.149.248"
USER="tito"
PASS="tiro123."
DB_NAME="document_management_db"
DB_USER="document_management_user"
DB_PASS="DocMgmt@12Kelvin" # Extracted from appsettings.json
REMOTE_DIR="/var/www/html/ParliamentSystem"
BACKEND_DIR="$REMOTE_DIR/api"
FRONTEND_DIR="$REMOTE_DIR/frontend/browser"
EXTERNAL_FRONTEND_REMOTE_DIR="$REMOTE_DIR/external-frontend"
EXTERNAL_FRONTEND_DIR="external-frontend"

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${GREEN}Starting Deployment to $SERVER_IP...${NC}"

# Check for sshpass
if ! command -v sshpass &> /dev/null; then
    echo -e "${RED}sshpass is not installed. Attempting to install...${NC}"
    if command -v apt-get &> /dev/null; then
        sudo apt-get update && sudo apt-get install -y sshpass
    else
        echo -e "${RED}Cannot install sshpass. Please install it manually: sudo apt install sshpass${NC}"
        exit 1
    fi
fi

# Function to run remote commands
run_remote() {
    sshpass -p "$PASS" ssh -o StrictHostKeyChecking=no $USER@$SERVER_IP "$1"
}

# Function to upload files
upload_file() {
    sshpass -p "$PASS" scp -o StrictHostKeyChecking=no -r "$1" $USER@$SERVER_IP:"$2"
}

# Function to run remote commands via a temporary script to avoid sudo piping issues
run_remote_sudo_script() {
    local SCRIPT_CONTENT="$1"
    local REMOTE_PAYLOAD="/tmp/deploy_payload_$RANDOM.sh"
    
    # Create the payload script locally
    # Use printf to safely write the content including newlines
    printf "%s\n" "$SCRIPT_CONTENT" > /tmp/local_payload.sh
    
    # Upload the payload
    upload_file "/tmp/local_payload.sh" "$REMOTE_PAYLOAD"
    
    # Execute with sudo
    # We echo the password into sudo which reads from stdin (-S)
    # We execute the payload with bash
    run_remote "echo '$PASS' | sudo -S -p '' bash $REMOTE_PAYLOAD && rm $REMOTE_PAYLOAD"
    
    # Cleanup local
    rm /tmp/local_payload.sh
}

# 1. Prepare Server Directories
echo -e "${GREEN}Creating remote directories...${NC}"
run_remote_sudo_script "mkdir -p $BACKEND_DIR $FRONTEND_DIR && chown -R $USER:$USER $REMOTE_DIR"

# 2. Database Setup
echo -e "${GREEN}Setting up Database...${NC}"
upload_file "document_management_db.sql" "/tmp/document_management_db.sql"
run_remote_sudo_script "
mysql -e \"CREATE DATABASE IF NOT EXISTS $DB_NAME;\"
mysql -e \"CREATE USER IF NOT EXISTS '$DB_USER'@'localhost' IDENTIFIED BY '$DB_PASS';\"
mysql -e \"GRANT ALL PRIVILEGES ON $DB_NAME.* TO '$DB_USER'@'localhost'; FLUSH PRIVILEGES;\"
mysql $DB_NAME < /tmp/document_management_db.sql
"

# 2.1 Apply Schema Updates
echo -e "${GREEN}Applying Schema Updates...${NC}"
run_remote_sudo_script "
mysql -u $DB_USER -p'$DB_PASS' -D $DB_NAME -e \"ALTER TABLE Clients ADD Password longtext CHARACTER SET utf8mb4 NULL;\" || true
mysql -u $DB_USER -p'$DB_PASS' -D $DB_NAME -e \"ALTER TABLE Clients ADD IsActivated tinyint(1) NOT NULL DEFAULT 0;\" || true
mysql -u $DB_USER -p'$DB_PASS' -D $DB_NAME -e \"ALTER TABLE Clients ADD ActivationCode longtext CHARACTER SET utf8mb4 NULL;\" || true
"

# 2.2 Update SMTP Settings (Now handled via appsettings.json)
# echo -e "${GREEN}Updating SMTP Settings...${NC}"
# run_remote_sudo_script "
# mysql -u $DB_USER -p'$DB_PASS' -D $DB_NAME -e \"DELETE FROM EmailSMTPSettings;\"
# "

# 3. Deploy Backend
echo -e "${GREEN}Building Backend...${NC}"
cd MySQL-NET-REST-API/DocumentManagement.API
dotnet publish -c Release -o ./publish
cd ../..

echo -e "${GREEN}Stopping Backend Service...${NC}"
run_remote_sudo_script "systemctl stop parliament-api || true"

echo -e "${GREEN}Uploading Backend...${NC}"
# Clean remote directory to ensure no stale files
run_remote_sudo_script "rm -rf $BACKEND_DIR/*"
upload_file "MySQL-NET-REST-API/DocumentManagement.API/publish/." "$BACKEND_DIR"
upload_file "parliament-api.service" "/tmp/parliament-api.service"
upload_file "ke-parliament-api.conf" "/tmp/ke-parliament-api.conf"

echo -e "${GREEN}Configuring Backend Service...${NC}"
run_remote_sudo_script "
mv /tmp/parliament-api.service /etc/systemd/system/
mkdir -p $BACKEND_DIR/Documents $BACKEND_DIR/Signatures $BACKEND_DIR/FileRequestDocument $BACKEND_DIR/SearchIndex $BACKEND_DIR/OCRTemp $BACKEND_DIR/summary
chown -R www-data:www-data $BACKEND_DIR
chmod -R 775 $BACKEND_DIR
systemctl daemon-reload
systemctl enable parliament-api
systemctl restart parliament-api
"

echo -e "${GREEN}Configuring Backend Apache...${NC}"
run_remote_sudo_script "
mv /tmp/ke-parliament-api.conf /etc/apache2/sites-available/
a2ensite ke-parliament-api.conf
"

# 4. Deploy Frontend
echo -e "${GREEN}Building Frontend...${NC}"
cd AngularFrontend
npm install --legacy-peer-deps
ng build --configuration production
cd ..

echo -e "${GREEN}Uploading Frontend...${NC}"
# Ensure the target directory is empty or we overwrite
run_remote_sudo_script "rm -rf $FRONTEND_DIR/*"
upload_file "AngularFrontend/dist/document-management/browser/." "$FRONTEND_DIR"
upload_file "ke-parliament.conf" "/tmp/ke-parliament.conf"

echo -e "${GREEN}Configuring Frontend Apache...${NC}"
run_remote_sudo_script "
mv /tmp/ke-parliament.conf /etc/apache2/sites-available/
a2ensite ke-parliament.conf
a2enmod rewrite proxy proxy_http proxy_wstunnel
systemctl restart apache2
"

# ------------------------------------------------------------------------------
# 5. Build and Deploy External Frontend
# ------------------------------------------------------------------------------
echo -e "${GREEN}Building External Frontend...${NC}"
cd $EXTERNAL_FRONTEND_DIR
npm install
npm run build
if [ $? -ne 0 ]; then
    echo -e "${RED}External Frontend build failed!${NC}"
    exit 1
fi
cd ..

echo -e "${GREEN}Deploying External Frontend...${NC}"
run_remote_sudo_script "
rm -rf $EXTERNAL_FRONTEND_REMOTE_DIR
mkdir -p $EXTERNAL_FRONTEND_REMOTE_DIR
chown -R $USER:$USER $EXTERNAL_FRONTEND_REMOTE_DIR
"
upload_file "$EXTERNAL_FRONTEND_DIR/build" "$EXTERNAL_FRONTEND_REMOTE_DIR"

echo -e "${GREEN}Configuring External Frontend Apache...${NC}"
upload_file "ke-parliament-web.conf" "/tmp/ke-parliament-web.conf"
run_remote_sudo_script "
mv /tmp/ke-parliament-web.conf /etc/apache2/sites-available/
a2ensite ke-parliament-web.conf
systemctl reload apache2
"

echo -e "${GREEN}Deployment Complete!${NC}"
echo -e "Frontend: https://ke-parliament.topdev.co.ke"
echo -e "Backend: https://ke-parliament-api.topdev.co.ke"
echo -e "External Frontend: https://ke-parliament-web.topdev.co.ke"
