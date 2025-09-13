// See https://aka.ms/new-console-template for more information

using DataProm.Core;
using DataProm.Core.Data;
using DataProm.ETL;

Console.WriteLine("DataProm s.r.o. © 2025");

// Main point
try
{
	DateTime start = DateTime.Now;
	// Initialize App data - Directory structure, Metadata, configs.
	AppData.Initialize();
	Console.WriteLine("App Data " + AppData.AppName + "Initialized...");
	//DataProm App layer initialization
	App.InitializeAppConfig(AppData.AppName);
	Console.WriteLine("Configuration " + AppData.AppName + "Initialized...");

	foreach (ProjectInfo projectInfo in App.GetEnabledProjects())
	{
		App.LoadProjectContext(projectInfo);
		// Custom implementation of AppDatabase
		AppDatabase.Initialize(App.SyncConfig);
		Console.WriteLine("AppDatabase Initialized...");
		ETLPipeline.GenerateAndUploadDummyData();
		Console.WriteLine("Dummy data generated and loaded");
		// Extract
		IEnumerable<DynamicRecordStruct> data = ETLPipeline.Extract();
		// Transform
		IEnumerable<DynamicRecordStruct> transformed = ETLPipeline.Transform(data);
		List<DynamicRecordStruct> transformedList = new List<DynamicRecordStruct>(transformed);
		// Load
		ETLPipeline.Load(transformedList);
	}
	DateTime end = DateTime.Now;
	Console.WriteLine($"Ellapsed {end.Subtract(start).TotalMilliseconds}");
	DataProm.Core.StartupDiagnosticsManager.MarkRunCompleted(App.AppConfig);
}
catch (Exception ex)
{
	if (ex is DataProm.Core.AppInitializationException)
	{
		FileLogger.Initialize(App.ApplicationRoot);
		Console.WriteLine($"Failed to start ...\n\n");
	}
	else
	{
		Console.WriteLine($"Error:\n\n");
	}
	FileLogger.LogException(ex);
}