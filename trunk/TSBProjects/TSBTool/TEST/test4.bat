:: Tests the TSBTool program
::
::
::@echo off

copy ..\bin\Debug\*.exe* .
copy ..\bin\Debug\TSPRBOWL.nes .
.\TSBToolSupreme TSPRBOWL.nes Test4.txt -out:output.nes
.\TSBToolSupreme -j -n -f -a -s -sch -pb -of output.nes > OUTPUT.txt
"C:\Program Files\ExamDiff\ExamDiff.exe" .\Test4.txt .\OUTPUT.txt
::FC /C /L Test1.txt OUTPUT.txt > RESULTS.txt
::del *.nes

::start notepad  RESULTS.txt


