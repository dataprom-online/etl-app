// See https://aka.ms/new-console-template for more information

Console.WriteLine("DataProm s.r.o. © 2025");
/*
// Run any tests from QA instances
static void InitializeQA()
{
	DateTime start = DateTime.Now;

	DataProm.Test.QA.TestProject.Initialize();
	DataProm.Test.QA.TestProject.GenerateAndUploadDummyData();
	DateTime end = DateTime.Now;
	Console.WriteLine($"Ellapsed {end.Subtract(start).TotalMilliseconds}");
	DataProm.Test.QA.TestProject.DownloadFromDatabase();
	end = DateTime.Now;
	Console.WriteLine($"Ellapsed {end.Subtract(start).TotalMilliseconds}");

	Console.WriteLine($"QA.Tests End. ellapsed {end.Subtract(start).TotalMilliseconds}");

	DataProm.Core.GlobalApp.StartupDiagnosticsManager.MarkRunCompleted(DataProm.Core.App.AppConfig);
}

InitializeQA();
*/