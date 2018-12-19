using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace QOL.Common.Serialization
{
    public static class JsonSerializer
    {
        /// <summary>
        /// 将对象序列化为Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
#if DEBUG
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
#else
            return JsonConvert.SerializeObject(obj, Formatting.None);
#endif
        }

        /// <summary>
        /// 将Json流反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string jsonString)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return Deserialize<T>(ms);
            }
        }
    }
}
