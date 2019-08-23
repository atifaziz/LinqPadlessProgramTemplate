@echo off
setlocal

set LPLESS=%~dp0lpless\dist\bin\lpless.dll
if not exist "%LPLESS%" call lpless\publish
if not %ERRORLEVEL%==0 exit /b %ERRORLEVEL%

pushd "%~dp0"
dotnet build tests && dotnet test --no-restore --no-build tests %*
popd
exit /b %ERRORLEVEL%
