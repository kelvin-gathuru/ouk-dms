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

echo -e "${GREEN}Starting Backend Deployment to $SERVER_IP...${NC}"

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
    printf "%s\n" "$SCRIPT_CONTENT" > /tmp/local_payload.sh
    
    # Upload the payload
    upload_file "/tmp/local_payload.sh" "$REMOTE_PAYLOAD"
    
    # Execute with sudo
    run_remote "echo '$PASS' | sudo -S -p '' bash $REMOTE_PAYLOAD && rm $REMOTE_PAYLOAD"
    
    # Cleanup local
    rm /tmp/local_payload.sh
}

# Deploy Backend
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

echo -e "${GREEN}Configuring Backend Service...${NC}"
run_remote_sudo_script "
mkdir -p $BACKEND_DIR/Documents $BACKEND_DIR/Signatures $BACKEND_DIR/FileRequestDocument $BACKEND_DIR/SearchIndex $BACKEND_DIR/OCRTemp $BACKEND_DIR/summary
chown -R www-data:www-data $BACKEND_DIR
chmod -R 775 $BACKEND_DIR
systemctl daemon-reload
systemctl enable parliament-api
systemctl restart parliament-api
"

echo -e "${GREEN}Backend Deployment Complete!${NC}"
