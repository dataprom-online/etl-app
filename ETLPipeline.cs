using System;
using System.Data;
using DataProm.Core.Data;
using DataProm.Core.FetchXml;


namespace DataProm.ETL;

internal static class ETLPipeline
{
    /// <summary>
    /// Generate Dummy data using <see cref='LoadDummyData()' and <see cref='TestDataReader(MetaRecord, double)' /> />
    /// </summary>
    /// <param name="deletePreviousDb"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void GenerateAndUploadDummyData(bool deletePreviousDb = true)
    {
        if (deletePreviousDb)
        {
            if (Core.App.SyncConfig.UploadConnectionInfo is null)
            {
                throw new ArgumentNullException("Configuration.Instance.UploadConnectInfo is null.");
            }
            string path = Core.App.SyncConfig.UploadConnectionInfo.ConnectionString.Replace("Data Source=", string.Empty);

            if (Path.Exists(path))
            {
                File.Delete(path);
            }
        }
        Console.WriteLine("Download + Upload DUMMY data...");
        MetaRecord meta = AppData.GetTestMetaRecord("DummyData");
        using (var db = AppDatabase.UploadDb)
        {
            db.EnsureOpen();
            IEnumerable<DynamicRecordStruct> data = LoadDummyData();
            Core.Sync.Uploader.Upload(db, data, meta);
        }
    }

    /// <summary>
    /// Load data from local database.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static IEnumerable<DynamicRecordStruct> Extract()
    {
        Fetch fetch = FetchXmlRepository.GetFetch("DummyData");
        if (fetch is null)
        {
            throw new ArgumentNullException("Fetch with id 'TestTable' not defined in FetchXml project folder.");
        }
        
        Console.WriteLine("Download data from local db..");
        using (var db = AppDatabase.DownloadDb)
        {
            db.EnsureOpen();
            using (IDataRecordReader reader = db.OpenDataReader())
            {
                foreach (DynamicRecordStruct rec in reader.GetRecordsByFetch(fetch))
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
    public static IEnumerable<DynamicRecordStruct> Transform(IEnumerable<DynamicRecordStruct> data)
    {
        foreach (DynamicRecordStruct rec in data)
        {
            if (rec.Get<DateOnly>("prop1") == DateOnly.FromDateTime(DateTime.Today))
            {
                rec.Set("prop9", 1);
            }
            yield return rec;
        }
    }
    /// <summary>
    /// Load Passed data to database.
    /// </summary>
    /// <param name="data"></param>
    public static void Load(IEnumerable<DynamicRecordStruct> data)
    {
        MetaRecord meta = MetaData.GetMetaRecord("DummyData");
        using (var db = AppDatabase.UploadDb)
        {
            db.EnsureOpen();
            // delete previous data
            db.DropTable("DummyData");
            Core.Sync.Uploader.Upload(db, data, meta);
        }
    }
    /// <summary>
    /// Load Dymmy data using implemented DataReader.
    /// </summary>
    /// <returns></returns>
    static IEnumerable<DynamicRecordStruct> LoadDummyData()
    {
        MetaRecord meta = AppData.GetTestMetaRecord("DummyData");
        DataReader reader = new DataReader(meta, 1_00_000);

        FieldReaderFactory.FieldReader[] _readers = FieldReaderFactory.GetOrCreateFieldReaders(meta);

        var random = new Random();

        var buffer = new object[reader.FieldCount];
        while (reader.Read())
        {
            DynamicRecordStruct dr = new DynamicRecordStruct(ref meta);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (!reader.IsDBNull(i))
                    dr.Set(i, _readers[i](reader, i));
            }
            yield return dr;
        }

        reader.Dispose();
    }
}
