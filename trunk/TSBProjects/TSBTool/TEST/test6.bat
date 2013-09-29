:: Tests the TSBTool program
::
::
::@echo off

copy ..\bin\Debug\TSBToolSupreme.exe .
copy "..\bin\Debug\TSB 2007-32-111.nes" .\tsb32team.nes 
.\TSBToolSupreme tsb32team.nes Test6.txt -out:output.nes
.\TSBToolSupreme -j -n -f -a -s -sch -pb -of output.nes > OUTPUT.txt
"C:\Program Files\ExamDiff Pro\ExamDiff.exe" .\Test6.txt .\OUTPUT.txt
::FC /C /L Test1.txt OUTPUT.txt > RESULTS.txt
::del *.nes

::start notepad  RESULTS.txt


