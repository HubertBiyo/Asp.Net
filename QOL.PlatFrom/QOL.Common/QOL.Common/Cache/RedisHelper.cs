using QOL.Common.Configuration;
using QOL.Common.Log;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Text;

namespace QOL.Common.Cache
{
    public class RedisHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["RedisConnectionString"].ConnectionString;
        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _lock = new object();
        /// <summary>
        /// 连接对象
        /// </summary>
        private volatile IConnectionMultiplexer _connection;
        /// <summary>
        /// 数据库
        /// </summary>
        private IDatabase _db;
        public RedisHelper()
        {
            if (!WebConfig.GetBoolean("CacheEnabled"))
            {
                Logger.Trace("Cache is not enabled");
                return;
            }

            _connection = ConnectionMultiplexer.Connect(ConnectionString);
            _db = GetDatabase();
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        protected IConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection;
            }
            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected)
                {
                    return _connection;
                }

                if (_connection != null)
                {
                    _connection.Dispose();
                }
                _connection = ConnectionMultiplexer.Connect(ConnectionString);
            }

            return _connection;
        }
        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        //public virtual void Set(string key, object data)
        //{
        //    if (data == null)
        //    {
        //        return;
        //    }
        //    var entryBytes = Serialize(data);
        //    var expiresIn = TimeSpan.FromMinutes(30);
        //    _db.StringSet(key, entryBytes, expiresIn);
        //}
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="cacheTime">时间</param>
        //public virtual void Set(string key, object data, int cacheTime)
        //{
        //    if (data == null)
        //    {
        //        return;
        //    }
        //    var entryBytes = Serialize(data);
        //    var expiresIn = TimeSpan.FromMinutes(cacheTime);

        //    _db.StringSet(key, entryBytes, expiresIn);
        //}
        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        //public virtual T Get<T>(string key)
        //{

        //    var rValue = _db.StringGet(key);
        //    if (!rValue.HasValue)
        //    {
        //        return default(T);
        //    }

        //    var result = Deserialize<T>(rValue);

        //    return result;
        //}
        //public virtual void RemoveKey(string key)
        //{
        //    if (IsSet(key))
        //        _db.KeyDelete(key);
        //}

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
            {
                return default(T);
            }
            var json = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 判断是否已经设置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            return _db.KeyExists(key);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns>byte[]</returns>
        private byte[] Serialize(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(json);
        }

        public void SetHash(string category, string key, object data)
        {
            if (_connection == null)
                return;

            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(key) || data == null)
                return;

            var entryBytes = Serialize(data);
            var expiresIn = TimeSpan.FromMinutes(30);
            _db.HashSet(category, key, entryBytes);
        }

        public T GetHash<T>(string category, string key)
        {
            if (_connection == null)
                return default(T);

            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            var rValue = _db.HashGet(category, key);
            var result = Deserialize<T>(rValue);
            return result;
        }

        public void HashClear(string category, string key)
        {
            if (_connection == null)
                return;

            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(key))
            {
                return;
            }

            if (_db.HashExists(category, key))
                _db.HashDelete(category, key);

        }

        public bool KeyDelete(string key)
        {
            if (_connection == null)
                return false;

            if (_db.KeyExists(key))
                return _db.KeyDelete(key);

            return false;
        }
    }
}
