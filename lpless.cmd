@echo off
setlocal
set LPLESS=%~dp0lpless\dist\bin\lpless.dll
if not exist "%LPLESS%" goto :missing
dotnet "%LPLESS%" %*
goto :EOF

:missing
echo LINQPadless binary is missing.>&2
echo Build it by running: lpless\publish>&2
goto :EOF
