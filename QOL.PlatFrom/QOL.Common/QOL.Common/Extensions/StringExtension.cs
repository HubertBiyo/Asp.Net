using System;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace QOL.Common.Extensions
{
    public static class StringExtension
    {
        public static string ToJsonEscape(this string str)
        {
            return str.Replace("\"", "\\\"")
                      .Replace("\\", "\\\\")
                      .Replace("/", "\\/");
        }

        /// <summary>
        /// 将字符串截取为"ab21v..."形式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubBrifString(this string str, int length)
        {
            if (length <= 3)
            {
                return str;
            }

            Regex regex = new Regex("[\u4e00-\u9fa5]");
            int strLength = 0;
            foreach (var c in str)
            {
                if (regex.IsMatch(c.ToString()))
                {
                    strLength += 2;
                }
                else
                {
                    strLength++;
                }
            }

            if (length >= strLength)
            {
                return str;
            }

            int newStrLength = 0;
            int maxStrLength = length - 3;
            StringBuilder sb = new StringBuilder();

            foreach (char c in str)
            {
                if (newStrLength >= maxStrLength)
                {
                    break;
                }

                if (regex.IsMatch(c.ToString()))
                {
                    newStrLength += 2;
                }
                else
                {
                    newStrLength++;
                }

                sb.Append(c);
            }

            sb.Append("...");

            return sb.ToString();
        }

        /// <summary>
        /// 将指定的字符串按位对齐，超长截取，不足位按指定字符补齐
        /// </summary>
        /// <example>字符串abcd，按位数8补齐，填充字符0，输出结果为：abcd0000</example>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="fill"></param>
        /// <returns></returns>
        public static string AlignToLength(this string str, int length, char fill)
        {
            var sb = new StringBuilder(length);

            for (var i = 0; i < length; i++)
            {
                sb.Append(i >= str.Length ? fill : str[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 在给定的字符串后面追加查询字符串
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string AppendQueryString(this string baseUrl, NameValueCollection parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(baseUrl);
            sb.Append("?");

            foreach (string item in parameters)
            {
                sb.Append(string.Format("{0}={1}&", item, parameters[item]));
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        /// <summary>
        /// 判断是否是Email
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(this string value)
        {
            return Regex.IsMatch(value.Trim(), @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 判断是否是手机号
        /// </summary>
        /// <param name="value"></param>
        /// <param name="countryKey"></param>
        /// <returns></returns>
        public static bool IsMobile(this string value, string countryKey = "CN")
        {
            if (countryKey == "CN")
            {
                return Regex.IsMatch(value.Trim(), @"^(1\d{10})$");
            }
            return Regex.IsMatch(value.Trim(), @"^(\d{5,15})$");
        }

        /// <summary>
        /// 隐藏手机号中间数字
        /// </summary>
        /// <param name="value"></param>
        /// <param name="num">后面保留的个数</param>
        /// <returns></returns>
        public static string Hide(this string value, int num = 2)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            try
            {
                if (value.Length > 8)
                {
                    string pre = value.Substring(0, 3);
                    string end;
                    if (value.Contains("@"))
                    {
                        int pos = value.LastIndexOf('@');
                        end = value.Substring(pos, value.Length - pos);
                    }
                    else
                    {
                        end = value.Substring(value.Length - num, num);
                    }

                    return string.Format("{0}*****{1}", pre, end);
                }
            }
            catch (Exception)
            {
            }
            return value;
        }
    }
}
