yarn
yarn workspace codesanook-ef-note run dev
dotnet ef database update --project ./src/Codesanook.EFNote/Codesanook.EFNote.csproj
dotnet watch run --project ./src/Codesanook.EFNote/Codesanook.EFNote.csproj
