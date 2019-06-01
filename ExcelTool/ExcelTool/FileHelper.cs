using System.Collections.Generic;
using System.IO;

namespace ExcelTool
{
    public class FileHelper
    {
        public static void Write(IList<string> lineData, string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            foreach (string line in lineData)
                sw.WriteLine(line);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        public static void Write(string data, string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(data);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        public static IList<string> ReadLine(string filePath)
        {
            var data = new List<string>();

            using (var sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    data.Add(line);
                }
                sr.Close();
            }

            return data;
        }
    }
}
