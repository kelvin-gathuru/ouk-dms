# Deployment Guide for Parliament System

This guide outlines the steps to deploy the Parliament System (Angular Frontend + .NET Core Backend) to a Linux server (Ubuntu/Debian assumed).

## System Architecture
- **Frontend URL**: `https://na.topdev.co.ke`
- **Backend URL**: `https://na-api.topdev.co.ke`
- **Database**: MySQL

## 1. Prerequisites
Ensure your server has the following installed:
- **Nginx**: Web server and reverse proxy.
- **MySQL Server**: Database.
- **.NET 8.0 SDK/Runtime**: To run the backend.
- **Node.js & npm**: To build the frontend (can be done locally).
- **Certbot**: For SSL certificates.

### Install Dependencies (Ubuntu)
```bash
sudo apt update
sudo apt install nginx mysql-server certbot python3-certbot-nginx -y

# Install .NET 8.0 (Adjust if using a different version)
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install -y dotnet-sdk-8.0
```

## 2. Database Setup

1.  **Secure MySQL**:
    ```bash
    sudo mysql_secure_installation
    ```

2.  **Create Database and User**:
    Log in to MySQL:
    ```bash
    sudo mysql -u root -p
    ```
    Run the following SQL commands:
    ```sql
    CREATE DATABASE document_management_db;
    CREATE USER 'document_management_user'@'localhost' IDENTIFIED BY 'DocMgmt@12Kelvin';
    GRANT ALL PRIVILEGES ON document_management_db.* TO 'document_management_user'@'localhost';
    FLUSH PRIVILEGES;
    EXIT;
    ```

3.  **Import Data**:
    Upload the `document_management_db.sql` file to your server and import it:
    ```bash
    mysql -u document_management_user -p document_management_db < document_management_db.sql
    ```

## 3. Backend Deployment (.NET API)

1.  **Publish the API**:
    Run this command in your local `MySQL-NET-REST-API/DocumentManagement.API` directory:
    ```bash
    dotnet publish -c Release -o ./publish
    ```

2.  **Upload to Server**:
    Copy the contents of the `./publish` folder to `/var/www/na-api.topdev.co.ke` on your server.
    ```bash
    sudo mkdir -p /var/www/na-api.topdev.co.ke
    # Use SCP or SFTP to upload files
    ```

3.  **Set Permissions**:
    ```bash
    sudo chown -R www-data:www-data /var/www/na-api.topdev.co.ke
    sudo chmod -R 755 /var/www/na-api.topdev.co.ke
    ```

4.  **Create Systemd Service**:
    Create a service file to keep the app running.
    ```bash
    sudo nano /etc/systemd/system/parliament-api.service
    ```
    Paste the following:
    ```ini
    [Unit]
    Description=Parliament System API
    
    [Service]
    WorkingDirectory=/var/www/na-api.topdev.co.ke
    ExecStart=/usr/bin/dotnet /var/www/na-api.topdev.co.ke/DocumentManagement.API.dll
    Restart=always
    # Restart service after 10 seconds if the dotnet service crashes:
    RestartSec=10
    KillSignal=SIGINT
    SyslogIdentifier=parliament-api
    User=www-data
    Environment=ASPNETCORE_ENVIRONMENT=Production
    Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
    
    [Install]
    WantedBy=multi-user.target
    ```

5.  **Start the Service**:
    ```bash
    sudo systemctl enable parliament-api.service
    sudo systemctl start parliament-api.service
    sudo systemctl status parliament-api.service
    ```

## 4. Frontend Deployment (Angular)

1.  **Build the App**:
    Run this in your local `AngularFrontend` directory:
    ```bash
    ng build --configuration production
    ```

2.  **Upload to Server**:
    Copy the contents of `dist/document-management` (or similar inside `dist`) to `/var/www/na.topdev.co.ke`.
    ```bash
    sudo mkdir -p /var/www/na.topdev.co.ke
    # Use SCP or SFTP to upload files
    ```

3.  **Set Permissions**:
    ```bash
    sudo chown -R www-data:www-data /var/www/na.topdev.co.ke
    ```

## 5. Nginx Configuration

Create a configuration file for both sites.

1.  **Create Config**:
    ```bash
    sudo nano /etc/nginx/sites-available/topdev.conf
    ```

2.  **Paste Configuration**:
    ```nginx
    # Backend API
    server {
        server_name na-api.topdev.co.ke;
    
        location / {
            proxy_pass http://localhost:5000; # Default .NET port, check appsettings if different
            proxy_http_version 1.1;
            proxy_set_header Upgrade $http_upgrade;
            proxy_set_header Connection keep-alive;
            proxy_set_header Host $host;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
        }
    }
    
    # Frontend
    server {
        server_name na.topdev.co.ke;
        root /var/www/na.topdev.co.ke;
        index index.html;
    
        location / {
            try_files $uri $uri/ /index.html;
        }
    }
    ```
    *Note: The .NET app usually listens on port 5000 by default. If your `appsettings.json` specifies a different port (e.g., 44313), update the `proxy_pass` directive or configure Kestrel in `appsettings.Production.json` to listen on 5000.*

3.  **Enable Site**:
    ```bash
    sudo ln -s /etc/nginx/sites-available/topdev.conf /etc/nginx/sites-enabled/
    sudo nginx -t
    sudo systemctl reload nginx
    ```

## 6. SSL Setup (HTTPS)

Run Certbot to obtain certificates and automatically configure Nginx.

```bash
sudo certbot --nginx -d na.topdev.co.ke -d na-api.topdev.co.ke
```

Follow the prompts. Certbot will update your Nginx config to force HTTPS.

## 7. Verification

1.  Visit `https://na.topdev.co.ke`. You should see the login page.
2.  Open Developer Tools (F12) -> Network tab.
3.  Try to log in. You should see requests going to `https://na-api.topdev.co.ke/...`.

## Code Changes Made
I have already applied the following changes to your local code:
1.  **Frontend**: Updated `src/environments/environment.prod.ts` to point to `https://na-api.topdev.co.ke/`.
2.  **Backend**: Created `appsettings.Production.json` with:
    -   `CorsUrls`: `https://na.topdev.co.ke`
    -   `JwtSettings:issuer`: `https://na-api.topdev.co.ke`
