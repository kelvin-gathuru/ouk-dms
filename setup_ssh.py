import pty
import os
import sys
import time

def setup_ssh(user, host, password):
    # Check if key exists, if not generate it
    key_path = os.path.expanduser('~/.ssh/id_rsa')
    if not os.path.exists(key_path):
        print("Generating SSH key...")
        os.system('ssh-keygen -t rsa -N "" -f ~/.ssh/id_rsa')

    cmd = f'ssh-copy-id -o StrictHostKeyChecking=no {user}@{host}'
    print(f"Running: {cmd}")
    
    pid, fd = pty.fork()
    
    if pid == 0:
        # Child process
        os.execvp('bash', ['bash', '-c', cmd])
    else:
        # Parent process
        output = b""
        while True:
            try:
                chunk = os.read(fd, 1024)
                if not chunk:
                    break
                output += chunk
                sys.stdout.write(chunk.decode('utf-8', errors='ignore'))
                
                if b"password:" in output:
                    os.write(fd, (password + "\n").encode())
                    output = b"" # Reset buffer to avoid sending password multiple times
                elif b"Are you sure you want to continue connecting" in output:
                    os.write(fd, b"yes\n")
                    output = b""
            except OSError:
                break
        
        _, status = os.waitpid(pid, 0)
        if os.WEXITSTATUS(status) == 0:
            print("\nSSH key copied successfully!")
        else:
            print("\nFailed to copy SSH key.")
            sys.exit(1)

if __name__ == "__main__":
    setup_ssh("kelvin", "102.210.148.10", "12Kelvin")
