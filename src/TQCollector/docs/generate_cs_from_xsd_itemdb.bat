@echo off
echo.
echo Generating "itemdb.cs" from itemdb.xsd"
echo.
cd ..
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\xsd.exe" itemdb.xsd /c /namespace:TQCollector
echo.
echo Generation completed! Check output file manually!
echo.
PAUSE