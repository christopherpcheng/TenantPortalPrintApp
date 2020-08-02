@echo off
dotnet build -r osx-x64 -t:BundleApp -p:RuntimeIdentifier=osx-x64 -p:Configuration=Release-OSX .\TenantPortalPrintApp.sln 