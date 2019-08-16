[CmdletBinding(SupportsShouldProcess = $true)]
param([switch]$DontPublishLinqPadless = $false,
      [switch]$DontDeleteRepo = $false)

Push-Location $PSScriptRoot

try
{
    if (!$dontPublishLinqPadless) {
        & $env:COMSPEC /c call lpless/publish.cmd
    }

    if ($LASTEXITCODE) {
        throw "Publishing of LINQPadless failed with non-zero exit code of $LASTEXITCODE."
    }

    $dirs  = Get-Content keep | ? { $_ -like '*/' } | % { "^$([System.Text.RegularExpressions.Regex]::Escape($_))" }
    $files = Get-Content keep | ? { $_ -notlike '*/' } | % { "^$([System.Text.RegularExpressions.Regex]::Escape($_))$" }

    $files = $dirs + $files

    Get-ChildItem -File -Recurse  |
        Resolve-Path -Relative |
        % { ($_ -replace '\\', '/') -replace '^\./', '' } |
        ? {
            $file = $_
            @($files | ? { $file -match $_ }).Length -eq 0
        } |
        Remove-Item

    if (!$dontDeleteRepo -and (Test-Path -PathType Container .git)) {
        Remove-Item -Recurse -Force .git
    }

    Get-ChildItem -Recurse -Directory |
        ? { (Get-ChildItem -Recurse -File $_.FullName | Measure-Object).Count -eq 0 } |
        Remove-Item -Recurse -Force
}
finally
{
    Pop-Location
}
