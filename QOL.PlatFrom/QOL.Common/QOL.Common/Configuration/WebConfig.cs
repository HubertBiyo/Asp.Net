using System;
using System.Configuration;

namespace QOL.Common.Configuration
{
    public static class WebConfig
    {
        /// <summary>
        /// 读取string类型设置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException(string.Format("Configuration \"{0}\" not found.", key));
            }
            return value;
        }

        public static bool GetBoolean(string key)
        {
            string strValue = GetString(key);
            bool value;
            if (!bool.TryParse(strValue, out value))
            {
                throw new ConfigurationErrorsException(string.Format("Value of configuration \"{0}\" is not a valid int type.", key));
            }
            return value;
        }

        /// <summary>
        /// 读取int类型设置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetInt(string key)
        {
            string strValue = GetString(key);
            int value;
            if (!int.TryParse(strValue, out value))
            {
                throw new ConfigurationErrorsException(string.Format("Value of configuration \"{0}\" is not a valid int type.", key));
            }
            return value;
        }

        /// <summary>
        /// 读取正整数设置值(>=0)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetPositiveInt(string key)
        {
            int value = GetInt(key);
            if (value < 0)
            {
                throw new ConfigurationErrorsException(string.Format("Value of configuration \"{0}\" must be a positive int number.", key));
            }
            return value;
        }

        /// <summary>
        /// 读取DateTimeUTC类型设置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeUTC(string key)
        {
            string strValue = GetString(key);
            DateTime value;
            if (!DateTime.TryParse(strValue, out value))
            {
                throw new ConfigurationErrorsException(string.Format("Value of configuration \"{0}\" is not a valid DateTime type.", key));
            }

            return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, DateTimeKind.Utc);
        }

        /// <summary>
        /// 读取连接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSqlConnectionString(string name)
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[name];
            if (connectionString == null)
            {
                return null;
            }
            return connectionString.ConnectionString;
        }
    }
}
