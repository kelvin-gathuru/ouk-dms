#!/bin/bash
set -e
SERVER_USER="kelvin"
SERVER_PASS="12Kelvin,"
SERVER_IP="102.210.148.10"
REMOTE_DIR="/var/www/html/OukDms"
BACKEND_DIR="MySQL-NET-REST-API/DocumentManagement.API"

echo "=== Deploying Backend Fix ==="
cd "$BACKEND_DIR"
rm -rf ./publish
dotnet publish -c Release -o ./publish
cd - > /dev/null

echo "--- Transferring Backend Files ---"
sshpass -p "$SERVER_PASS" scp -o StrictHostKeyChecking=no -r "$BACKEND_DIR/publish/"* $SERVER_USER@$SERVER_IP:$REMOTE_DIR/api/

echo "--- Restarting Service ---"
sshpass -p "$SERVER_PASS" ssh -o StrictHostKeyChecking=no $SERVER_USER@$SERVER_IP "echo '$SERVER_PASS' | sudo -S systemctl restart ouk-dms-api.service"
sshpass -p "$SERVER_PASS" ssh -o StrictHostKeyChecking=no $SERVER_USER@$SERVER_IP "echo '$SERVER_PASS' | sudo -S systemctl status ouk-dms-api.service --no-pager"
