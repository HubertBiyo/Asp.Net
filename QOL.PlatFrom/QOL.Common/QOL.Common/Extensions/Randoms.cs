using System;
using System.Text;

namespace QOL.Common.Extensions
{
    public static class Randoms
    {
        public static string CleanGUID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string RandomPassword()
        {
            var builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(10, 99));
            builder.Append(RandomString(2, false));

            return builder.ToString();
        }

        public static string CreateSalt()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static int MinPasswordLength()
        {
            const int minPasswordLength = 6;
            return minPasswordLength;
        }

        private static string RandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            var random = new Random((int)DateTime.UtcNow.Ticks);
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public static int RandomNumber(int min, int max)
        {
            var random = new Random((int)DateTime.UtcNow.Ticks);
            return random.Next(min, max);
        }

        public static string GetRandom()
        {
            Random ro = new Random();
            int iResult;
            int iUp = 999999;
            int iDown = 100000;
            iResult = ro.Next(iDown, iUp);
            return iResult.ToString().Trim();
        }
    }
}
