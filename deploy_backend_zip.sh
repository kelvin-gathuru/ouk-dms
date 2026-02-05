#!/bin/bash

# Configuration
SERVER_IP="102.210.149.248"
USER="tito"
PASS="tiro123."
REMOTE_DIR="/var/www/html/ParliamentSystem"
BACKEND_DIR="$REMOTE_DIR/api"

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${GREEN}Starting Optimized Backend Deployment to $SERVER_IP...${NC}"

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

# 3. Deploy Backend
echo -e "${GREEN}Building Backend...${NC}"
cd MySQL-NET-REST-API/DocumentManagement.API
dotnet publish -c Release -o ./publish

echo -e "${GREEN}Zipping Build...${NC}"
cd publish
zip -r ../backend_build.zip .
cd ..
cd ../..

echo -e "${GREEN}Stopping Backend Service...${NC}"
run_remote_sudo_script "systemctl stop parliament-api || true"

echo -e "${GREEN}Uploading Backend Zip...${NC}"
# Clean remote directory to ensure no stale files
run_remote_sudo_script "rm -rf $BACKEND_DIR/*"
upload_file "MySQL-NET-REST-API/DocumentManagement.API/backend_build.zip" "/tmp/backend_build.zip"

echo -e "${GREEN}Extracting and Configuring Backend Service...${NC}"
run_remote_sudo_script "
unzip -o /tmp/backend_build.zip -d $BACKEND_DIR
rm /tmp/backend_build.zip
chown -R www-data:www-data $BACKEND_DIR
chmod -R 775 $BACKEND_DIR
systemctl daemon-reload
systemctl restart parliament-api
"

echo -e "${GREEN}Backend Deployment Complete!${NC}"
