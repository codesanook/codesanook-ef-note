#!/bin/sh

ls /root/.dotnet/tools/

set -e # -e  Exit immediately if a command exits with a non-zero status.
export PATH="$PATH:/root/.dotnet/tools"

until dotnet ef database update --verbose
do
  echo -e "\033[31mA database server is unavailable - sleeping."
  sleep 1 # Number of seconds
done
echo -e "\033[31mDatabase migration has finished"

dotnet watch run --verbose --no-launch-profile
