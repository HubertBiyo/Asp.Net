using System.IO;
using System.Text;

namespace QOL.Common.Serialization
{
    public static class XmlSerializer
    {
        /// <summary>
        /// 将对象序列化为Xml流
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                serializer.Serialize(ms, obj);
                ms.Position = 0;

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 将Xml流反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="contentEncoding"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream, Encoding contentEncoding = null)
        {
            if (contentEncoding == null)
            {
                contentEncoding = Encoding.Default;
            }
            using (StreamReader reader = new StreamReader(stream, contentEncoding))
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(reader);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        /// <summary>
        /// 将Xml字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <param name="contentEncoding"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xmlString, Encoding contentEncoding = null)
        {
            if (contentEncoding == null)
            {
                contentEncoding = Encoding.Default;
            }
            using (MemoryStream ms = new MemoryStream(contentEncoding.GetBytes(xmlString)))
            {
                return Deserialize<T>(ms);
            }
        }
    }
}
