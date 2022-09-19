# ssh-mon - tool for linux server monitoring
using SSH-net for handling ssh connections https://github.com/sshnet/SSH.NET
It's using Private key authentication and AES encryption for sensitive data.
 
Usage:
1. Launch app for the first time and close it. Default config files will be created
2. Create file/files in "servers" folder, with connection information please use default file as an example
3. Generate RSA keypair. Put private key in "KEYFILE" file in "servers" folder and pub key in .ssh/authorized_keys on server
4. Run app and use option nuber one.<br>
5.(Optional) Encrypt Servers directory (recommended)<br>
6.(Optional) Use "in air" decryption when launching test (recommended)<br>
7.(Optional) Create your own module for more advanced tests<br>
![ssh_mon](https://user-images.githubusercontent.com/98389805/190985509-ceacb55a-07fb-4f68-b026-20e40f21ccec.png)
