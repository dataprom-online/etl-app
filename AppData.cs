using System;
using DataProm.Core;
using DataProm.Core.Data;

namespace DataProm.ETL;

internal class AppData
{
    public static readonly string AppName = "DemoApp";
    static readonly string PROJECT_NAME = "SqliteETL";
    internal static void Initialize()
    {
        string appDataPath = Path.Combine(App.ApplicationRoot, AppName);
        string projectRootPath = Path.Combine(appDataPath, PROJECT_NAME);
        string projectConfigurationDirectory = Path.Combine(projectRootPath, "Configuration");

        if (!Directory.Exists(App.ApplicationRoot))
        {
            Directory.CreateDirectory(App.ApplicationRoot);
        }

        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }
        else
        {
            Directory.Delete(appDataPath, true);
        }

        Directory.CreateDirectory(projectRootPath);

        Directory.CreateDirectory(projectRootPath);
        Directory.CreateDirectory(projectConfigurationDirectory);

        // initialie app config
        string appConfigPath = Path.Combine(appDataPath, "app.config.json");
        if (!File.Exists(appConfigPath))
            WriteToFile(appConfigPath, APP_CONFIG_JSON);

        // initialie sync config
        string projSyncConfigPath = Path.Combine(projectConfigurationDirectory, "sync.config.xml");
        if (!File.Exists(projSyncConfigPath))
            WriteToFile(projSyncConfigPath, SYNC_CONFIG_SQLITE);

        // initialie config
        string projMetadataPath = Path.Combine(projectConfigurationDirectory, "metadata.xml");
        if (!File.Exists(projMetadataPath))
            WriteToFile(projMetadataPath, METADATA_XML);

        // initialie config
        string projFertchPath = Path.Combine(projectRootPath, "fetch.xml");
        if (!File.Exists(projFertchPath))
            WriteToFile(projFertchPath, FETCH_XML);
    }

    static void WriteToFile(string fileName, string content)
    {
        // Convert single quotes to double quotes for valid JSON/XML
        string fixedContent = content.Replace("'", "\"");
        // Optional: trim if leading spaces or newlines are undesired
        fixedContent = fixedContent.Trim();
        File.WriteAllText(fileName, fixedContent);
    }

    internal static MetaRecord GetTestMetaRecord(string name)
    {
        return new MetaRecord(name, properties:
        new MetaProperty[16]
        {
            new MetaProperty("prop0", MetaProperty.MetaType.Object, MetaProperty.MetaDisplayFormat.String),
            new MetaProperty("prop1", MetaProperty.MetaType.Object,MetaProperty.MetaDisplayFormat.DateOnly, dateFormat: "yyyyMMdd"),
            new MetaProperty("prop2", MetaProperty.MetaType.Object,MetaProperty.MetaDisplayFormat.TimeOnly),
            new MetaProperty("prop3", MetaProperty.MetaType.Object,MetaProperty.MetaDisplayFormat.DateTime),
            new MetaProperty("prop4", MetaProperty.MetaType.Object, MetaProperty.MetaDisplayFormat.Integer),
            new MetaProperty("prop5", MetaProperty.MetaType.Object, MetaProperty.MetaDisplayFormat.Double),
            new MetaProperty("prop6", MetaProperty.MetaType.Object, MetaProperty.MetaDisplayFormat.Decimal),
            new MetaProperty("prop7", MetaProperty.MetaType.Object, MetaProperty.MetaDisplayFormat.Float),
            new MetaProperty("prop8", MetaProperty.MetaType.Object, MetaProperty.MetaDisplayFormat.Long),
            new MetaProperty("prop9", MetaProperty.MetaType.Object, MetaProperty.MetaDisplayFormat.Boolean),
            new MetaProperty("prop10", MetaProperty.MetaType.PrimaryKey,MetaProperty.MetaDisplayFormat.Guid),
            new MetaProperty("prop11", MetaProperty.MetaType.Object, MetaProperty.MetaDisplayFormat.Byte),
            new MetaProperty("prop12", MetaProperty.MetaType.Object,MetaProperty.MetaDisplayFormat.StringDateOnly, dateFormat:"yyyyMMdd"),
            new MetaProperty("prop13", MetaProperty.MetaType.Object,MetaProperty.MetaDisplayFormat.StringDateTime, dateTimeFormat: "yyyyMMddHHmmss"),
            new MetaProperty("prop14", MetaProperty.MetaType.Object,MetaProperty.MetaDisplayFormat.StringTimeOnly, timeFormat:"HHmmss"),
            new MetaProperty("prop15", MetaProperty.MetaType.Normalized, MetaProperty.MetaDisplayFormat.String)
        }, null);
    }

    #region App Data (configs, fetch, metadata)
    static readonly string APP_CONFIG_JSON = @"{
    'AppName': 'DemoApp',
    'IsOnline': true,
    'IncrementalDownload': true,
    'DeleteAllData': false,
    'Culture': {
        'Language': 'en-SK',
        'DateTimeFormat': 'yyyy-MM-ddTHH:mm:ss',
        'DateFormat': 'yyyy-MM-dd',
        'TimeFormat': 'HH:mm:ss',
        'CurrencyDecimalDigits': 2
    },
    'Projects': [
        {
            'Name': 'SqliteETL',
            'Enabled': true
        }
    ]
}";

    static readonly string SYNC_CONFIG_SQLITE = @$"
<Configuration>
  <SyncConfig forceDeleteData='false' incrementalDownload='true'/>
  <Connections>
      <UploadConnection 
      type='sqlite' 
      connectionString='Data Source={Path.Combine(App.ApplicationRoot, AppName, PROJECT_NAME, "test.db")}'
      database='test.db'/>
      <DownloadConnection 
      type='sqlite' 
      connectionString='Data Source={Path.Combine(App.ApplicationRoot, AppName, PROJECT_NAME, "test.db")}'
      database='test.db'/>
  </Connections>
</Configuration>";

    static readonly string METADATA_XML = @"
    <Metadata version='v1'>
        <MetaRecord name='DummyData'>
            <Property name='prop0' displayFormat='0' />
            <Property name='prop1' displayFormat='1' dateFormat='yyyy-MM-dd'/>
            <Property name='prop2' displayFormat='2' />
            <Property name='prop3' displayFormat='3' />
            <Property name='prop4' displayFormat='4' />
            <Property name='prop5' displayFormat='5' />
            <Property name='prop6' displayFormat='6' />
            <Property name='prop7' displayFormat='7' />
            <Property name='prop8' displayFormat='8' />
            <Property name='prop9' displayFormat='9' />
            <Property name='prop10' displayFormat='10' nullable='false'/>
            <Property name='prop11' displayFormat='11' />
            <Property name='prop12' displayFormat='12' dateFormat='yyyy-MM-dd'/>
            <Property name='prop13' displayFormat='13' dateTimeFormat='yyyy-MM-ddTHH:mm:ss'/>
            <Property name='prop14' displayFormat='14' timeFormat='HH:mm:ss' />
            <Property name='prop15' displayFormat='4' />
        </MetaRecord>
    </Metadata>
    ";
    static readonly string FETCH_XML = @"
    <fetchXml>
    <fetch mapping='logical' id='DummyData'>
        <entity name='DummyData' alias='e'>
            <all-attributes />
          </entity>
    </fetch>
</fetchXml>";
    #endregion
}
