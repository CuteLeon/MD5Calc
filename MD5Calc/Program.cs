using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MD5Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int index = 0; index < 10000; index++)
            {
                string hash = HashCalculator.GetHash("Greet from Leon.");
            }

            return;
            string targetDir = args.Length > 0 ?
                args[0] :
                AppDomain.CurrentDomain.BaseDirectory;

            if (!Directory.Exists(targetDir))
            {
                Console.WriteLine($"未找到文件存放目录：{targetDir}");
                Environment.Exit(-2);
            }

            string exportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("md5_{0}_{1}.txt", Path.GetFileName(targetDir), DateTime.Now.ToString("hhMMddHHmmssfff")));

            Console.WriteLine($"开始计算文件MD5，工作目录：{targetDir}");
            using (FileStream fileStream = new FileStream(exportPath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    Array.ForEach(
                    Directory.GetFiles(targetDir, "*", SearchOption.AllDirectories),
                    path =>
                    {
                        lock (streamWriter)
                        {
                            streamWriter.WriteLine(string.Format("{0} = {1}",
                                Path.GetFileName(path),
                                GetMD5HashFromFile(path)));
                        }
                    });
                }
            }

            GC.Collect();

            Console.WriteLine("任务完成！\n\t=> {0}", exportPath);
            Process.Start("notepad", exportPath);
            Console.ReadLine();
        }

        private static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] hash = md5.ComputeHash(fileStream);
                    return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
    }
}
