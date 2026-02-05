#!/bin/bash
# Script to deploy the server-side build

PASSWORD="12Kelvin,"

run_sudo() {
    echo "$PASSWORD" | sudo -S "$@" 2>&1
}

BUILD_PATH="/home/kelvin/ParliamentSystem_New/AngularFrontend/dist/document-management/browser"
DEPLOY_PATH="/var/www/html/ParliamentSystem/frontend/browser"

echo "=== Deploying new build ==="
if [ -d "$BUILD_PATH" ]; then
    echo "Build found at $BUILD_PATH"
    
    # Backup existing deployment
    if [ -d "$DEPLOY_PATH" ]; then
        run_sudo cp -r "$DEPLOY_PATH" "${DEPLOY_PATH}_backup_$(date +%Y%m%d_%H%M%S)"
    fi
    
    # Copy new files
    run_sudo rm -rf "$DEPLOY_PATH"/*
    run_sudo cp -r "$BUILD_PATH"/* "$DEPLOY_PATH/"
    
    # Set permissions
    run_sudo chown -R www-data:www-data "$DEPLOY_PATH"
    run_sudo chmod -R 755 "$DEPLOY_PATH"
    
    echo "Deployment complete!"
else
    echo "Error: Build directory not found at $BUILD_PATH"
    exit 1
fi
