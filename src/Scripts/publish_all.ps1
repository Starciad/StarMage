# --------------------------------------------- 
# Star Mage Build Pipeline
# ---------------------------------------------

# Clear console
Clear-Host

# Configuration
$gameName = 'StarMage'
$outputDir = '..\Publish'

# Project definitions
$projects = @(
    @{ Name='Windows'; Path='..\StarMage.Windows\StarMage.Windows.csproj' },
    @{ Name='Linux'; Path='..\StarMage.Linux\StarMage.Linux.csproj' }
)

# Clean output directory
if (Test-Path $outputDir) {
    Remove-Item $outputDir -Recurse -Force
}

# Function: publish + post-process
function Publish-Project {
    param (
        [string]$projectName,
        [string]$projectPath
    )

    $publishDir = Join-Path $outputDir "$gameName.$projectName"
    dotnet publish $projectPath -c Release --output $publishDir
    
	if ($LASTEXITCODE -ne 0) {
        return
    }
}

# Execute publishes
foreach ($proj in $projects) {
    Publish-Project -projectName $proj.Name -projectPath $proj.Path
}
