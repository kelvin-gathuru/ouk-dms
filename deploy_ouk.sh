#!/bin/bash
set -e

SERVER_USER="kelvin"
SERVER_PASS="12Kelvin,"
SERVER_IP="102.210.148.10"
REMOTE_DIR="/var/www/html/OukDms"
FRONTEND_DIR="AngularFrontend"
BACKEND_DIR="MySQL-NET-REST-API/DocumentManagement.API"
EMAIL="kgathuru@ouk.ac.ke"

echo "=== Starting OUK DMS Deployment ==="

# 1. Build Backend
echo "--- Building Backend ---"
cd "$BACKEND_DIR"
rm -rf ./publish
dotnet publish -c Release -o ./publish
cd - > /dev/null

# 2. Build Frontend
echo "--- Building Frontend ---"
cd "$FRONTEND_DIR"
# Skip npm install if node_modules exists to save time, unless forced (can add flag later)
if [ ! -d "node_modules" ]; then
    npm install
fi
# Fix for possible memory issues or clean build
ng build --configuration production
cd - > /dev/null

# 3. Prepare Remote Directory
echo "--- Preparing Server Directories ---"
sshpass -p "$SERVER_PASS" ssh -o StrictHostKeyChecking=no $SERVER_USER@$SERVER_IP "echo '$SERVER_PASS' | sudo -S mkdir -p $REMOTE_DIR/api $REMOTE_DIR/frontend"
sshpass -p "$SERVER_PASS" ssh -o StrictHostKeyChecking=no $SERVER_USER@$SERVER_IP "echo '$SERVER_PASS' | sudo -S chown -R $SERVER_USER:$SERVER_USER $REMOTE_DIR"

# 4. Transfer Files
echo "--- Transferring Backend Files ---"
sshpass -p "$SERVER_PASS" scp -o StrictHostKeyChecking=no -r "$BACKEND_DIR/publish/"* $SERVER_USER@$SERVER_IP:$REMOTE_DIR/api/

echo "--- Transferring Frontend Files ---"
sshpass -p "$SERVER_PASS" scp -o StrictHostKeyChecking=no -r "$FRONTEND_DIR/dist/document-management/"* $SERVER_USER@$SERVER_IP:$REMOTE_DIR/frontend/

echo "--- Transferring Apache Config ---"
sshpass -p "$SERVER_PASS" scp -o StrictHostKeyChecking=no ouk-dms.conf $SERVER_USER@$SERVER_IP:/tmp/ouk-dms.conf

# 5. Configure Server
echo "--- Configuring Server ---"
sshpass -p "$SERVER_PASS" ssh -o StrictHostKeyChecking=no $SERVER_USER@$SERVER_IP << EOF
    # Move Apache config
    echo '$SERVER_PASS' | sudo -S mv /tmp/ouk-dms.conf /etc/apache2/sites-available/ouk-dms.conf
    
    # Enable Modules & Site
    echo '$SERVER_PASS' | sudo -S a2enmod proxy proxy_http rewrite ssl
    echo '$SERVER_PASS' | sudo -S a2ensite ouk-dms.conf
    
    # Restart Apache to apply HTTP config before Certbot
    echo '$SERVER_PASS' | sudo -S systemctl reload apache2

    # Setup Systemd for .NET
    cat <<SERVICE > /tmp/ouk-dms-api.service
[Unit]
Description=OUK DMS API
[Service]
WorkingDirectory=$REMOTE_DIR/api
ExecStart=/usr/bin/dotnet $REMOTE_DIR/api/DocumentManagement.API.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=ouk-dms-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5005
[Install]
WantedBy=multi-user.target
SERVICE

    echo '$SERVER_PASS' | sudo -S mv /tmp/ouk-dms-api.service /etc/systemd/system/ouk-dms-api.service

    # Start/Restart API Service
    echo '$SERVER_PASS' | sudo -S systemctl daemon-reload
    echo '$SERVER_PASS' | sudo -S systemctl enable ouk-dms-api.service
    echo '$SERVER_PASS' | sudo -S systemctl restart ouk-dms-api.service

    # Certbot SSL Setup
    echo "--- Running Certbot ---"
    # We use --apache plugin. Non-interactive.
    # We allow it to expand/update existing certs if any.
    echo '$SERVER_PASS' | sudo -S certbot --apache -d ouk-dms.topdev.co.ke -d ouk-dms-api.topdev.co.ke --non-interactive --agree-tos -m $EMAIL --redirect
EOF

echo "=== Deployment Complete ==="
