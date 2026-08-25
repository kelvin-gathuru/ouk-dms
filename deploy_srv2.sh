#!/bin/bash
set -e

# ============================================================
# OUK DMS - Deployment script for srv2.ouk.ac.ke
# Server: 197.136.12.98 (srv2)
# Layout:
#   Frontend -> /var/www/srv2.ouk.ac.ke/frontend
#   Backend  -> /var/www/srv2.ouk.ac.ke/api-bin
#   Nginx    -> /etc/nginx/sites-available/srv2.ouk.ac.ke
#   Service  -> ouk-dms-api.service (port 5001)
# ============================================================

SERVER_USER="ouk"
SERVER_PASS="ndUiwSTzK4WcmfRh"
SERVER_IP="197.136.12.98"
REMOTE_DIR="/var/www/srv2.ouk.ac.ke"
FRONTEND_DIR="AngularFrontend"
BACKEND_DIR="MySQL-NET-REST-API/DocumentManagement.API"
EMAIL="kgathuru@ouk.ac.ke"
NGINX_SITE="srv2.ouk.ac.ke"

echo "=== Starting OUK DMS Deployment (srv2.ouk.ac.ke) ==="

# 1. Build Backend
echo "--- Building Backend ---"
cd "$BACKEND_DIR"
rm -rf ./publish
dotnet publish -c Release -o ./publish
cd - > /dev/null

# 2. Build Frontend (srv2 configuration -> apiUrl https://srv2.ouk.ac.ke/)
echo "--- Building Frontend ---"
cd "$FRONTEND_DIR"
rm -rf dist/
if [ ! -d "node_modules" ]; then
    npm install
fi
npx ng build --configuration srv2
cd - > /dev/null

# 3. Prepare Remote Directories
echo "--- Preparing Server Directories ---"
sshpass -p "$SERVER_PASS" ssh -o StrictHostKeyChecking=no $SERVER_USER@$SERVER_IP "echo '$SERVER_PASS' | sudo -S mkdir -p $REMOTE_DIR/api-bin $REMOTE_DIR/frontend"
sshpass -p "$SERVER_PASS" ssh -o StrictHostKeyChecking=no $SERVER_USER@$SERVER_IP "echo '$SERVER_PASS' | sudo -S chown -R $SERVER_USER:$SERVER_USER $REMOTE_DIR/api-bin $REMOTE_DIR/frontend"

# 4. Transfer Files
echo "--- Transferring Backend Files ---"
sshpass -p "$SERVER_PASS" ssh -o StrictHostKeyChecking=no $SERVER_USER@$SERVER_IP "rm -rf $REMOTE_DIR/api-bin && mkdir -p $REMOTE_DIR/api-bin"
sshpass -p "$SERVER_PASS" scp -o StrictHostKeyChecking=no -r "$BACKEND_DIR/publish/"* $SERVER_USER@$SERVER_IP:$REMOTE_DIR/api-bin/

echo "--- Transferring Frontend Files ---"
sshpass -p "$SERVER_PASS" ssh -o StrictHostKeyChecking=no $SERVER_USER@$SERVER_IP "rm -rf $REMOTE_DIR/frontend && mkdir -p $REMOTE_DIR/frontend"
if [ -d "$FRONTEND_DIR/dist/document-management/browser" ]; then
    sshpass -p "$SERVER_PASS" scp -o StrictHostKeyChecking=no -r "$FRONTEND_DIR/dist/document-management/browser/"* $SERVER_USER@$SERVER_IP:$REMOTE_DIR/frontend/
else
    sshpass -p "$SERVER_PASS" scp -o StrictHostKeyChecking=no -r "$FRONTEND_DIR/dist/document-management/"* $SERVER_USER@$SERVER_IP:$REMOTE_DIR/frontend/
fi

# 5. Transfer Nginx Config
echo "--- Transferring Nginx Config ---"
sshpass -p "$SERVER_PASS" scp -o StrictHostKeyChecking=no srv2.ouk.ac.ke.conf $SERVER_USER@$SERVER_IP:/tmp/srv2.ouk.ac.ke.conf

# 6. Configure Server
echo "--- Configuring Server ---"
sshpass -p "$SERVER_PASS" ssh -o StrictHostKeyChecking=no $SERVER_USER@$SERVER_IP << EOF
    # Move nginx config
    echo '$SERVER_PASS' | sudo -S cp /tmp/srv2.ouk.ac.ke.conf /etc/nginx/sites-available/$NGINX_SITE
    echo '$SERVER_PASS' | sudo -S ln -sf /etc/nginx/sites-available/$NGINX_SITE /etc/nginx/sites-enabled/$NGINX_SITE
    echo '$SERVER_PASS' | sudo -S nginx -t && echo '$SERVER_PASS' | sudo -S systemctl reload nginx

    # Systemd service for the .NET API
    cat <<SERVICE > /tmp/ouk-dms-api.service
[Unit]
Description=OUK DMS API
After=network.target

[Service]
WorkingDirectory=$REMOTE_DIR/api-bin
ExecStart=/usr/bin/dotnet $REMOTE_DIR/api-bin/DocumentManagement.API.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=ouk-dms-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5001

[Install]
WantedBy=multi-user.target
SERVICE

    echo '$SERVER_PASS' | sudo -S mv /tmp/ouk-dms-api.service /etc/systemd/system/ouk-dms-api.service
    echo '$SERVER_PASS' | sudo -S systemctl daemon-reload
    echo '$SERVER_PASS' | sudo -S systemctl enable ouk-dms-api.service

    # Create required application directories
    echo '$SERVER_PASS' | sudo -S mkdir -p $REMOTE_DIR/api-bin/wwwroot/Signatures
    echo '$SERVER_PASS' | sudo -S mkdir -p $REMOTE_DIR/api-bin/wwwroot/Documents
    echo '$SERVER_PASS' | sudo -S mkdir -p $REMOTE_DIR/api-bin/wwwroot/FileRequestDocument
    echo '$SERVER_PASS' | sudo -S mkdir -p $REMOTE_DIR/api-bin/wwwroot/SearchIndex
    echo '$SERVER_PASS' | sudo -S mkdir -p $REMOTE_DIR/api-bin/wwwroot/OCRTemp
    echo '$SERVER_PASS' | sudo -S mkdir -p $REMOTE_DIR/api-bin/wwwroot/summary

    # Fix permissions before restart
    echo '$SERVER_PASS' | sudo -S chown -R www-data:www-data $REMOTE_DIR/api-bin
    echo '$SERVER_PASS' | sudo -S chown -R www-data:www-data $REMOTE_DIR/frontend
    echo '$SERVER_PASS' | sudo -S chmod -R 775 $REMOTE_DIR/api-bin/wwwroot

    echo '$SERVER_PASS' | sudo -S systemctl restart ouk-dms-api.service

    # Verify
    sleep 5
    echo "--- Health checks ---"
    curl -s -o /dev/null -w "API swagger: HTTP %{http_code}\n" http://localhost:5001/swagger/index.html || true
    curl -s -o /dev/null -w "Frontend: HTTP %{http_code}\n" https://$NGINX_SITE/ || true
EOF

echo "=== Deployment Complete (srv2.ouk.ac.ke) ==="
