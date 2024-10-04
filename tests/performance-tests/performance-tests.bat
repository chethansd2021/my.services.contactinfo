
call mvn -B clean test-compile gatling:test -Dhttps.protocols=TLSv1,TLSv1.1,TLSv1.2

powershell -f combine-reports.ps1
