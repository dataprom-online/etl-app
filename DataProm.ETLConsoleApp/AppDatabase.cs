using System;
using DataProm.Core;
using DataProm.Core.Data;

namespace DataProm.ETLConsoleApp;

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
            UploadDb = GetUploadDatabaseByConfig(config.UploadConnectionInfo);
            DownloadDb = GetDownloadDatabaseByConfig(config.DownloadConnectionInfo);
        }
    }

    private static IDatabase GetDownloadDatabaseByConfig(IConnectionInfo connectionInfo)
    {
        return connectionInfo.Type switch
        {
            "oracle" => new DataProm.OracleDb.Core.OracleDatabase(connectionInfo),
            "sqlite" => new DataProm.Sqlite.Core.SqliteDatabase(connectionInfo),
            "sql" => new DataProm.Sql.Core.SqlDatabase(connectionInfo),
            _ => throw new InvalidDataException($"Type of database is not supported for download {connectionInfo.Type}")
        };
    }

    private static IDatabase GetUploadDatabaseByConfig(IConnectionInfo connectionInfo)
    {
        return connectionInfo.Type switch
        {
            "sqlite" => new DataProm.Sqlite.Core.SqliteDatabase(connectionInfo),
            "sql" => new DataProm.Sql.Core.SqlDatabase(connectionInfo),
            _ => throw new InvalidDataException($"Type of database is not supported for upload {connectionInfo.Type}")
        };
    }
}