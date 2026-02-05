#!/bin/bash
set -e

# Configuration
SERVER_IP="102.210.148.10"
USER="kelvin"
PASS="12Kelvin,"
REMOTE_DIR="/var/www/html/OukDms"
FRONTEND_DIR="$REMOTE_DIR/frontend/browser"

# Colors
GREEN='\033[0;32m'
NC='\033[0m' # No Color

echo -e "${GREEN}=== Redeploying Frontend for OUK DMS ===${NC}"

echo -e "${GREEN}--- Building Frontend ---${NC}"
cd AngularFrontend
npm install
npm run build -- --configuration production
cd ..

echo -e "${GREEN}--- Transferring Frontend Files ---${NC}"
sshpass -p "$PASS" scp -o StrictHostKeyChecking=no -r AngularFrontend/dist/document-management/browser/* $USER@$SERVER_IP:$FRONTEND_DIR/

echo -e "${GREEN}=== Frontend Redeployment Complete ===${NC}"
