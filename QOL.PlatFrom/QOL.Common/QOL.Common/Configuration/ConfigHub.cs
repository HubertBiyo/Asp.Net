using System;
using System.Data.SqlClient;
using QOL.Common.Cache;
using QOL.Common.Data;
#if DEBUG
#else
using QOL.Common.Security;
#endif

namespace QOL.Common.Configuration
{
    public class ConfigHub
    {
        /// <summary>
        /// 读取数据库连接字符串
        /// 数据库连接串没有命名空间
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type">默认为只读</param>
        /// <returns></returns>
        public static string GetConnectionString(string areakey, string name, SqlTypes type = SqlTypes.ReadOnly)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            var cachekey = string.Format("ConnectionString_{0}_{1}_{2}", areakey,name, (int)type);
            var cacheObj = _cache.Get(cachekey);
            if (cacheObj != null)
            {
                return cacheObj.ToString();
            }

            string data;

            using (var command = new SqlCommand(_cssql))
            {
                command.Parameters.AddWithValue("@area", areakey);
                command.Parameters.AddWithValue("@key", name);
                command.Parameters.AddWithValue("@type", (int)type);

                using (var executor = new SqlExecutor())
                {
                    data = executor.ExecuteScalar<string>(command, _configConnectionString);
                }
            }

            _cache.Set(cachekey, data, TimeSpan.FromMinutes(10));
            return data;
        }

        #region Static

        private static readonly string _configConnectionString = WebConfig.GetSqlConnectionString("Config");

        private static readonly MemCache _cache = new MemCache();

        private const string _cssql = @"
SELECT [Value] 
  FROM [dbo].[DbConnection]
 WHERE [JDABKey] = @key AND [ReadOrWrite] = @type";


        #endregion
    }
}
