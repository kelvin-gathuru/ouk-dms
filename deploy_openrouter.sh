#!/bin/bash

# Server Details
SERVER_IP="102.210.148.10"
USER="kelvin"
PASS="12Kelvin,"
BACKEND_DEST="/home/kelvin/ParliamentSystem_New/MySQL-NET-REST-API/DocumentManagement.API"
FRONTEND_DEST="/var/www/html/ParliamentSystem/frontend/browser"

echo "Stopping Backend Service..."
ssh $USER@$SERVER_IP "echo $PASS | sudo -S systemctl stop na-api.service"

echo "Deploying Backend..."
scp -r /home/k3lv1n/Documents/CHEPKEN/ParliamentSystem/MySQL-NET-REST-API/DocumentManagement.API/publish/* $USER@$SERVER_IP:$BACKEND_DEST/

echo "Deploying Frontend..."
scp -r /home/k3lv1n/Documents/CHEPKEN/ParliamentSystem/AngularFrontend/dist/document-management/browser/* $USER@$SERVER_IP:$FRONTEND_DEST/

echo "Starting Backend Service..."
ssh $USER@$SERVER_IP "echo $PASS | sudo -S systemctl start na-api.service"

echo "Deployment Complete!"
