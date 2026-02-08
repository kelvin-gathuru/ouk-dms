#!/bin/bash
set -e

# Configuration
SERVER_IP="102.210.149.248"
USER="tito"
PASS="tiro123."
REMOTE_DIR="/var/www/html/ParliamentSystem/frontend/browser"
LOCAL_DIR="/home/k3lv1n/Documents/OUK DMS/ParliamentSystem/AngularFrontend/dist/document-management/browser"

# Colors
GREEN='\033[0;32m'
NC='\033[0m' # No Color

echo -e "${GREEN}=== Deploying Frontend to $SERVER_IP ===${NC}"

# Change ownership to tito
echo -e "${GREEN}--- Setting Permissions for tito ---${NC}"
sshpass -p "$PASS" ssh -o StrictHostKeyChecking=no $USER@$SERVER_IP "echo '$PASS' | sudo -S chown -R $USER:$USER $REMOTE_DIR"

if [ ! -d "$LOCAL_DIR" ]; then
    echo "Error: Local directory $LOCAL_DIR does not exist."
    exit 1
fi

echo -e "${GREEN}--- Transferring Frontend Files ---${NC}"
sshpass -p "$PASS" scp -o StrictHostKeyChecking=no -r "$LOCAL_DIR/"* $USER@$SERVER_IP:$REMOTE_DIR/

# Change ownership back to www-data
echo -e "${GREEN}--- Restoring Permissions for www-data ---${NC}"
sshpass -p "$PASS" ssh -o StrictHostKeyChecking=no $USER@$SERVER_IP "echo '$PASS' | sudo -S chown -R www-data:www-data $REMOTE_DIR"

echo -e "${GREEN}=== Deployment Complete ===${NC}"
