@echo off
fsc --platform:x86 --standalone --nologo --times -o:bin\Main.exe^
    src\Suave.fs^
    src\Main.fs