dotnet ef --project ../App database update 0
dotnet ef --project ../App migrations remove
dotnet ef --project ../App migrations add InititalCreate
dotnet ef --project ../App database update
pause