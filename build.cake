#addin "Cake.Docker"
#addin "Cake.Powershell"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Setup(context =>
{
    Information("Cleaning server artifacts...");
    CleanDirectories("./artifacts/server");
});

Teardown(context =>
{
    Information("Cleaning up old images...");
    StartPowershellScript("docker", args =>
        {
            args.Append("rmi");
            args.Append("$(docker images --quiet --filter dangling=true)");
            args.Append("> $null");
        });
});


Task("Clean")
    .Does(() =>
    {
        Func<IFileSystemInfo, bool> exclude_node_modules = fileSystemInfo => {
            return !fileSystemInfo.Path.FullPath.Contains("node_modules");
        };
        
        CleanDirectories("./src/**/bin", exclude_node_modules);
        CleanDirectories("./src/**/obj", exclude_node_modules);
    });

Task("Restore")
    .IsDependentOn("Clean")
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