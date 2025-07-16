@echo off

REM Check if a build type was specified as the first parameter
IF "%1"=="" goto setDefaultBuild
SET Build=%1
goto startDeploy

:setDefaultBuild
SET Build=Release

:startDeploy

echo deploy for %Build%

set destination=..\..\AssemblyCache\Azuro.EnterpriseLibrary\
rem cd ..\Azuro\Azuro.Crm

call :copyOutput Azuro.Crm.Integration.Common
call :copyOutput Azuro.Crm.Entities
call :copyOutput Azuro.Crm.Integration.v2011
call :copyOutput Azuro.Crm.SmsMessages
call :copyOutput Azuro.Crm.SmsMessageHandlers
call :copyOutput Azuro.Crm.SmsPlugin.v2011
call :copyOutput Azuro.Crm.Workflow
call :copyOutput Azuro.Crm.Workflow.SupportDesk
call :copyOutput Azuro.Crm.Integration.NAble.Plugin
call :copyOutput Azuro.Crm.Integration.NAble.Entities
call :copyOutput Azuro.Crm.Integration.NAble

echo done building for %Build%

IF not "%2"=="nopause" pause
goto exit

rem copyOutput solutionDir 
:copyOutput
for %%e in (dll,pdb) do call :copyFile %1 %%e
goto:eof

rem copyOutput solutionDir ext
:copyFile
echo copying %1\bin\%Build%\%1.%2
xcopy "%1\bin\%Build%\%1.%2" "%destination%" /F /Y /R /D			
goto:eof


:exit
