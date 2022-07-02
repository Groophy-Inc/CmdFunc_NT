@echo off
cd..
cd cmdfunc
dotnet build --configuration Release 
cd..
cd tests
dotnet run --configuration Release