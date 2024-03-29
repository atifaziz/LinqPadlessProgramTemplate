#!/usr/bin/env bash
set -e
cd "$(dirname "$0")"
if [[ ! -f lpless/dist/bin/lpless.dll ]]; then
    lpless/publish.sh
fi
dotnet build tests
dotnet test --no-restore --no-build tests "$@"
