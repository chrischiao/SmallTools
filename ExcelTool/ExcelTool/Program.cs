using System;
using System.IO;

namespace ExcelTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0]))
                return;

            string file = args[0];
            if (!File.Exists(file))
                return;

      
            string extension = Path.GetExtension(file);
            if (!string.Equals(extension, ".xlsx"))
                return;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("excel file: {0}", file);

            string tableName = Path.GetFileNameWithoutExtension(file);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("converting ...");
            if (DataConverter.ExcelToSqlText(file, tableName))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("succeed!");

                string sqlFilePath = file.Replace("xlsx", "sql");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("sql file: {0}", sqlFilePath);

                var data = FileHelper.ReadLine(sqlFilePath);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine();
                foreach (string d in data)
                    Console.WriteLine(d);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("failed!");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("press any key to exit");
            Console.ReadKey();
        }
    }
}
