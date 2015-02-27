using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Configuration;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void shared_GetMatrixValues(string name, string conditionals, string returns)
    {
        var output = new SharedRefXml(GetSettings()["SharedRef"]).GetMatrixValues(name, conditionals, returns);

        SendToPipe(output);
    }

    private const string _sql = "SELECT name, value FROM ::fn_listextendedproperty(NULL, NULL, NULL, NULL, NULL, NULL, NULL)";

    private static Dictionary<string, string> GetSettings()
    {
        using (var connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            using (SqlCommand selectProps = new SqlCommand(_sql, connection))
            {
                var settings = new Dictionary<string, string>();

                using (SqlDataReader reader = selectProps.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        settings.Add(reader.GetSqlString(0).Value, reader.GetSqlString(1).Value);
                    }
                }

                return settings;
            }
        }
    }


    private static void SendToPipe(string output)
    {
        var doc = XDocument.Parse(output);

        SqlDataRecord record = null;

        List<string> names = null;

        List<string> values = null;

        foreach (var item in doc.Root.Elements("items").Elements())
        {
            if (names == null || record == null)
            {
                names = new List<string>(item.Attributes().Count());
                names.AddRange(item.Attributes().Select(a => a.Name.LocalName));

                record = CreateDataRecord(names);
                SqlContext.Pipe.SendResultsStart(record);
            }

            values = new List<string>(item.Attributes().Select(a => a.Value));

            FillDataRecord(values, ref record);

            SqlContext.Pipe.SendResultsRow(record);
        }

        if (record != null)
            SqlContext.Pipe.SendResultsEnd();
    }

    private static SqlDataRecord CreateDataRecord(List<string> meta)
    {
        var metaData = new List<SqlMetaData>(meta.Count);

        metaData.AddRange(meta.ToList().Select(i => new SqlMetaData(i, SqlDbType.NVarChar, 250)));

        var record = new SqlDataRecord(metaData.ToArray());

        return record;
    }

    private static void FillDataRecord(List<string> values, ref SqlDataRecord record)
    {
        int count = 0;
        foreach (var val in values)
        {
            record.SetString(count++, val);
        }
    }
};
