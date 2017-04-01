#addin "Cake.Docker"

var tag = Argument<string>("tag");

Setup(context =>
{
    Information(string.Format("Publishing tag '{0}'/", tag));
});

Task("DockerTag")
    .Does(() =>
    {
        DockerTag("cleankludge/website:unstable", string.Format("cleankludge/website:{0}", tag));
    });

Task("DockerPush")
    .Does(() =>
    {
        DockerPush(string.Format("cleankludge/website:{0}", tag));
    });

Task("Default")
    .IsDependentOn("DockerPush");

RunTarget("Default");