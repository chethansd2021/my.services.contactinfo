# Tests
Tests are used to verify functionality in an automated fashion.

Development Team is responsible for all tests. QA Team is accountable, reviewing the tests and modifying them to ensure coverage and accuracy. 

## Getting Started
1. Install Java JDK - https://jdk.java.net/21/
2. Install Maven CLI - https://maven.apache.org/download.cgi
3. (Optional) Add `mvn` to your path
    - Running `mvn --version` should return the mvn version if everything is configured correctly. 
4. (Optional) Install VS Code Plugin for `.feature` syntax highlighting - https://marketplace.visualstudio.com/items?itemName=karatelabs.karate
5. (Optional) Install VS Code Plugin for `.scala` syntax highlighting - https://marketplace.visualstudio.com/items?itemName=scala-lang.scala
6. (Optional) Update tests `config.json` urls to point to environment you want to test 
  - Functional Tests: `tests/functional-tests/src/test/java/config.json`
  - Performance Tests: `tests/performance-tests/src/test/java/config.json`
  - Smoke Tests: `tests/smoke-tests/src/test/java/config.json`
7. (If using localhost) Start web application 
8. Run test .bat file
  - Functional Tests: `tests/functional-tests/functional-tests.bat`
  - Performance Tests: `tests/performance-tests/performance-tests.bat`
  - Smoke Tests: `tests/smoke-tests/smoke-tests.bat`
9. Review results

## functional-tests
Tests that verify that API Call and Response headers, payload and other behaviors are as expected.     
Automated to run after deployment to environment. 

## performance-tests
Load tests against sets of APIs verifying that the API is meeting expectations.  
Automated to run after deployment to environment. 

## smoke-tests
Simple API calls run on a scheduled basis to verify API is alive.  
Automated to run after deployment to environment. 

## unit-tests
Code level unit tests covering isolated dedicate units of work within the codebase.  
Gated. Automated to run before pull request merge.  