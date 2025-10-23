using System;
using DataProm.Core;
using DataProm.Core.Data;
using DataProm.Core.FetchXml;
using Microsoft.VisualBasic;

namespace DataProm.ETLConsoleApp;

public class ETLPipeline
{
    public static void Run()
    {
        foreach (FetchXmlRepository.FetchEntry fetchEntry in FetchXmlRepository.FetchEntries.Values)
        {
            ConsolePrint.WriteLine($"Fetch ID : {fetchEntry.Fetch.Id}");
            List<DynamicRecordStruct> data = new List<DynamicRecordStruct>();

            // Extract
            ConsolePrint.WriteLine("Extracting data..", ConsolePrint.Category.Progress);
            data = new List<DynamicRecordStruct>(Extract(fetchEntry));
            // Transform
            ConsolePrint.WriteLine("Transforming data..", ConsolePrint.Category.Progress);
            Transform(data);
            // Load
            ConsolePrint.WriteLine("Loading data..", ConsolePrint.Category.Progress);
            Load(fetchEntry, data, true);
        }
    }
    /// <summary>
    /// Extract Data from database.
    /// </summary>
    /// <param name="fetchEntry"></param>
    /// <returns>Enumerable record.</returns>
    static IEnumerable<DynamicRecordStruct> Extract(FetchXmlRepository.FetchEntry fetchEntry)
    {
        using (IDatabase db = AppDatabase.DownloadDb)
        {
            db.EnsureOpen();
            using (IDataRecordReader reader = db.OpenDataReader())
            {
                foreach (DynamicRecordStruct rec in reader.GetRecordsByFetch(fetchEntry.Fetch.Id))
                {
                    yield return rec;
                }
            }
        }
    }

    /// <summary>
    /// Transform passed data.
    /// </summary>
    /// <param name="data"></param>
    /// <returns>Enumerable record.</returns>
    static IEnumerable<DynamicRecordStruct> Transform(IEnumerable<DynamicRecordStruct> data)
    {
        // implement transform logic
        // e.g. 
        // foreach (DynamicRecordStruct rec in data)
        // {
        //     if (rec.Get<DateOnly>("{name of property}") == DateOnly.FromDateTime(DateTime.Today))
        //     {
        //         rec.Set("{name of property}", 1);
        //     }
        //     yield return rec;
        // }
        return data;
    }

    /// <summary>
    /// Load Passed data to database and delete previous if set
    /// </summary>
    /// <param name="data"></param>
    /// <param name="deletePreviousData"></param>
    static void Load(FetchXmlRepository.FetchEntry fetchEntry, IEnumerable<DynamicRecordStruct> data, bool deletePreviousData)
    {
        using (var db = AppDatabase.UploadDb)
        {
            db.EnsureOpen();
            // delete previous data
            if (deletePreviousData)
                db.PrepareEmptyTable(fetchEntry.UploadMeta, out string cmd);

            Core.Sync.Uploader.Upload(db, data, fetchEntry.UploadMeta);
        }
    }
}
