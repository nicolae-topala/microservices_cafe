[CmdletBinding()]
param (
    [Parameter(Mandatory=$false)]
    [System.Collections.Hashtable[]]
    $APIs = @(
        @{ Name = "User";     Path = "../src/Microservices/User/User.API" },
        @{ Name = "Products"; Path = "../src/Microservices/Products/Products.API" },
        @{ Name = "Inventory"; Path = "../src/Microservices/Inventory/Inventory.API" },
        @{ Name = "Price";    Path = "../src/Microservices/Price/Price.API" }
    ),

    [Parameter(Mandatory=$false)]
    [string]
    $GatewayPath = "../src/Gateway/Gateway.API"
)

# Make sure that any error in a PowerShell cmdlet stops execution
$ErrorActionPreference = 'Stop'

Write-Host "`n-----------------------------------------"
Write-Host "Restore tools..."
Write-Host "-----------------------------------------"

dotnet tool restore
if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to restore tools"
        exit 1
}

Write-Host "`n-----------------------------------------"
Write-Host "Starting schema generation and subgraph packing..."
Write-Host "-----------------------------------------"

foreach ($API in $APIs) {
    $APIName = $API.Name
    $APIPath = $API.Path
    
    Write-Host "`n--------------------------------------------"
    Write-Host "Processing '$APIName' at path: $APIPath"
    Write-Host "--------------------------------------------"

    #
    # 1) Generate schema
    #
    Write-Host "Generating schema for '$APIName' API..."
    & dotnet run --project $APIPath -- schema export --output schema.graphql
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to generate schema for '$APIName' API."
        exit 1
    }
    Write-Host "Schema for '$APIName' generated successfully."

    #
    # 2) Pack subgraph
    #
    Write-Host "Packing subgraph for '$APIName' API..."
    & dotnet fusion subgraph pack -w $APIPath
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to pack subgraph for '$APIName' API."
        exit 1
    }
    Write-Host "Subgraph for '$APIName' packed successfully."
}

Write-Host "`n--------------------------------------------"
Write-Host "Composing the gateway with all subgraphs..."
Write-Host "--------------------------------------------"

# Build an array of arguments for the 'fusion compose' command
$composeArgs = @("fusion", "compose", "-p", "$GatewayPath/gateway")

foreach ($API in $APIs) {
    $composeArgs += @("-s", $API.Path)
}

Write-Host "Running: dotnet $composeArgs"

# Call dotnet + the subcommand, passing the array as separate arguments
& dotnet $composeArgs
if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to compose the gateway."
    exit 1
}

Write-Host "`n--------------------------------------------"
Write-Host "Generate and save Gateway schema for frontend..."
Write-Host "--------------------------------------------"

Write-Host "Generating schema for Gateway..."
& dotnet run --project $GatewayPath -- schema export --output ../../../../frontend/schema.graphql
if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to generate schema for Gateway."
    exit 1
}
Write-Host "Schema for Gateway generated successfully."

Write-Host "`n--------------------------------------------"
Write-Host "All operations completed successfully."
Write-Host "--------------------------------------------"

Write-Host "Press Enter to exit..."
Read-Host