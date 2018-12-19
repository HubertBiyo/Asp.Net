using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Common.Extensions
{
    public static class Extend
    {
        #region  把字符串转换为指定的类型,如果转换不成功,返回指定类型的默认值...
        /// <summary>
        /// 把字符串转换为指定的类型,如果转换不成功,返回指定类型的默认值.
        /// </summary>
        /// <typeparam name="T">要转换的类型</typeparam>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this string source)
        {
            if (String.IsNullOrEmpty(source))
            {
                return default(T);
            }
            source = source.Trim();
            try
            {
                return (T)Convert.ChangeType(source, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 把字符串转换为指定的类型,如果转换不成功,返回指定类型的默认值.
        /// </summary>
        /// <typeparam name="T">要转换的类型</typeparam>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this string source, T defaultValue)
        {
            if (String.IsNullOrEmpty(source))
            {
                return defaultValue;
            }
            source = source.Trim();
            try
            {
                return (T)Convert.ChangeType(source, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region 获取对象的自定义说明...
        private static EnumCache<string, string> descriptionCache = new EnumCache<string, string>();
        /// <summary>
        /// 获取对象的自定义说明...
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDescription(this object obj)
        {

            Type type = obj.GetType();
            string key = String.Concat(type.Namespace, type.FullName, obj.ToString());

            return descriptionCache.Get(key, () =>
            {
                FieldInfo field = type.GetField(obj.ToString());
                if (field == null)
                {
                    return null;
                }
                var desc = field.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
                if (desc == null)
                {
                    return null;
                }
                return ((DescriptionAttribute)desc).Description;
            });
        }
        #endregion

        #region 获取枚举的值与说明 By:Libing
        /// <summary>
        /// 获取枚举的值与说明
        /// </summary>
        /// <param name="obj">枚举对象</param>
        /// <returns></returns>
        public static Dictionary<int, string> EnumToDict(Type type)
        {
            if (type == null)
            {
                return null;
            }
            var enums = Enum.GetValues(type);
            Dictionary<int, string> dict = new Dictionary<int, string>();
            foreach (var item in enums)
            {
                dict.Add((int)item, item.GetDescription());
            }
            return dict;
        }
        #endregion

        #region Md5 加密方法.......
        /// <summary>
        /// Md5 加密方法
        /// </summary>
        /// <param name="inputStr">要加密的字符串</param>
        /// <returns></returns>
        public static string EncryptMD5(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }
            string strReturn = "";
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytInput = System.Text.UTF8Encoding.Default.GetBytes(source);
                byte[] bytArray = md5.ComputeHash(bytInput);
                for (int i = 0; i < bytArray.Length; i++)
                {
                    strReturn += bytArray[i].ToString("X");
                }
                bytArray = null;
                bytInput = null;
                return strReturn;
            }
        }
        #endregion


        #region 加密 解密 方法

        private static string CryptKey = "64623FB229B4463C99922C9C";

        public static string Encrypt(this string source, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                key = CryptKey;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(source);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            return ret.ToString();
        }

        public static string Decrypt(this string source, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                key = CryptKey;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = source.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(source.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion
    }
}
