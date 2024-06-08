@echo off
echo.
echo Generating "config.cs" from config.xsd"
echo.
cd ..
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\xsd.exe" config.xsd /c
echo.
echo Generation completed! Check output file manually!
echo.
PAUSE