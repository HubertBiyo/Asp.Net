using System;
using System.Text;

namespace QOL.Common.Security
{
    public static class SecretGenerator
    {
        /// <summary>
        /// 生成64位密钥
        /// </summary>
        /// <returns></returns>
        public static string NewPaymentSecret()
        {
            StringBuilder keyBuilder = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                keyBuilder.Append(CryptogramAlphabet[_random.Next(0, CryptogramAlphabet.Length - 1)]);
            }

            return Encryption.Encrypt(keyBuilder.ToString());
        }

        private static readonly Random _random = new Random();
        private const string CryptogramAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    }
}
