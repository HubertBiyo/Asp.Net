using System;
using System.Security.Cryptography;
using System.Text;

namespace QOL.Common.Security
{
    public static class DES
    {
        private const string DESKey = "kkahgsdfkjhasdgfkJHGK*&^*&HGFJHDsi87f69832q(*&(&*^5rhgKJHGFJdaksjf";

        /// <summary>
        /// 3des加密字符串
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>加密后并经base63编码的字符串</returns>
        /// <remarks>重载，指定编码方式</remarks>
        public static string Encrypt3DES(string plainText)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            DES.Key = hashMD5.ComputeHash(Encoding.UTF8.GetBytes(DESKey));
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            byte[] Buffer = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        /// <summary>
        /// 3des解密字符串
        /// </summary>
        /// <param name="entryptText">密文</param>
        /// <returns>解密后的字符串</returns>
        /// <remarks>静态方法，指定编码方式</remarks>
        public static string Decrypt3DES(string entryptText)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            DES.Key = hashMD5.ComputeHash(Encoding.UTF8.GetBytes(DESKey));
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();
            byte[] Buffer = Convert.FromBase64String(entryptText);
            return Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }
    }
}
