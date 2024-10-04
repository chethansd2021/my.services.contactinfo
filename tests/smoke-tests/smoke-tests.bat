@echo off 
@rem "-Dhttps.protocols=TLSv1,TLSv1.1,TLSv1.2" flag is resolve bug in JDK 11 regarding SSL
@rem "-Dkarate.options" including this flag will break the test runner

@REM SET WEB_APP_NAME=smoke-test-web-api 
@REM echo Start Web Project... 
@REM start "smoke-Test-Web-Api" dotnet run --project ../../src/my.services.contactinfo.Web/my.services.contactinfo.Web.csproj --verbosity minimal

echo Run Smoke Tests...
call mvn clean install test -Dtest=SmokeTestRunner -Dhttps.protocols=TLSv1,TLSv1.1,TLSv1.2

echo Open Test Files.... 
start "" %~dp0target/karate-reports/karate-summary.html
start "" %~dp0target/cucumber-html-reports/overview-features.html
