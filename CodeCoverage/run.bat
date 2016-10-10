@echo off

setlocal ENABLEDELAYEDEXPANSION
REM Set project name

set project=%1

if "%project%" == "" goto missing_project_name

set fileList=
for %%y in (%*) do (

set v1=%%y
call set v2=x%%v1:UnitTests=%%%%

if not !v2!==x%%y (set fileList=!fileList! UnitTests\%%y\bin\Debug\%%y.dll)

)

echo %fileList%

REM create output folder
if not exist ..\cireport\testcoveragereport mkdir ..\cireport\testcoveragereport

REM Set working dir
pushd ..\

REM NUnit
REM OpenCover
set command=packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -target:"packages\NUnit.ConsoleRunner.3.4.1\tools\nunit3-console.exe" -targetargs:"%fileList% --result=TestResult_%project%.xml;format=nunit2" -output:"TestCoverage_%project%.xml"
call %command%
if %errorlevel% neq 0 goto error

REM REM ReportGenerator
set command=packages\ReportGenerator.2.4.5.0\tool\reportgenerator.exe -reports:"TestCoverage_%project%.xml" -targetdir:"cireport\testcoveragereport\"
call %command%
if %errorlevel% neq 0 goto error

echo Success.
goto end

:missing_project_name
echo Please provide name of your test project.
goto end

:error
echo Failure.

:end
popd
