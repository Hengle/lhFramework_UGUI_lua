/// <summary>
/// 提供用于计算指定文件哈希值的方法
/// <example>例如计算文件的MD5值:
/// <code>
///   string hashMd5=HashHelper.ComputeMD5("MyFile.txt");

using System.Text;

namespace LaoHan.Infrastruture
{
    public sealed class lhHash
    {
        /// <summary>
        /// 提供 CRC32 算法的实现
        /// </summary>
        public class Crc32 : System.Security.Cryptography.HashAlgorithm
        {
            public const uint DefaultPolynomial = 0xedb88320;
            public const uint DefaultSeed = 0xffffffff;
            private uint hash;
            private uint seed;
            private uint[] table;
            private static uint[] defaultTable;
            public Crc32()
            {
                table = InitializeTable(DefaultPolynomial);
                seed = DefaultSeed;
                Initialize();
            }
            public Crc32(uint polynomial, uint seed)
            {
                table = InitializeTable(polynomial);
                this.seed = seed;
                Initialize();
            }
            public override void Initialize()
            {
                hash = seed;
            }
            protected override void HashCore(byte[] buffer, int start, int length)
            {
                hash = CalculateHash(table, hash, buffer, start, length);
            }
            protected override byte[] HashFinal()
            {
                byte[] hashBuffer = UInt32ToBigEndianBytes(~hash);
                this.HashValue = hashBuffer;
                return hashBuffer;
            }
            public static uint Compute(byte[] buffer)
            {
                return ~CalculateHash(InitializeTable(DefaultPolynomial), DefaultSeed, buffer, 0, buffer.Length);
            }
            public static uint Compute(uint seed, byte[] buffer)
            {
                return ~CalculateHash(InitializeTable(DefaultPolynomial), seed, buffer, 0, buffer.Length);
            }
            public static uint Compute(uint polynomial, uint seed, byte[] buffer)
            {
                return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
            }
            private static uint[] InitializeTable(uint polynomial)
            {
                if (polynomial == DefaultPolynomial && defaultTable != null)
                {
                    return defaultTable;
                }
                uint[] createTable = new uint[256];
                for (int i = 0; i < 256; i++)
                {
                    uint entry = (uint)i;
                    for (int j = 0; j < 8; j++)
                    {
                        if ((entry & 1) == 1)
                            entry = (entry >> 1) ^ polynomial;
                        else
                            entry = entry >> 1;
                    }
                    createTable[i] = entry;
                }
                if (polynomial == DefaultPolynomial)
                {
                    defaultTable = createTable;
                }
                return createTable;
            }
            private static uint CalculateHash(uint[] table, uint seed, byte[] buffer, int start, int size)
            {
                uint crc = seed;
                for (int i = start; i < size; i++)
                {
                    unchecked
                    {
                        crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
                    }
                }
                return crc;
            }
            private byte[] UInt32ToBigEndianBytes(uint x)
            {
                return new byte[] { (byte)((x >> 24) & 0xff), (byte)((x >> 16) & 0xff), (byte)((x >> 8) & 0xff), (byte)(x & 0xff) };
            }
        }
        /// <summary>
        ///  计算指定文件的MD5值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static string ComputeMD5(string fileName)
        {
            string hashMD5 = string.Empty;
            //检查文件是否存在，如果文件存在则进行计算，否则返回空值
            if (System.IO.File.Exists(fileName))
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    //计算文件的MD5值
                    System.Security.Cryptography.MD5 calculator = System.Security.Cryptography.MD5.Create();
                    byte[] buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    //将字节数组转换成十六进制的字符串形式
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("x2"));
                    }
                    hashMD5 = stringBuilder.ToString();
                }//关闭文件流
            }//结束计算
            return hashMD5;
        }//ComputeMD5
        /// <summary>
        ///  计算指定文件的CRC32值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static string ComputeCRC32(string fileName)
        {
            string hashCRC32 = string.Empty;
            //检查文件是否存在，如果文件存在则进行计算，否则返回空值
            if (System.IO.File.Exists(fileName))
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    //计算文件的CSC32值
                    Crc32 calculator = new Crc32();
                    byte[] buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    //将字节数组转换成十六进制的字符串形式
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("x2"));
                    }
                    hashCRC32 = stringBuilder.ToString();
                }//关闭文件流
            }
            return hashCRC32;
        }//ComputeCRC32
        /// <summary>
        ///  计算指定文件的SHA1值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static string ComputeSHA1(string fileName)
        {
            string hashSHA1 = string.Empty;
            //检查文件是否存在，如果文件存在则进行计算，否则返回空值
            if (System.IO.File.Exists(fileName))
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    //计算文件的SHA1值
                    System.Security.Cryptography.SHA1 calculator = System.Security.Cryptography.SHA1.Create();
                    byte[] buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    //将字节数组转换成十六进制的字符串形式
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("x2"));
                    }
                    hashSHA1 = stringBuilder.ToString();
                }//关闭文件流
            }
            return hashSHA1;
        }//ComputeSHA1
    }
}