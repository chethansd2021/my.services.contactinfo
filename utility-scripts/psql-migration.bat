@echo off 
set PROJECT_PATH=%~dp0..\src\my.services.contactinfo.Data.Postgres.Migrations\my.services.contactinfo.Data.Postgres.Migrations.csproj

echo Add a posgreSQL code first migration. 
set /p migrationName=Name of migration [CamelCased]?: 

echo:
dotnet ef migrations add %migrationName% --project %PROJECT_PATH%

echo:
echo Migration Added. 