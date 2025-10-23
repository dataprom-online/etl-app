using DataProm.Core;
using DataProm.ETLConsoleApp;

ConsolePrint.WriteLine("DataProm s.r.o. © 2025", ConsolePrint.Category.Title);

try
{
    // ✅ Access raw args
    string[] cmdArgs = Environment.GetCommandLineArgs().Skip(1).ToArray();

    // ✅ Parse named argument manually (simple & dependency-free)
    string? appName = null;
    for (int i = 0; i < args.Length; i++)
    {
        if (args[i].Equals("--app", StringComparison.OrdinalIgnoreCase))
        {
            if (i + 1 < args.Length)
                appName = args[i + 1].Trim();
            break;
        }
    }

    // ✅ Validate argument
    if (string.IsNullOrWhiteSpace(appName))
    {
        ConsolePrint.WriteLine("Error: Missing or invalid argument '--app <name>'", ConsolePrint.Category.Error);
        ConsolePrint.WriteLine("Error: Missing or invalid argument '--app <name>'", ConsolePrint.Category.Warning);
        ShowUsage();
        return;
    }

    DateTime start = DateTime.Now;

    // ✅ Initialize App data - Directory structure, Metadata, configs
    App.InitializeAppConfig(appName);
    ConsolePrint.WriteLine($"App Data {App.AppConfig.AppName} Initialized...");
    ConsolePrint.WriteLine($"App Configuration {App.AppConfig.AppName} Initialized...");
    ConsolePrint.WriteLine($"App Culture {App.AppConfig.AppName} Initialized...");

    foreach (ProjectInfo projectInfo in App.GetEnabledProjects())
    {
        App.LoadProjectContext(projectInfo);

        ConsolePrint.WriteLine($"Project {projectInfo.Name} Initialized...");
        ConsolePrint.WriteLine("Metadata Initialized...");
        ConsolePrint.WriteLine("FetchXml Repository Initialized...");

        AppDatabase.Initialize(App.SyncConfig);
        ConsolePrint.WriteLine("AppDatabase Initialized...");        

        // ✅ ETL Pipeline
        ConsolePrint.WriteLine("ETL Pipeline Running...");
        ETLPipeline.Run();
        ConsolePrint.WriteLine("ETL Pipeline Finished...");
    }

    DateTime end = DateTime.Now;
    ConsolePrint.WriteLine($"Elapsed {end.Subtract(start).TotalMilliseconds} ms", ConsolePrint.Category.Complete);

    StartupDiagnosticsManager.MarkRunCompleted(App.AppConfig);
}
catch (DataProm.Core.AppInitializationException)
{
    FileLogger.Initialize(App.ApplicationRoot);
    ConsolePrint.WriteLine("Failed to start ...", ConsolePrint.Category.Error);

}
catch (Exception ex)
{
    ConsolePrint.WriteLine(ex.Message, ConsolePrint.Category.Error);
    FileLogger.LogException(ex);
}


/// <summary>
/// Prints usage instructions
/// </summary>
static void ShowUsage()
{
    ConsolePrint.WriteLine("Usage: DataProm.ETLConsoleApp --app <application_name>", ConsolePrint.Category.Info);
    ConsolePrint.WriteLine("Example: DataProm.ETLConsoleApp --app DemoApp", ConsolePrint.Category.Info);
}