@ECHO OFF
REM ** Assumes we're starting in the Azuro.Crm directory

cd Azuro.Crm.Entities
del *.nupkg
nuget pack Azuro.Crm.Entities.csproj -Prop Configuration=Release -Prop Platform=AnyCPU -Symbols
rem for /F %%a in ('dir /b *.nupkg') do set FileName=%%~na
rem nuget push %FileName%.nupkg

cd ..\Azuro.Crm.Integration.Common
del *.nupkg
nuget pack Azuro.Crm.Integration.Common.csproj -Prop Configuration=Release -Prop Platform=AnyCPU -Symbols
rem for /F %%a in ('dir /b *.nupkg') do set FileName=%%~na
rem  nuget push %FileName%.nupkg

cd ..\Azuro.Crm.Integration.v2011
del *.nupkg
nuget pack Azuro.Crm.Integration.v2011.csproj -Prop Configuration=Release -Prop Platform=AnyCPU -Symbols
rem for /F %%a in ('dir /b *.nupkg') do set FileName=%%~na
rem  nuget push %FileName%.nupkg

cd ..\Azuro.Crm.SmsMessages
del *.nupkg
nuget pack Azuro.Crm.SmsMessages.csproj -Prop Configuration=Release -Prop Platform=AnyCPU -Symbols
rem for /F %%a in ('dir /b *.nupkg') do set FileName=%%~na
rem  nuget push %FileName%.nupkg

cd ..\Azuro.Crm.Integration.v2015
del *.nupkg
nuget pack Azuro.Crm.Integration.v2015.csproj -Prop Configuration=Release -Prop Platform=AnyCPU -Symbols
rem for /F %%a in ('dir /b *.nupkg') do set FileName=%%~na
rem nuget push %FileName%.nupkg

REM ** Return to the beginning
cd ..