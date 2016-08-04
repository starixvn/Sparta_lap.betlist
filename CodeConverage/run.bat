@echo off

REM Set project name
set project=%1

if "%project%" == "" goto missing_project_name

REM create output folder
if not exist .\output mkdir .\output
if not exist ".\output\%project%" mkdir ".\output\%project%"

REM Set working dir
pushd ..\

REM NUnit
REM Dependencies\NUnit.Runners.2.6.4\tools\nunit-console.exe "UnitTests\%project%.UnitTests\bin\debug\%project%.UnitTests.dll" /noshadow /xml:"CodeConverage\output\%project%\TestResult.xml"

REM OpenCover
set command=Dependencies\OpenCover.4.5.3723\OpenCover.Console.exe -register:user -target:"Dependencies\NUnit.ConsoleRunner.3.4.1\tools\nunit3-console.exe" -targetargs:"UnitTests\%project%.Tests\bin\debug\%project%.Tests.dll --result=CodeConverage\output\%project%\TestResult.xml" -filter:+[%project%]* -output:"CodeConverage\output\%project%\result.xml"
call %command%
if %errorlevel% neq 0 goto error

REM REM ReportGenerator
set command=Dependencies\ReportGenerator.2.1.6.0\reportgenerator.exe -reports:"CodeConverage\output\%project%\result.xml" -targetdir:"CodeConverage\output\%project%\"
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