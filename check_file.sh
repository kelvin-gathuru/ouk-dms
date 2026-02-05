#!/bin/bash
SERVER_IP="102.210.149.248"
USER="tito"
PASS="tiro123."
REMOTE_DIR="/var/www/html/ParliamentSystem/api/Documents"

sshpass -p "$PASS" ssh -o StrictHostKeyChecking=no $USER@$SERVER_IP "ls -ld $REMOTE_DIR"
