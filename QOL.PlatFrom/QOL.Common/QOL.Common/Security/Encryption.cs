using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace QOL.Common.Security
{
    public static class Encryption
    {
        private const string DefaultKey = "LKJAHSDFKUASGDF(*&^%*&^Rduiasdfhp8YUOhl;ihjfasdf89)(*&ahsdflh21&^%hsadflkhkjh)(gdfKJHGGGGKJGsakjfdhkjhgsdf>::KHGsadf78687^*&^*{}<>/.,KJGKJHGcdzxvx76v5IKGKJGHdf675765HJGFJHdfsfsdf8769ou3429(*&^*&%wer023847yKFGKJHGF<><>[[]])){}{po9[](*)*iuaosiudfiasdofyu*&^!";
        private const string DefaultAESKey = "PnwkalGMYZxQkFr8lEaXrlRtXVIo99UK";
        private const string DefaultIV = "OpenXLive+-*&Hy$";
        private const string DefaultHMACKey = "Hhgjasd7676GS5s81NHt612Bswd76Bds86JOI8iioiGS123Gd76afuY69JxU30Oc";

        public static string Encrypt(string plainText)
        {
            return Encrypt(plainText, DefaultKey);
        }

        public static string Encrypt(string plainText, string key)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            DES.Key = hashMD5.ComputeHash(Encoding.UTF8.GetBytes(key));
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            byte[] Buffer = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        public static string Decrypt(string entryptText)
        {
            return Decrypt(entryptText, DefaultKey);
        }

        public static string Decrypt(string entryptText, string key)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            DES.Key = hashMD5.ComputeHash(Encoding.UTF8.GetBytes(key));
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();
            byte[] Buffer = Convert.FromBase64String(entryptText);
            return Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        public static string EncryptAES(string plainText)
        {
            return EncryptAES(plainText, DefaultIV, DefaultAESKey);
        }

        public static string EncryptAES(string plainText, string iv, string key)
        {
            SymmetricAlgorithm aes = Rijndael.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static string DecryptAES(string entryptText)
        {
            return DecryptAES(entryptText, DefaultIV, DefaultAESKey);
        }

        public static string DecryptAES(string entryptText, string iv, string key)
        {
            byte[] cipher = Convert.FromBase64String(entryptText);

            SymmetricAlgorithm aes = Rijndael.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            using (MemoryStream ms = new MemoryStream(cipher))
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (MemoryStream resultMS = new MemoryStream())
                    {
                        while (true)
                        {
                            int read = cs.ReadByte();
                            if (read == -1)
                            {
                                break;
                            }

                            resultMS.WriteByte((byte)read);
                        }

                        return Encoding.UTF8.GetString(resultMS.ToArray());
                    }

                }
            }
        }

        public static byte[] EncryptMD5(byte[] data)
        {
            return new MD5CryptoServiceProvider().ComputeHash(data);
        }

        public static string EncryptMD5(string plainText)
        {
            var bytes = EncryptMD5(Encoding.UTF8.GetBytes(plainText));
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static string EncryptHMACMD5(string plainText)
        {
            return EncryptHMACMD5(plainText, DefaultHMACKey);
        }

        public static string EncryptHMACMD5(string plainText, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] md5Bytes = new HMACMD5(keyBytes).ComputeHash(plainBytes);

            StringBuilder sb = new StringBuilder();
            foreach (var b in md5Bytes)
            {
                sb.Append(b.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

    }
}
