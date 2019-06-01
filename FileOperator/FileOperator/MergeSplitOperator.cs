using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileOperator
{
    public static class MergeSplitOperator
    {
        /// <summary>
        /// 将目录下的所有文件合并为一个文件
        /// </summary>
        public static void MergeFiles(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    return;

                var files = Directory.GetFiles(path);
                if (files.Length < 1)
                    return;

                IList<MemoryStream> memoryStreams = new List<MemoryStream>();
                memoryStreams.Add(GetFileInfoStream(files));
                foreach (var file in files)
                    memoryStreams.Add(new MemoryStream(File.ReadAllBytes(file)));

                string targetPath = Path.Combine(path, "0.mf");
                using (FileStream fileStream = new FileStream(targetPath, FileMode.Create))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, memoryStreams);
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 拆分文件
        /// </summary>
        public static void SplitFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return;
                string directory = Path.GetDirectoryName(path);

                FileStream fileStream = new FileStream(path, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                IList<MemoryStream> memoryStreams = binaryFormatter.Deserialize(fileStream) as IList<MemoryStream>;
                if (memoryStreams != null && memoryStreams.Count > 1)
                {
                    string[] fileNames = GetFileNames(memoryStreams[0]);
                    if (memoryStreams.Count - 1 != fileNames.Length)
                        return;

                    for (int i = 1; i < memoryStreams.Count; i++)
                    {
                        MemoryStreamToFile(memoryStreams[i], Path.Combine(directory, fileNames[i - 1]));
                    }
                }
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 文件信息作为流输出
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        private static MemoryStream GetFileInfoStream(string[] files)
        {
            string[] fileNames = files.Select(f => Path.GetFileName(f)).ToArray();

            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, fileNames);
            return memoryStream;
        }

        /// <summary>
        /// 解析文件信息流
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        private static string[] GetFileNames(MemoryStream memoryStream)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            memoryStream.Position = 0;
            //memoryStream.Seek(0, SeekOrigin.Begin);
            string[] fileNames = binaryFormatter.Deserialize(memoryStream) as string[];
            memoryStream.Close();
            return fileNames;
        }

        /// <summary>
        /// 从流还原出文件
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <param name="filePath"></param>
        private static void MemoryStreamToFile(MemoryStream memoryStream, string filePath)
        {
            byte[] buffer = new byte[memoryStream.Length];
            memoryStream.Read(buffer, 0, (int)memoryStream.Length);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(buffer, 0, (int)memoryStream.Length);
                fileStream.Close();
            }

            memoryStream.Close();
        }
    }
}