using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Linq;

namespace ExcelTool
{
    public class DataConverter
    {
        public static bool ExcelToSqlText(string excelPath, string tableName)
        {
            if (!File.Exists(excelPath))
                return false;

            var dataTable = ExcelReader.ExcelToTable(excelPath);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                var sqls = GetInsertSqlFromDataTable(dataTable, tableName);
                if (sqls.Any())
                {
                    string txtPath = excelPath.Replace("xlsx", "sql");
                    if (File.Exists(txtPath))
                        File.Delete(txtPath);

                    FileHelper.Write(sqls, txtPath);
                    return true;
                }
            }

            return false;
        }

        public static bool ExcelToCsharpText(string excelPath, string tableName)
        {
            if (!File.Exists(excelPath))
                return false;

            var dataTable = ExcelReader.ExcelToTable(excelPath);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                var text = GetInsertCsharpCode(dataTable, tableName);
                if (!string.IsNullOrEmpty(text))
                {
                    string txtPath = excelPath.Replace("xlsx", "cs");
                    if (File.Exists(txtPath))
                        File.Delete(txtPath);

                    FileHelper.Write(text, txtPath);
                    return true;
                }
            }

            return false;
        }

        public static IList<string> GetInsertSqlFromDataTable(DataTable dataTable, string tableName)
        {
            var sqls = new List<string>();

            if (dataTable != null && dataTable.Columns.Count > 0 && dataTable.Rows.Count > 0)
            {
                string sqlFormat = "insert into " + tableName + " values({0});";
                int columnCount = dataTable.Columns.Count;
                foreach (DataRow row in dataTable.Rows)
                {
                    StringBuilder valuesBuilder = new StringBuilder();
                    for (int i = 0; i < columnCount; i++)
                        valuesBuilder.Append("'" + row[i].ToString() + "',");

                    string values = valuesBuilder.ToString().TrimEnd(',');
                    sqls.Add(string.Format(sqlFormat, values));
                }
            }

            return sqls;
        }

        public static string GetInsertCsharpCode(DataTable dataTable, string tableName)
        {
            var csharpBuilder = new StringBuilder();

            if (dataTable != null && dataTable.Columns.Count > 0 && dataTable.Rows.Count > 0)
            {
                csharpBuilder.Append("var sqlList = new List<string> \r\n{\r\n");

                string sqlFormat = "insert into " + tableName + " values({0});";
                int columnCount = dataTable.Columns.Count;
                foreach (DataRow row in dataTable.Rows)
                {
                    StringBuilder valuesBuilder = new StringBuilder();
                    for (int i = 0; i < columnCount; i++)
                        valuesBuilder.Append("'" + row[i].ToString() + "',");

                    string values = valuesBuilder.ToString().TrimEnd(',');
                    string sql = string.Format(sqlFormat, values);

                    csharpBuilder.Append("\"");
                    csharpBuilder.Append(sql);
                    csharpBuilder.Append("\"");
                    csharpBuilder.Append(",\r\n");
                }

                csharpBuilder.Append("};\r\n");
                csharpBuilder.Append("foreach (var sql in sqlList)\r\nlinker.ExecuteSQL(sql);\r\n");
            }

            return csharpBuilder.ToString();
        }
    }
}
