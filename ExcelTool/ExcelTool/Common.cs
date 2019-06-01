namespace ExcelTool
{
    public class Common
    {
        private static string _exeFolder = System.Environment.CurrentDirectory;

        public static string DataFolder { get { return _exeFolder + @"\..\..\..\Data"; } }
    }
}
