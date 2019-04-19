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
        /// 计算文件哈希
        /// </summary>
        /// <param name="plaintText"></param>
        /// <returns></returns>
        public static string GetHash(string plaintText)
        {
            return GetString(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(plaintText)));
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
