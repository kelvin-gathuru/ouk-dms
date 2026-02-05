#!/bin/bash

# Quick deployment script for authentication fix
# Deploys only the changed backend and frontend files

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
echo "Quick Deploy - Authentication Fix"
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
# Upload only the changed DLL files
upload_file "MySQL-NET-REST-API/DocumentManagement.API/publish/DocumentManagement.Data.dll" "/tmp/DocumentManagement.Data.dll"
upload_file "MySQL-NET-REST-API/DocumentManagement.API/publish/DocumentManagement.API.dll" "/tmp/DocumentManagement.API.dll"

run_remote_sudo_script "
mv /tmp/DocumentManagement.Data.dll $BACKEND_DIR/
mv /tmp/DocumentManagement.API.dll $BACKEND_DIR/
chown www-data:www-data $BACKEND_DIR/DocumentManagement.Data.dll
chown www-data:www-data $BACKEND_DIR/DocumentManagement.API.dll
"

echo ""
echo -e "${GREEN}Starting Backend Service...${NC}"
run_remote_sudo_script "systemctl start parliament-api"

sleep 3

echo ""
echo -e "${GREEN}Checking Backend Service Status...${NC}"
run_remote_sudo_script "systemctl status parliament-api --no-pager -l | head -20"

# Deploy frontend
echo ""
echo -e "${GREEN}Deploying External Frontend...${NC}"
cd external-frontend

# The build was already done, just upload it
run_remote_sudo_script "rm -rf $EXTERNAL_FRONTEND_REMOTE_DIR/build"
upload_file "build" "$EXTERNAL_FRONTEND_REMOTE_DIR/"

run_remote_sudo_script "
chown -R www-data:www-data $EXTERNAL_FRONTEND_REMOTE_DIR
systemctl reload apache2
"

cd ..

echo ""
echo -e "${GREEN}=========================================="
echo "Deployment Complete!"
echo "==========================================${NC}"
echo ""
echo -e "${GREEN}IMPORTANT: Users must log out and log back in for the token fix to take effect!${NC}"
echo ""
echo "Test the deployment:"
echo "1. Clear browser localStorage or use incognito mode"
echo "2. Login at https://ke-parliament-web.topdev.co.ke"
echo "3. Try uploading a petition"
echo ""
echo "External Frontend: https://ke-parliament-web.topdev.co.ke"
echo "Backend API: https://ke-parliament-api.topdev.co.ke"
echo ""
