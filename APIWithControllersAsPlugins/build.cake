var target = Argument("target", "default");
var configuration = Argument("configuration", "Debug");

private void CleanProject(string projectDirectory){
    var projectFile = $"Plugins/{projectDirectory}/{projectDirectory}.csproj";
    var bin = $"Plugins/{projectDirectory}/bin";
    var obj = $"Plugins/{projectDirectory}/obj";

    var deleteSettings = new DeleteDirectorySettings{
        Force= true,
        Recursive = true
    };

    var cleanSettings = new DotNetCoreCleanSettings
    {
        Configuration = configuration
    };
    if (DirectoryExists(bin))
    {
      DeleteDirectory(bin, deleteSettings);
    }
    if (DirectoryExists(obj))
    {
      DeleteDirectory(obj, deleteSettings);
    }
    DotNetCoreClean(projectFile, cleanSettings);
}

Task("clean").Does( () =>
{ 
  CleanProject("DashboardControllerPlugin");
  CleanProject("OrdersControllerPlugin");
  CleanProject("ProductsControllerPlugin");
});

Task("build")
  .IsDependentOn("clean")
  .Does( () =>
{ 
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = configuration
    };

    DotNetCoreBuild("Plugins/DashboardControllerPlugin/DashboardControllerPlugin.csproj", settings);
    DotNetCoreBuild("Plugins/OrdersControllerPlugin/OrdersControllerPlugin.csproj", settings);
    DotNetCoreBuild("Plugins/ProductsControllerPlugin/ProductsControllerPlugin.csproj", settings);
});

Task("publish")
  .IsDependentOn("build")
  .Does(() =>
  { 
    DotNetCorePublish("Plugins/DashboardControllerPlugin/DashboardControllerPlugin.csproj", new DotNetCorePublishSettings
    {
        NoBuild = true,
        Configuration = configuration,
        OutputDirectory = "publish/DashboardControllerPlugin"
    });

    DotNetCorePublish("Plugins/OrdersControllerPlugin/OrdersControllerPlugin.csproj", new DotNetCorePublishSettings
    {
        NoBuild = true,
        Configuration = configuration,
        OutputDirectory = "publish/OrdersControllerPlugin"
    });

    DotNetCorePublish("Plugins/ProductsControllerPlugin/ProductsControllerPlugin.csproj", new DotNetCorePublishSettings
    {
        NoBuild = true,
        Configuration = configuration,
        OutputDirectory = "publish/ProductsControllerPlugin"
    });
  });

Task("copy-to-apphost")
  .IsDependentOn("publish")
  .Does(() =>
  {
    CopyDirectory("publish/DashboardControllerPlugin", "MyHost/bin/debug/netcoreapp3.1/Plugins/DashboardControllerPlugin");
    CopyDirectory("publish/DashboardControllerPlugin", "MyHost2/bin/debug/netcoreapp2.1/Plugins/DashboardControllerPlugin");
    CopyDirectory("publish/OrdersControllerPlugin", "MyHost/bin/debug/netcoreapp3.1/Plugins/OrdersControllerPlugin");
    CopyDirectory("publish/ProductsControllerPlugin", "MyHost/bin/debug/netcoreapp3.1/Plugins/ProductsControllerPlugin");
    CopyDirectory("publish/ProductsControllerPlugin", "MyHost2/bin/debug/netcoreapp2.1/Plugins/ProductsControllerPlugin");
  });

Task("default")
  .IsDependentOn("copy-to-apphost");

RunTarget(target);