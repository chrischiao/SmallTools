using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileOperator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
                return;

            string path = args[0];
            if (File.Exists(path))
                MergeSplitOperator.SplitFile(path);
            else if (Directory.Exists(path))
                MergeSplitOperator.MergeFiles(path);
        }
    }
}
