#!/bin/bash

# Deployment script for Client Password Reset
# Deploys backend (API, MediatR) and external frontend

# Configuration
SERVER_IP="102.210.149.248"
USER="tito"
PASS="tiro123."
BACKEND_DIR="/var/www/html/ParliamentSystem/api"
EXTERNAL_FRONTEND_REMOTE_DIR="/var/www/html/ParliamentSystem/external-frontend"

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${GREEN}=========================================="
echo "Deploy - Client Password Reset"
echo "==========================================${NC}"

# Check for sshpass
if ! command -v sshpass &> /dev/null; then
    echo -e "${RED}sshpass is not installed. Installing...${NC}"
    sudo apt-get update && sudo apt-get install -y sshpass
fi

# Function to run remote commands
run_remote() {
    sshpass -p "$PASS" ssh -o StrictHostKeyChecking=no $USER@$SERVER_IP "$1"
}

# Function to upload files
upload_file() {
    sshpass -p "$PASS" scp -o StrictHostKeyChecking=no -r "$1" $USER@$SERVER_IP:"$2"
}

# Function to run remote commands with sudo
run_remote_sudo_script() {
    local SCRIPT_CONTENT="$1"
    local REMOTE_PAYLOAD="/tmp/deploy_payload_$RANDOM.sh"
    
    printf "%s\n" "$SCRIPT_CONTENT" > /tmp/local_payload.sh
    upload_file "/tmp/local_payload.sh" "$REMOTE_PAYLOAD"
    run_remote "echo '$PASS' | sudo -S -p '' bash $REMOTE_PAYLOAD && rm $REMOTE_PAYLOAD"
    rm /tmp/local_payload.sh
}

# Build backend locally
echo ""
echo -e "${GREEN}Building Backend...${NC}"
cd MySQL-NET-REST-API/DocumentManagement.API
dotnet publish -c Release -o ./publish

if [ $? -ne 0 ]; then
    echo -e "${RED}Backend build failed!${NC}"
    exit 1
fi
cd ../..

echo ""
echo -e "${GREEN}Stopping Backend Service on Server...${NC}"
run_remote_sudo_script "systemctl stop parliament-api"

echo ""
echo -e "${GREEN}Uploading Backend DLLs...${NC}"
# Upload changed DLLs
upload_file "MySQL-NET-REST-API/DocumentManagement.API/publish/DocumentManagement.API.dll" "/tmp/DocumentManagement.API.dll"
upload_file "MySQL-NET-REST-API/DocumentManagement.API/publish/DocumentManagement.MediatR.dll" "/tmp/DocumentManagement.MediatR.dll"
upload_file "MySQL-NET-REST-API/DocumentManagement.API/publish/DocumentManagement.Helper.dll" "/tmp/DocumentManagement.Helper.dll"
upload_file "MySQL-NET-REST-API/DocumentManagement.API/publish/appsettings.json" "/tmp/appsettings.json"
upload_file "MySQL-NET-REST-API/DocumentManagement.API/publish/appsettings.Production.json" "/tmp/appsettings.Production.json"

run_remote_sudo_script "
mv /tmp/DocumentManagement.API.dll $BACKEND_DIR/
mv /tmp/DocumentManagement.MediatR.dll $BACKEND_DIR/
mv /tmp/DocumentManagement.Helper.dll $BACKEND_DIR/
mv /tmp/appsettings.json $BACKEND_DIR/
mv /tmp/appsettings.Production.json $BACKEND_DIR/
chown www-data:www-data $BACKEND_DIR/DocumentManagement.API.dll
chown www-data:www-data $BACKEND_DIR/DocumentManagement.MediatR.dll
chown www-data:www-data $BACKEND_DIR/DocumentManagement.Helper.dll
chown www-data:www-data $BACKEND_DIR/appsettings.json
chown www-data:www-data $BACKEND_DIR/appsettings.Production.json
"

echo ""
echo -e "${GREEN}Starting Backend Service...${NC}"
run_remote_sudo_script "systemctl start parliament-api"

sleep 3

echo ""
echo -e "${GREEN}Checking Backend Service Status...${NC}"
run_remote_sudo_script "systemctl status parliament-api --no-pager -l | head -20"

# Build Frontend
echo ""
echo -e "${GREEN}Building External Frontend...${NC}"
cd external-frontend
npm run build

if [ $? -ne 0 ]; then
    echo -e "${RED}Frontend build failed!${NC}"
    exit 1
fi
cd ..

# Deploy frontend
echo ""
echo -e "${GREEN}Deploying External Frontend...${NC}"

# Upload build folder
# We use a zip to make it faster/cleaner
cd external-frontend
zip -r build.zip build
upload_file "build.zip" "/tmp/build.zip"
rm build.zip
cd ..

run_remote_sudo_script "
rm -rf $EXTERNAL_FRONTEND_REMOTE_DIR/build
unzip /tmp/build.zip -d $EXTERNAL_FRONTEND_REMOTE_DIR/
rm /tmp/build.zip
chown -R www-data:www-data $EXTERNAL_FRONTEND_REMOTE_DIR
systemctl reload apache2
"

echo ""
echo -e "${GREEN}=========================================="
echo "Deployment Complete!"
echo "==========================================${NC}"
