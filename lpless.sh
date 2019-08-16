#!/usr/bin/env bash
set -e
cd "$(dirname "$0")"
LPLESS=lpless/dist/bin/lpless.dll
if [[ -f $LPLESS ]]; then
    dotnet $LPLESS "$@"
else
    echo "LINQPadless executable is missing.">&2
    echo "Build it by running: lpless/publish.sh">&2
    exit 1
fi
