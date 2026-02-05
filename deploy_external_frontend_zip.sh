#!/bin/bash

# Configuration
SERVER_IP="102.210.149.248"
USER="tito"
PASS="tiro123."
REMOTE_BASE_DIR="/var/www/html/ParliamentSystem/external-frontend"
REMOTE_BUILD_DIR="$REMOTE_BASE_DIR/build"

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${GREEN}Starting Optimized External Frontend Deployment to $SERVER_IP...${NC}"

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
    sshpass -p "$PASS" scp -o StrictHostKeyChecking=no "$1" $USER@$SERVER_IP:"$2"
}

# Function to run remote commands via a temporary script
run_remote_sudo_script() {
    local SCRIPT_CONTENT="$1"
    local REMOTE_PAYLOAD="/tmp/deploy_payload_$RANDOM.sh"
    
    printf "%s\n" "$SCRIPT_CONTENT" > /tmp/local_payload.sh
    upload_file "/tmp/local_payload.sh" "$REMOTE_PAYLOAD"
    run_remote "echo '$PASS' | sudo -S -p '' bash $REMOTE_PAYLOAD && rm $REMOTE_PAYLOAD"
    rm /tmp/local_payload.sh
}

# Build
echo -e "${GREEN}Building External Frontend...${NC}"
cd external-frontend
npm install
npm run build
if [ $? -ne 0 ]; then
    echo -e "${RED}External Frontend build failed!${NC}"
    exit 1
fi

echo -e "${GREEN}Zipping Build...${NC}"
cd build
zip -r ../../external_frontend_build.zip .
cd ../..

# Deploy
echo -e "${GREEN}Uploading External Frontend Zip...${NC}"
upload_file "external_frontend_build.zip" "/tmp/external_frontend_build.zip"

echo -e "${GREEN}Extracting and Configuring...${NC}"
run_remote_sudo_script "
# Ensure directories exist
mkdir -p $REMOTE_BUILD_DIR

# Clean old files
rm -rf $REMOTE_BUILD_DIR/*

# Extract new files
unzip -o /tmp/external_frontend_build.zip -d $REMOTE_BUILD_DIR
rm /tmp/external_frontend_build.zip

# Set permissions
chown -R www-data:www-data $REMOTE_BASE_DIR
chmod -R 755 $REMOTE_BASE_DIR

# Apache Config (Optional, but good to ensure)
# We assume the config file is already set up or we can upload it if needed.
# For now, we just reload Apache to be safe.
systemctl reload apache2
"

# Upload Apache config just in case (matching the other script)
echo -e "${GREEN}Updating Apache Config...${NC}"
upload_file "ke-parliament-web.conf" "/tmp/ke-parliament-web.conf"
run_remote_sudo_script "
mv /tmp/ke-parliament-web.conf /etc/apache2/sites-available/
a2ensite ke-parliament-web.conf
systemctl reload apache2
"

echo -e "${GREEN}Deployment Complete!${NC}"
echo -e "External Frontend: https://ke-parliament-web.topdev.co.ke"
