#!/bin/bash

set -e # -e  Exit immediately if a command exits with a non-zero status.

dotnet tool install --global dotnet-ef
export PATH="$PATH:/root/.dotnet/tools"

ls

until dotnet ef database update --verbose; do
>&2 echo "SQL Server is starting up"
sleep 10 # number of seconds
done

>&2 echo "MySQL is up"

run_cmd="dotnet watch run --urls http://*:80"
exec $run_cmd
