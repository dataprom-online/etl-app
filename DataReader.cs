using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using DataProm.Core.Data;

namespace DataProm.ETL;

#nullable disable
internal class DataReader : IDataReader
{
    MetaProperty[] _properties { get; }
    MetaRecord MetaRecord { get; }
    private string _normalizedStr;
    int _hitWhenAddNewNormalizedStr;
    int _cnt;
    public DataReader(MetaRecord meta, double cnt = 1_000_000)
    {
        MetaRecord = meta;
        _properties = meta.Properties;
        _cnt = (int)cnt;
        data = new object[_properties.Length];
        _hitWhenAddNewNormalizedStr = (int)(cnt * 0.1);
        _normalizedStr = string.Empty;
    }

    #region Insert new data to data array.
    private void InsertNewData()
    {
        data = new object[_properties.Length];

        for (int i = 0; i < _properties.Length; i++)
        {
            MetaProperty mp = _properties[i];

            if (mp.IsNormalized)
            {
                if (index % _hitWhenAddNewNormalizedStr == 0)
                {
                    _normalizedStr = RandomString(20);
                }
                data[i] = _normalizedStr;
                continue;
            }

            switch (mp.DisplayFormat)
            {
                // on the purpose we set null on byte format to validate materialiser isDbNullExpr
                case MetaProperty.MetaDisplayFormat.Byte:
                    byte value1 = 64;
                    data[i] = value1;
                    break;
                case MetaProperty.MetaDisplayFormat.String:
                    data[i] = RandomString(5);
                    break;
                case MetaProperty.MetaDisplayFormat.DateTime:
                    data[i] = DateTime.Now;
                    break;
                case MetaProperty.MetaDisplayFormat.DateOnly:
                    data[i] = DateOnly.FromDateTime(DateTime.Today).ToString(mp.DateFormat);
                    break;
                case MetaProperty.MetaDisplayFormat.StringDateTime:
                    data[i] = DateTime.Now.ToString(mp.DateTimeFormat);
                    break;
                case MetaProperty.MetaDisplayFormat.StringDateOnly:
                    data[i] = "19900613";
                    break;
                case MetaProperty.MetaDisplayFormat.StringTimeOnly:
                    data[i] = "232359";
                    break;
                case MetaProperty.MetaDisplayFormat.TimeOnly:
                    data[i] = TimeOnly.MinValue;
                    break;
                case MetaProperty.MetaDisplayFormat.Double:
                    data[i] = double.Pi;
                    break;
                case MetaProperty.MetaDisplayFormat.Float:
                    data[i] = float.Epsilon;
                    break;
                case MetaProperty.MetaDisplayFormat.Integer:
                    data[i] = Int32.MaxValue;
                    break;
                case MetaProperty.MetaDisplayFormat.Long:
                    data[i] = Int64.MinValue;
                    break;
                case MetaProperty.MetaDisplayFormat.Decimal:
                    data[i] = decimal.One;
                    break;
                case MetaProperty.MetaDisplayFormat.Guid:
                    data[i] = Guid.NewGuid();
                    break;
                case MetaProperty.MetaDisplayFormat.Boolean:
                    data[i] = false;
                    break;
                default:
                    data[i] = null;
                    break;
            }
        }
    }
    #endregion
    int index = 0;
    object[] data;
    public object this[int i] => data[i];

    public object this[string name] => data[MetaRecord.GetIndex(name)];

    public int Depth => index;

    public bool IsClosed => throw new NotImplementedException();

    public int RecordsAffected => index;

    public int FieldCount => _properties.Length;

    public void Close()
    {
        //throw new NotImplementedException();
    }

    public void Dispose()
    {
        data = null;
    }

    public bool GetBoolean(int i) => (bool)data[i];

    public byte GetByte(int i) => (byte)data[i];

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
        throw new NotImplementedException();
    }

    public char GetChar(int i)
    {
        throw new NotImplementedException();
    }

    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
        throw new NotImplementedException();
    }

    public IDataReader GetData(int i)
    {
        throw new NotImplementedException();
    }

    public string GetDataTypeName(int i) => _properties[i].Type.ToString();

    public DateTime GetDateTime(int i) => (DateTime)data[i];
    public decimal GetDecimal(int i) => (decimal)data[i];
    public double GetDouble(int i) => (double)data[i];
    public Type GetFieldType(int i) => _properties[i].GetType();
    public float GetFloat(int i) => (float)data[i];
    public Guid GetGuid(int i) => (Guid)data[i];
    public short GetInt16(int i) => (short)data[i];
    public int GetInt32(int i) => (int)data[i];
    public long GetInt64(int i) => (long)data[i];
    public string GetName(int i) => _properties[i].Name;
    public int GetOrdinal(string name) => MetaRecord.GetIndex(name);
    public string GetString(int i) => (string)data[i];

    public object GetValue(int i) => data[i];

    public int GetValues(object[] values)
    {
        throw new NotImplementedException();
    }
    public DataTable GetSchemaTable()
    {
        throw new NotImplementedException();
    }

    public bool IsDBNull(int i)
    {
        return data[i] == DBNull.Value;
    }

    public bool NextResult()
    {
        if (index + 1 < _cnt)
        {
            InsertNewData();
            index++;
            return true;
        }
        return false;
    }

    public bool Read() => NextResult();

    #region helpers
    static Random _random = new Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var buffer = new char[length];
        for (int i = 0; i < length; i++)
            buffer[i] = chars[_random.Next(chars.Length)];
        return new string(buffer);
    }
    #endregion
}
#nullable restore
