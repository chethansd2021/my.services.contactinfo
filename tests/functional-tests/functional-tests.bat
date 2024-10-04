@rem "-Dhttps.protocols=TLSv1,TLSv1.1,TLSv1.2" flag is to resolve bug in JDK 11 regarding SSL
call mvn clean install test -Dtest=FunctionalTestRunner -Dhttps.protocols=TLSv1,TLSv1.1,TLSv1.2

echo Open Test Files.... 
start "" %~dp0target/karate-reports/karate-summary.html
start "" %~dp0target/cucumber-html-reports/overview-features.html