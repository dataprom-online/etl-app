using System;
using DataProm.Core;
using DataProm.Core.Data;

namespace DataProm.ETL;

#nullable disable warnings
/// <summary>
/// Encapsulates required database info
/// </summary>
internal static class AppDatabase
{
    private static readonly object _lock = new();
    /// <summary>Instance of database what is used to upload data.</summary>
    public static IDatabase UploadDb { get; private set; }
    /// <summary>Instance of database what is used to download data from server.</summary>
    public static IDatabase DownloadDb { get; private set; }

    public static void Initialize(SyncConfiguration config)
    {
        lock (_lock)
        {
            UploadDb = new DataProm.Sqlite.Core.SqliteDatabase(config.UploadConnectionInfo);
            DownloadDb = new DataProm.Sqlite.Core.SqliteDatabase(config.DownloadConnectionInfo);
        }
    }
}