using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Common.Extensions
{
    public class EnumCache<TKey, TValue>
    {

        private Dictionary<TKey, TValue> dictionary;

        public EnumCache(IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public EnumCache()
            : this(null)
        {
        }

        private readonly object mutexLock = new object();


        public TValue Get(TKey key, Func<TValue> creator)
        {
            TValue value;
            if (this.dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            lock (this.mutexLock)
            {
                if (this.dictionary.TryGetValue(key, out value))
                {
                    return value;
                }
                value = creator();
                var newDictionary = this.dictionary.ToDictionary(d => d.Key, d => d.Value, this.dictionary.Comparer);
                newDictionary.Add(key, value);
                this.dictionary = newDictionary;
                return value;
            }
        }
    }
}
