using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Common.Tools
{
    public class DateTimeHelper
    {
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(DateTime time)
        {
            return time.Ticks;
        }
        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            return new DateTime(Convert.ToInt64(timeStamp));
        }

        public static string ConvertNormalTime(string utcStr)
        {
            if (string.IsNullOrEmpty(utcStr))
                return string.Empty;

            utcStr = utcStr.Replace("T", " ").Replace("Z", "");

            return DateTime.Parse(utcStr).AddHours(8).ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
