// See https://aka.ms/new-console-template for more information

using DataProm.Core;
using DataProm.Core.Data;
using DataProm.ETL;

ConsolePrint.PrintColoredTitle("DataProm s.r.o. © 2025");

// Main point
try
{
	DateTime start = DateTime.Now;
	// Initialize App data - Directory structure, Metadata, configs.
	AppData.Initialize();
	ConsolePrint.WriteLine("App Data " + AppData.AppName + " Initialized...");
	//DataProm App layer initialization
	App.InitializeAppConfig(AppData.AppName);
	ConsolePrint.WriteLine("Configuration " + AppData.AppName + " Initialized...");

	foreach (ProjectInfo projectInfo in App.GetEnabledProjects())
	{
		App.LoadProjectContext(projectInfo);
		// Custom implementation of AppDatabase
		AppDatabase.Initialize(App.SyncConfig);
		ConsolePrint.WriteLine("AppDatabase Initialized...");
		ETLPipeline.GenerateAndUploadDummyData();
		ConsolePrint.WriteLine("Dummy data generated and loaded...", ConsolePrint.Category.Progress);
		// Extract
		IEnumerable<DynamicRecordStruct> data = ETLPipeline.Extract();
		// Transform
		IEnumerable<DynamicRecordStruct> transformed = ETLPipeline.Transform(data);
		List<DynamicRecordStruct> transformedList = new List<DynamicRecordStruct>(transformed);
		// Load
		ETLPipeline.Load(transformedList);
	}
	DateTime end = DateTime.Now;
	ConsolePrint.WriteLine($"Ellapsed {end.Subtract(start).TotalMilliseconds}",ConsolePrint.Category.Complete);
	DataProm.Core.StartupDiagnosticsManager.MarkRunCompleted(App.AppConfig);
}
catch (Exception ex)
{
	if (ex is DataProm.Core.AppInitializationException)
	{
        FileLogger.Initialize(App.ApplicationRoot);
        ConsolePrint.WriteLine("Failed to start ...", ConsolePrint.Category.Error);
	}
	else
	{
		ConsolePrint.WriteLine(ex.Message, ConsolePrint.Category.Error);
	}
	FileLogger.LogException(ex);
}