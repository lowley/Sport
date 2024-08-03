cd /d "C:\Users\olivier\source\repos\SportSolution\SportProject\"
dotnet clean
dotnet build SportProject.csproj -f net8.0-android -c Debug
rem cd bin\Debug\net8.0-android\
rem adb install sxb.sport.apk

