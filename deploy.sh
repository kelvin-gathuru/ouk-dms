#!/bin/bash
set -e

# Configuration
SERVER_USER="kelvin"
SERVER_IP="102.210.148.10"
REMOTE_DIR="/var/www/html/ParliamentSystem"
FRONTEND_DIR="AngularFrontend"
BACKEND_DIR="MySQL-NET-REST-API/DocumentManagement.API"

echo "=== Starting Deployment ==="

# 1. Build Backend
echo "--- Building Backend ---"
cd $BACKEND_DIR
dotnet publish -c Release -o ./publish
cd ../..

# 2. Build Frontend
echo "--- Building Frontend ---"
cd $FRONTEND_DIR
# Ensure dependencies are installed
if [ ! -d "node_modules" ]; then
    npm install
fi
ng build --configuration production
cd ..

# 3. Prepare Remote Directory
echo "--- Preparing Server Directories ---"
ssh $SERVER_USER@$SERVER_IP "echo '12Kelvin,' | sudo -S mkdir -p $REMOTE_DIR/api $REMOTE_DIR/frontend && echo '12Kelvin,' | sudo -S chown -R $SERVER_USER:$SERVER_USER $REMOTE_DIR"

# 4. Transfer Files
echo "--- Transferring Backend Files ---"
scp -r $BACKEND_DIR/publish/* $SERVER_USER@$SERVER_IP:$REMOTE_DIR/api/

echo "--- Transferring Frontend Files ---"
scp -r $FRONTEND_DIR/dist/document-management/* $SERVER_USER@$SERVER_IP:$REMOTE_DIR/frontend/

echo "--- Transferring Config Files ---"
scp na.conf $SERVER_USER@$SERVER_IP:/tmp/na.conf

# 5. Configure Server
echo "--- Configuring Server ---"
ssh $SERVER_USER@$SERVER_IP << 'EOF'
    # Move Apache config
    echo '12Kelvin,' | sudo -S mv /tmp/na.conf /etc/apache2/sites-available/na.conf
    
    # Enable Modules & Site
    echo '12Kelvin,' | sudo -S a2enmod proxy proxy_http rewrite ssl
    echo '12Kelvin,' | sudo -S a2ensite na.conf
    
    # Restart Apache
    echo '12Kelvin,' | sudo -S systemctl reload apache2

    # Setup Systemd for .NET
    cat <<SERVICE > /tmp/parliament-api.service
[Unit]
Description=Parliament System API
[Service]
WorkingDirectory=$REMOTE_DIR/api
ExecStart=/usr/bin/dotnet $REMOTE_DIR/api/DocumentManagement.API.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=parliament-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000
[Install]
WantedBy=multi-user.target
SERVICE

    echo '12Kelvin,' | sudo -S mv /tmp/parliament-api.service /etc/systemd/system/parliament-api.service

    # Start/Restart API Service
    echo '12Kelvin,' | sudo -S systemctl daemon-reload
    echo '12Kelvin' | sudo -S systemctl enable parliament-api.service
    echo '12Kelvin' | sudo -S systemctl restart parliament-api.service
EOF

echo "=== Deployment Complete ==="
