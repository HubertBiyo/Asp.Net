using System;
using System.Text;

namespace QOL.Common.Security
{
    public static class Randoms
    {
        public static string CleanGUID()
        {
            return System.Guid.NewGuid().ToString("N");
        }

        public static string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(10, 99));
            builder.Append(RandomString(2, false));

            return builder.ToString();
        }

        public static string CreateSalt()
        {
            return System.Guid.NewGuid().ToString("N");
        }

        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random((int)DateTime.UtcNow.Ticks);
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
            {
                return builder.ToString().ToLower();
            }
            return builder.ToString();
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random((int)DateTime.UtcNow.Ticks);
            return random.Next(min, max);
        }
    }
}
