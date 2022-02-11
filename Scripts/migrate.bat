dotnet ef --project ../Service database update 0
dotnet ef --project ../Service migrations remove
dotnet ef --project ../Service migrations add InititalCreate
dotnet ef --project ../Service database update
pause