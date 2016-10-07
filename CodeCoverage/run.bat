@echo off

REM Set project name
setlocal ENABLEDELAYEDEXPANSION
set project=%1

if "%project%" == "" goto missing_project_name

for %%x in (%*) do (
 set fileList=!fileList! UnitTests\%%x\bin\Debug\%%x.dll

)

 echo %fileList%

REM create output folder
if not exist ..\cireport\testcoveragereport mkdir ..\cireport\testcoveragereport

REM Set working dir
pushd ..\

REM NUnit
REM OpenCover
set command=Dependencies\OpenCover.4.5.3723\OpenCover.Console.exe -register:user -target:"Dependencies\NUnit.ConsoleRunner.3.4.1\tools\nunit3-console.exe" -targetargs:"%fileList% --result=UnitTest.xml ;format=nunit2" -output:"UnitTestCoverage.xml"
call %command%
if %errorlevel% neq 0 goto error

REM REM ReportGenerator
set command=Dependencies\ReportGenerator.2.1.6.0\reportgenerator.exe -reports:"UnitTestCoverage.xml" -targetdir:"cireport\testcoveragereport\"
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
