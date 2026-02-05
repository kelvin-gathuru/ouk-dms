#!/bin/bash
SERVER_IP="102.210.149.248"
USER="tito"
FRONTEND_DEST="/var/www/html/ParliamentSystem/frontend/browser"

echo "Deploying Frontend..."
sshpass -p "tiro123." scp -o StrictHostKeyChecking=no -r /home/k3lv1n/Documents/CHEPKEN/ParliamentSystem/AngularFrontend/dist/document-management/browser/* $USER@$SERVER_IP:$FRONTEND_DEST/

echo "Deployment Complete!"
