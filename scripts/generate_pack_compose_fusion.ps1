<#
.SYNOPSIS
    Generates schemas, packs subgraphs, and composes the gateway for multiple microservices
    using HotChocolate Fusion.

.DESCRIPTION
    1. Checks if .NET is available.
    2. Checks (and installs, if needed) the hotchocolate.fusion.commandline tool.
    3. Iterates over each API to:
       - Generate the schema via `dotnet run --project ... -- schema export ...`
       - Pack the subgraph via `dotnet fusion subgraph pack ...`
    4. Composes the gateway via `dotnet fusion compose -p ... -s ...`
    5. Aborts if any external command fails (exit code != 0).
#>

[CmdletBinding()]
param (
    [Parameter(Mandatory=$false)]
    [System.Collections.Hashtable[]]
    $APIs = @(
        @{ Name = "Products"; Path = "../src/Microservices/Products/Products.API" },
        @{ Name = "User";     Path = "../src/Microservices/User/User.API" }
    ),

    [Parameter(Mandatory=$false)]
    [string]
    $GatewayPath = "../src/Gateway/Gateway.API/gateway"
)

# Make sure that any error in a PowerShell cmdlet stops execution
$ErrorActionPreference = 'Stop'

Write-Host "-----------------------------------------"
Write-Host "Checking if .NET is installed..."
Write-Host "-----------------------------------------"
try {
    # If this fails, .NET is not installed or not on PATH
    dotnet --list-sdks | Out-Null
    Write-Host ".NET is installed."
}
catch {
    Write-Error "It appears .NET is not installed or not on the PATH."
    exit 1
}

Write-Host "`n-----------------------------------------"
Write-Host "Checking hotchocolate.fusion.commandline..."
Write-Host "-----------------------------------------"
# Check if hotchocolate.fusion.commandline is installed
$toolList = & dotnet tool list -g
if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to check global .NET tools."
    exit 1
}

if (-not ($toolList | Select-String -SimpleMatch "hotchocolate.fusion.commandline")) {
    Write-Host "Not installed. Installing globally..."
    & dotnet tool install --global hotchocolate.fusion.commandline
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to install hotchocolate.fusion.commandline"
        exit 1
    }
    Write-Host "hotchocolate.fusion.commandline installed successfully."
}
else {
    Write-Host "hotchocolate.fusion.commandline is already installed."
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
$composeArgs = @("fusion", "compose", "-p", $GatewayPath)

foreach ($API in $APIs) {
    $composeArgs += @("-s", $API.Path)
}

Write-Host "Running: dotnet $($composeArgs -join ' ')"

# Call dotnet + the subcommand, passing the array as separate arguments
& dotnet $composeArgs
if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to compose the gateway."
    exit 1
}

Write-Host "`n--------------------------------------------"
Write-Host "All operations completed successfully."
Write-Host "--------------------------------------------"
