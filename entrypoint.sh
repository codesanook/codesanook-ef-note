#!/bin/sh

set -e # -e  Exit immediately if a command exits with a non-zero status.

dotnet tool install --global dotnet-ef
export PATH="$PATH:/root/.dotnet/tools"

until dotnet ef database update --verbose; do
>&2 echo "MySQL is starting up"
sleep 5 # number of seconds
done

>&2 echo "MySQL is up"

dotnet --version
echo "$ASPNETCORE_AUTO_RELOAD_WS_ENDPOINT"

dotnet watch run --verbose --no-launch-profile
