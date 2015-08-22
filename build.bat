@echo Off

echo BUILD.BAT - NuGet package restore started.
".\src\.nuget\NuGet.exe" restore ".\src\Caliburn.Micro.Reactive.Extensions.sln" -OutputDirectory ".\src\packages"
echo BUILD.BAT - NuGet package restore finished.

echo BUILD.BAT - FAKE build started.
".\src\packages\FAKE.4.1.4\tools\Fake.exe" build.fsx
echo BUILD.BAT - FAKE build finished.