:: Tests the TSBTool program
::
::
::@echo off

copy ..\bin\Debug\*.exe* .
copy ..\bin\Debug\TSPRBOWL.nes .
.\TSBToolSupreme TSPRBOWL.nes Test3.txt -out:output.nes
.\TSBToolSupreme output.nes > OUTPUT.txt
"C:\Program Files\ExamDiff\ExamDiff.exe" .\Test3.txt .\OUTPUT.txt
::FC /C /L Test1.txt OUTPUT.txt > RESULTS.txt
::del *.nes

::start notepad  RESULTS.txt


