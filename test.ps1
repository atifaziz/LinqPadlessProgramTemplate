$tests =
    Get-ChildItem .\tests\*.linq |
        % `
        {
            if ((Get-Content $_ | Out-String) -match '(?m:^//<\s*([0-9]+)\s*$)') {
                $exitCode = [int]$matches[1]
            } else {
                $exitCode = $null
            }

            $output =
                Get-Content $_ |
                    ? { $_ -match '(?m:^//\|\s*(.*?)\s*$)' } |
                    % { $matches[1] } |
                    Out-String

            [psobject]@{
                File             = $_;
                ExpectedExitCode = $exitCode;
                ExpectedOutput   = $output
            }
        }

$cmd = $env:ComSpec
$passCount = 0
$failCount = 0
$skipCount = 0

foreach ($test in $tests)
{
    Write-Output "==== $($test.File.Name)"

    $skip = $false

    if ($test.ExpectedExitCode -eq $null) {
        Write-Warning "$($test.File.Name): expected exit code is undefined; test will be skipped"
        $skip = $true
    }

    if (!$test.ExpectedOutput) {
        Write-Warning "$($test.File.Name): expected output is undefined; test will be skipped"
        $skip = $true
    }

    if ($skip) {
        $skipCount++
        continue;
    }

    $failed = $false

    & $cmd /c call lpless.cmd -x $test.File | Out-Null

    if ($LASTEXITCODE)
    {
        Write-Error "$($test.File.Name): compilation failed"
        $failed = $true
    }
    else
    {
        $actualOutput = & $cmd /c call lpless.cmd $test.File foo bar baz | Out-String

        if ($LASTEXITCODE -ne $test.ExpectedExitCode)
        {
            $failed = $true
            Write-Error "$($test.File.Name): exit code mismatch; expected = $($test.ExpectedExitCode); actual = $LASTEXITCODE"
        }

        if ($actualOutput -ne $test.ExpectedOutput)
        {
            $failed = $true
            Write-Output "EXPECTED:"
            Write-Output $test.ExpectedOutput
            Write-Output "ACTUAL:"
            Write-Output $actualOutput
            Write-Error "$($test.File.Name): output mismatch"
        }
    }

    if ($failed) {
        $failCount++
    } else {
        $passCount++
    }
}

$count = $tests.Length

Write-Output `
"
*************** SUMMARY ***************
Passed  = $passCount/$count
Failed  = $failCount/$count
Skipped = $skipCount/$count
"
