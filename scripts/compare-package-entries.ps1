<#
.SYNOPSIS
    Compares two .nupkg files entry by entry.

.DESCRIPTION
    Two packs of the same commit are never byte-identical as FILES: a .nupkg is a zip and
    NuGet stamps every entry with its file mtime, so the container bytes differ even when
    the contents do not. The comparison that means something is entry by entry, which is
    what this script does.

    Two entries are treated specially, both for reasons that have nothing to do with the
    build:

      .signature.p7s        nuget.org's repository signature, added after the build. A
                            package downloaded from nuget.org has it; a package you just
                            packed does not. Ignored.
      *.psmdcp              OPC core properties. The FILE NAME is a GUID on some NuGet
                            versions and the fixed 'nuget.psmdcp' on others, so the entry
                            is compared under a stable key instead of its name.

    Exits 0 when every remaining entry matches, 1 otherwise, listing what differed.

.PARAMETER Mine
    The package you built.

.PARAMETER Published
    The package to compare against - typically downloaded from nuget.org.

.EXAMPLE
    pwsh ./scripts/compare-package-entries.ps1 `
        -Mine ./packages/Wolfgang.D20.Dice.<version>.nupkg `
        -Published ./wolfgang.d20.dice.<version>.nupkg

.NOTES
    See REPRODUCIBLE-BUILD.md for the full verification procedure, including which
    differences are expected and which are worth reporting.
#>

param
(
    [Parameter(Mandatory)] [string] $Mine,
    [Parameter(Mandatory)] [string] $Published
)

$ErrorActionPreference = 'Stop'
$sha256 = [System.Security.Cryptography.SHA256]::Create()

function Get-PackageEntryHashes([string] $package)
{
    $hashes = @{}
    $archive = [System.IO.Compression.ZipFile]::OpenRead((Resolve-Path -LiteralPath $package))

    try
    {
        foreach ($entry in $archive.Entries)
        {
            if ($entry.FullName -eq '.signature.p7s')
            {
                continue
            }

            $key = if ($entry.FullName.EndsWith('.psmdcp')) { 'OPC core properties' } else { $entry.FullName }
            $stream = $entry.Open()

            try
            {
                $hashes[$key] = (($sha256.ComputeHash($stream) | ForEach-Object { $_.ToString('x2') }) -join '')
            }
            finally
            {
                $stream.Dispose()
            }
        }
    }
    finally
    {
        $archive.Dispose()
    }

    return $hashes
}

# Deliberately not $mine / $published: PowerShell variable names are case-insensitive, so
# assigning to those would assign THROUGH the [string] parameters above and silently
# stringify the hashtables.
$mineHashes = Get-PackageEntryHashes $Mine
$publishedHashes = Get-PackageEntryHashes $Published

$differences = @(
    @($mineHashes.Keys) + @($publishedHashes.Keys) |
        Sort-Object -Unique |
        Where-Object { $mineHashes[$_] -ne $publishedHashes[$_] }
)

if ($differences.Count -eq 0)
{
    Write-Host "All $($mineHashes.Count) package entries are identical."
    exit 0
}

Write-Host "$($differences.Count) of $($mineHashes.Count) entries differ:"

foreach ($difference in $differences)
{
    Write-Host "  $difference"
}

exit 1
