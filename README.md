# Perequisites
1. To allow locally created scripts permanently:  
powershell  
`Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser`

2. You need to have OpenSSL installed.  
If you have Git you can add the following User Env Path to use the OpenSSL from it.  
C:\Program Files\Git\usr\bin\ 

# Intial setup
Run `generate_certificates` to generate and trust a https certificate

# Fusion
After making changes the microservices graphs you will have to generate and compose them again for the gateway  
To do that you will have to run the `generate_pack_compose_fusion` script
