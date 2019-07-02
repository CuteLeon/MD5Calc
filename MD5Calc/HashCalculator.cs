using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MD5Calc
{
    /// <summary>
    /// 哈希计算器
    /// </summary>
    public static class HashCalculator
    {
        /// <summary>
        /// 哈希算法
        /// </summary>
        /// <remarks>线程安全、懒加载</remarks>
        private static readonly HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();

        /// <summary>
        /// 计算哈希
        /// </summary>
        /// <param name="plaintText"></param>
        /// <returns></returns>
        public static string GetHash(string plaintText)
        {
            lock (hashAlgorithm)
            {
                try
                {

                    return GetString(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(plaintText)));
                }
                catch (Exception ex)
                {
                    return $"计算哈希失败：{ex.Message}";
                }
            }
        }

        /// <summary>
        /// 计算文件哈希
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileHash(string path)
        {
            lock (hashAlgorithm)
            {
                try
                {
                    return GetString(hashAlgorithm.ComputeHash(File.ReadAllBytes(path)));
                }
                catch (Exception ex)
                {
                    return $"计算文件哈希失败：{ex.Message}";
                }
            }
        }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string GetString(byte[] values)
        {
            int valueCount = values.Length,
                charCount = values.Length * 2,
                charIndex = 0;
            char[] chars = new char[charCount];

            for (int valueIndex = 0; valueIndex < valueCount; valueIndex++)
            {
                byte currentValue = values[valueIndex];
                chars[charIndex++] = GetHexValue(currentValue / 16);
                chars[charIndex++] = GetHexValue(currentValue % 16);
            }

            char GetHexValue(int i)
                => (char)(i < 10 ? i + 48 : i + 55);

            return new string(chars);
        }
    }
}
