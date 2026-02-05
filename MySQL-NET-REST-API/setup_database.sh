#!/bin/bash

# Database Setup Script for Document Management Application
# This script creates the database, user, and imports the schema

echo "Setting up MySQL database for Document Management Application..."

# Create database and user with stronger password
sudo mysql << EOF
-- Temporarily set password policy to LOW
SET GLOBAL validate_password.policy=LOW;
SET GLOBAL validate_password.length=6;

-- Create database
CREATE DATABASE IF NOT EXISTS document_management_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Create user with password
CREATE USER IF NOT EXISTS 'document_management_user'@'localhost' IDENTIFIED BY 'DocMgmt@12Kelvin';

-- Grant privileges
GRANT ALL PRIVILEGES ON document_management_db.* TO 'document_management_user'@'localhost';
FLUSH PRIVILEGES;

-- Show databases
SELECT 'Database created successfully' AS Status;
EOF

if [ $? -eq 0 ]; then
    echo "✓ Database and user created successfully"
    echo "  Database: document_management_db"
    echo "  User: document_management_user"
    echo "  Password: DocMgmt@12Kelvin"
else
    echo "✗ Failed to create database and user"
    exit 1
fi

# Import the schema
echo ""
echo "Importing database schema from MySql.sql..."
mysql -u document_management_user -pDocMgmt@12Kelvin document_management_db < MySql.sql 2>&1 | grep -v "Using a password"

if [ $? -eq 0 ]; then
    echo "✓ Database schema imported successfully"
    echo ""
    echo "========================================="
    echo "Database setup complete!"
    echo "========================================="
    echo "Connection Details:"
    echo "  Server: localhost"
    echo "  Port: 3306"
    echo "  Database: document_management_db"
    echo "  User: document_management_user"
    echo "  Password: DocMgmt@12Kelvin"
    echo ""
    echo "Default Login Credentials:"
    echo "  Admin: admin@gmail.com / Admin@123"
    echo "  Employee: employee@gmail.com / Employee@123"
    echo "========================================="
else
    echo "✗ Failed to import database schema"
    exit 1
fi
