#!/bin/bash

# Configuration
SERVER_IP="102.210.149.248"
USER="tito"
PASS="tiro123."
REMOTE_DIR="/var/www/html/ParliamentSystem"
EXTERNAL_FRONTEND_REMOTE_DIR="$REMOTE_DIR/external-frontend"
EXTERNAL_FRONTEND_DIR="external-frontend"

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${GREEN}Starting Deployment of External Frontend to $SERVER_IP...${NC}"

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
cd $EXTERNAL_FRONTEND_DIR
npm install
npm run build
if [ $? -ne 0 ]; then
    echo -e "${RED}External Frontend build failed!${NC}"
    exit 1
fi
cd ..

# Deploy
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
echo -e "External Frontend: https://ke-parliament-web.topdev.co.ke"
