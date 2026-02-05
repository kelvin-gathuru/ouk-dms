#!/bin/bash

# Configuration
SERVER_IP="102.210.149.248"
USER="tito"
PASS="tiro123."
REMOTE_DIR="/var/www/html/ParliamentSystem"
FRONTEND_DIR="$REMOTE_DIR/frontend/browser"

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${GREEN}Starting Frontend Deployment to $SERVER_IP...${NC}"

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

# Deploy Frontend
echo -e "${GREEN}Uploading Frontend...${NC}"
# Clean remote directory to ensure no stale files
run_remote_sudo_script "rm -rf $FRONTEND_DIR/*"
upload_file "AngularFrontend/dist/document-management/browser/." "$FRONTEND_DIR"

echo -e "${GREEN}Configuring Frontend Permissions...${NC}"
run_remote_sudo_script "
chown -R www-data:www-data $FRONTEND_DIR
chmod -R 755 $FRONTEND_DIR
systemctl reload apache2
"

echo -e "${GREEN}Frontend Deployment Complete!${NC}"
