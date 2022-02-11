dotnet ef --project ../ database update 0
dotnet ef --project ../ migrations remove
dotnet ef --project ../ migrations add InititalCreate
dotnet ef --project ../ database update
pause