#!/bin/bash
# entrypoint.sh

# Exit immediately if a command exits with a non-zero status.
set -e

# Run initialize.mssql.sh and start SQL Server
./initialize.mssql.sh & /opt/mssql/bin/sqlservr
