using QOL.Common.Configuration;
using System;
using System.Collections.Generic;

namespace QOL.Common.Cache
{
    public class MemCache
    {
        public object Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            var isEnable = WebConfig.GetBoolean("CacheEnabled");
            if (!isEnable)
                return null;

            lock (_lock)
            {
                CacheUnit unit;

                if (_cache.TryGetValue(key, out unit))
                {
                    if (DateTime.Now.CompareTo(unit.ExpireTime) <= 0)
                    {
                        return unit.Data;
                    }

                    _cache.Remove(key);
                    return null;
                }

                return null;
            }
        }

        public void Set(string key, object data)
        {
            var isEnable = WebConfig.GetBoolean("CacheEnabled");
            if (!isEnable)
                return;

            Set(key, data, TimeSpan.FromMinutes(30));
        }

        public void Set(string key, object data, TimeSpan expire)
        {
            if (string.IsNullOrWhiteSpace(key) || data == null)
            {
                return;
            }

            lock (_lock)
            {
                var now = DateTime.Now;

                //Add cache
                if (_cache.ContainsKey(key))
                {
                    var unit = _cache[key];
                    unit.Data = data;
                    unit.ExpireTime = now.Add(expire);
                }
                else
                {
                    _cache.Add(key, new CacheUnit
                    {
                        Key = key,
                        Data = data,
                        ExpireTime = now.Add(expire)
                    });
                }

                //Remove expired
                foreach (var pair in _cache)
                {
                    if (now.CompareTo(pair.Value.ExpireTime) > 0)
                    {
                        _remove.Add(pair.Value.Key);
                    }
                }
                foreach (var removeKey in _remove)
                {
                    _cache.Remove(removeKey);
                }
                _remove.Clear();
            }
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            lock (_lock)
            {
                return _cache.Remove(key);
            }
        }

        public int Clear()
        {
            lock (_lock)
            {
                int count = _cache.Count;

                _cache.Clear();
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                return count;
            }
        }

        public MemCache()
        {
            _instances.Add(this);
        }

        private class CacheUnit
        {
            public string Key { set; get; }

            public DateTime ExpireTime { set; get; }

            public object Data { set; get; }
        }

        private readonly object _lock = new object();
        private readonly List<string> _remove = new List<string>();
        private readonly Dictionary<string, CacheUnit> _cache = new Dictionary<string, CacheUnit>();

        #region Static

        public static MemCache DefaultInstance
        {
            get { return _defaultInstance; }
        }

        public static int ClearAllCache()
        {
            lock (_instances)
            {
                int all = 0;

                foreach (var memCach in _instances)
                {
                    all += memCach.Clear();
                }

                return all;
            }
        }

        static MemCache()
        {
            _instances = new List<MemCache>();
            _defaultInstance = new MemCache();
        }

        private static readonly MemCache _defaultInstance;
        private readonly static List<MemCache> _instances;

        #endregion
    }
}
