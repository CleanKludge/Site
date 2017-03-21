#addin "Cake.Powershell"
#addin "Cake.Path"
#addin "Cake.Compression"

var tempPath = EnvironmentVariable("Temp");
var target = Argument("target", "Default");

Task("dotnetcore")
    .Description("Install dotnetcore 1.1.0")
    .WithCriteria(() => !DirectoryExists("C:\\Program Files\\dotnet\\host\\fxr\\1.1.0"))
    .Does(() =>
    {
        ChocolateyInstall("dotnetcore", new ChocolateyInstallSettings {
            Version = "1.1.0",
            LimitOutput = true
        });
    });

Task("dotnetcore sdk")
    .Description("Install dotnetcore sdk 1.0.0-rc4-004771")
    .IsDependentOn("dotnetcore")
    .WithCriteria(() => !DirectoryExists("C:\\Program Files\\dotnet\\sdk\\1.0.0-rc4-004771"))
    .Does(() =>
    {
        var resource = DownloadFile("https://go.microsoft.com/fwlink/?linkid=841686");
        var exitCode = StartProcess(resource.FullPath, new ProcessSettings { 
            RedirectStandardOutput = true,
            Arguments = new ProcessArgumentBuilder()
                .Append("SKIP_VSU_CHECK=1")
                .Append("/install")
                .Append("-q")
        });
        
        if(exitCode != 0)
            throw new Exception(string.Format("[{0}] Failed to install dotnetcore sdk 1.0.0-rc4-004771", exitCode));   

        DeleteFile(resource);
    });

Task("nodejs")
    .Description("Install nodejs")
    .IsDependentOn("dotnetcore sdk")
    .WithCriteria(() => !DirectoryExists("C:\\Program Files\\nodejs"))
    .Does(() =>
    {
        ChocolateyInstall("nodejs.install", new ChocolateyInstallSettings {
            Version = "7.5.0",
            LimitOutput = true,
            Force = false,
            NotSilent = true
        });

        AddToPath("C:\\Program Files\\nodejs\\");
        ReloadPath();
    });

Task("npm-update")
    .Description("Update npm")
    .IsDependentOn("nodejs")
    .Does(() =>
    {
        var exitCode = StartProcess("C:\\Program Files\\nodejs\\npm.cmd", new ProcessSettings { 
            RedirectStandardOutput = true,
            Arguments = new ProcessArgumentBuilder()
                .Append("update")
        });
        
        if(exitCode != 0)
            throw new Exception(string.Format("[{0}] Failed to update npm", exitCode));   
    });

Task("bower")
    .Description("Install bower")
    .IsDependentOn("npm-update")
    .Does(() =>
    {
        ReloadPath();
        var exitCode = StartProcess("C:\\Program Files\\nodejs\\npm.cmd", new ProcessSettings { 
            RedirectStandardOutput = true,
            Arguments = new ProcessArgumentBuilder()
                .Append("install")
                .Append("-g")
                .Append("bower")
        });
        
        if(exitCode != 0)
            throw new Exception(string.Format("[{0}] Failed to install bower", exitCode));   
    });

Task("Default")
    .IsDependentOn("bower");

RunTarget(target);