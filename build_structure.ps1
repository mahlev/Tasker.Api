$outFile = "$PWD\structure.txt"
$rootPath = $PWD.Path

"Section 1: Project Structure" | Out-File $outFile -Encoding UTF8
"============================" | Out-File $outFile -Encoding UTF8 -Append

function Get-Tree {
    param($path, $prefix)
    $items = Get-ChildItem -Path $path -Force | Where-Object {
        $_.Name -notin @('bin', 'obj', '.git', '.vs', 'node_modules', '.idea', 'structure.txt', 'build_structure.ps1')
    }
    $total = @($items).Count
    $i = 0
    foreach ($item in $items) {
        $i++
        $isLast = ($i -eq $total)
        $marker = $(if ($isLast) { "\--- " } else { "+--- " })
        $line = $prefix + $marker + $item.Name
        $line | Out-File $outFile -Encoding UTF8 -Append
        if ($item.PSIsContainer) {
            $childPrefix = $prefix + $(if ($isLast) { "     " } else { "|    " })
            Get-Tree $item.FullName $childPrefix
        }
    }
}

Split-Path $rootPath -Leaf | Out-File $outFile -Encoding UTF8 -Append
Get-Tree $rootPath ""

"`r`n`r`nSection 2: Code Inside" | Out-File $outFile -Encoding UTF8 -Append
"======================" | Out-File $outFile -Encoding UTF8 -Append

$allFiles = Get-ChildItem -Path $rootPath -Recurse -Force | Where-Object {
    -not $_.PSIsContainer -and
    $_.FullName -notmatch '\\(bin|obj|\.git|\.vs|node_modules|\.idea)(\\|$)' -and
    $_.Name -notin @('structure.txt', 'build_structure.ps1') -and
    $_.Extension -notmatch '\.(dll|exe|pdb|png|jpg|jpeg|gif|ico|zip|tar|gz|7z)$'
}

foreach ($f in $allFiles) {
    $relPath = $f.FullName.Substring($rootPath.Length + 1)
    "`r`n--- File: $relPath ---`r`n" | Out-File $outFile -Encoding UTF8 -Append
    try {
        $content = Get-Content $f.FullName -Raw -ErrorAction SilentlyContinue
        if ($null -ne $content) {
            $content | Out-File $outFile -Encoding UTF8 -Append
        }
    } catch {}
}
