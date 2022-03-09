cd .\Server\bin\Debug
start cmd.exe /c Avvia.bat
start cmd.exe /c AvviaCom.bat
start cmd.exe /c AvviaVer.bat
start cmd.exe /c AvviaComp.bat
SLEEP 400
cd ../../../Dns/bin/Debug
start cmd.exe /c  Dns.exe 7000
