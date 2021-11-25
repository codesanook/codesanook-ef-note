#!/bin/bash
# initialize.mssql.sh

# How to connect to SQL Server:
# https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash#connect-to-sql-server
# sqlcmd Utility options: https://docs.microsoft.com/en-us/sql/tools/sqlcmd-utility?view=sql-server-ver15#syntax

INPUT_SQL_FILE="init-db.mssql.sql"

until /opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P "$MSSQL_SA_PASSWORD" -i $INPUT_SQL_FILE > /dev/null 2>&1
do
  echo -e "\n\033[31mSQL server is unavailable - sleeping."
  sleep 1 # Sleep for a second
done

echo -e "\n\033[31mInitializing a database has done."
