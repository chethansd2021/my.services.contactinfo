@echo off 
set PROJECT_PATH=%~dp0..\src\my.services.contactinfo.Data.Postgres.Migrations\my.services.contactinfo.Data.Postgres.Migrations.csproj

echo Generate an idempotent migration script for code first migrations in the PSQL Migrations project
set /p OUTPUT_NAME=Name of script?: 
for /f "delims=" %%# in ('powershell get-date -format "{yyyy-MM-dd-HH-mm-ss}"') do @set _date=%%#

set OUTPUT_PATH=%~dp0../src/MigrationScript/ExecutionScript/%_date%-%OUTPUT_NAME%.sql

echo:
echo Generating '%OUTPUT_PATH%' ...
echo:
dotnet build "%PROJECT_PATH%" -v minimal
dotnet ef migrations script -i -o "%OUTPUT_PATH%" --project "%PROJECT_PATH%" --no-build

echo:
echo Script created at '%OUTPUT_PATH%'

echo:
call start %OUTPUT_PATH% 