# Backend
## Perequisites
1. Allow locally created scripts permanently.  
In powershell: `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser`

2. You need to have OpenSSL installed.  
If you have Git you can add the following User Env Path to use the OpenSSL from it.  
`C:\Program Files\Git\usr\bin\`

## Intial setup
1) Run `generate_certificates` to generate and trust a https certificate, from `scripts` folder.

## Fusion
After making changes to any of the microservices graphs you will have to generate and compose them again for the gateway.  
To do that you will have to run the `generate_pack_compose_fusion` script.

# Frontend
## Perequisites
Node.js
pnpm as your package manager (npm install -g pnpm)

## Initial setup
1) Open the frontend folder and run `npx auth secret`
2) It will create a `.env.local` file, now add all the missing variables from `.env.local.example`
3) Install packages `pnpm i`
4) Run using `pnpm dev`
