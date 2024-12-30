Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Write-Host "=================================================="
Write-Host "  Generating and Trusting ASP.NET Core Dev Certs  "
Write-Host "=================================================="

# --------------------------------------------------
# 1. Configuration
# --------------------------------------------------

$exportDir = "..\certificates\"
$exportCrt = "localhost.crt"

# Create the directory if it does not exist
if (-not (Test-Path -Path $exportDir)) {
    New-Item -ItemType Directory -Path $exportDir -Force
    Write-Host "Directory created: $exportDir"
} else {
    Write-Host "Directory already exists: $exportDir"
}

# ----------------------------------------------------------------------------
# 2. Generate and trust a new dev certificate
# ----------------------------------------------------------------------------
Write-Host "`nGenerating and trusting new ASP.NET Core HTTPS development certificate..."
dotnet dev-certs https -ep $exportDir$exportCrt --trust --format PEM


Write-Host "`nDone! Certificate $exportCrt (PEM format) exported to:"
Write-Host $exportDir$exportCrt