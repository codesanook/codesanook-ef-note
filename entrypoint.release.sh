#!/bin/bash
set -e # -e  Exit immediately if a command exits with a non-zero status.
sleep 10 # Explicit wait in number of seconds

pwd
ls
dotnet Codesanook.EFNote.dll
