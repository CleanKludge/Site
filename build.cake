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

        foreach(var file in GetFiles("*.sln"))
        {
            DotNetCoreBuild(file.ToString(), settings);
        }
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetCoreTestSettings
        {
            Configuration = configuration
        };

        foreach(var file in GetFiles("./tests/*/*.csproj"))
        {
            DotNetCoreTest(file.ToString(), settings);
        }
    });

Task("Publish")
    .IsDependentOn("Test")
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