@echo off
fsc --platform:x86 --nologo -o:debug\Main.exe^
    src\Suave.fs^
    src\Main.fs