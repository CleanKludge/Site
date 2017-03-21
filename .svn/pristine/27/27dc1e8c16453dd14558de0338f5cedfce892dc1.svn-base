#addin "Cake.Docker"
#addin "Cake.Powershell"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("Restore")
    .Does(() =>
    {
        var settings = new DotNetCoreRestoreSettings
        {
            Verbose = false
        };

        DotNetCoreRestore(settings);
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var settings = new DotNetCoreBuildSettings
        {
            Configuration = configuration
        };

        foreach(var file in GetFiles("./src/*/*.csproj"))
        {
            DotNetCoreBuild(file.ToString(), settings);
        }
    });

Task("Publish")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetCorePublishSettings
        {
            Configuration = configuration,
            OutputDirectory = "./artifacts/server"
        };

        DotNetCorePublish("./src/CleanKludge.Server/CleanKludge.Server.csproj", settings);
    });

Task("DockerBuild")
    .IsDependentOn("Publish")
    .Does(() =>
    {
        var settings = new DockerBuildSettings
        {
            File = "./docker/server.dockerfile",
            ForceRm = true,
            Tag = new []  { "cleankludge/website:unstable" }
        };

        DockerBuild(settings, ".");
    });

Task("Default")
    .IsDependentOn("DockerBuild");

RunTarget(target);