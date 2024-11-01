#!/usr/bin/env bash

set -eu
set -o pipefail

dotnet tool restore
dotnet paket restore
dotnet run -v:m --project ./build/build.fsproj -- -t "$@"