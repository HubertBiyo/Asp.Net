using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Common.Security
{
    public static class Generate
    {
        /// <summary>
        /// 获得三位的随机数
        /// </summary>
        /// <returns></returns>
        public static string GetTreeNumRandom()
        {
            Random ro = new Random();
            int iResult;
            int iUp = 999;
            int iDown = 100;
            iResult = ro.Next(iDown, iUp);
            return iResult.ToString().Trim();
        }

        /// <summary>
        /// 新的日期字符串
        /// </summary>
        /// <param name="oldShortDateTime"></param>
        /// <returns></returns>
        public static string generateNewLongDateTime(string oldShortDateTime)
        {
            System.DateTime currentTime = DateTime.Now;
            string LongTimeString = currentTime.ToLongTimeString().ToString().Trim();//10:01:01
            string DateAndTime = oldShortDateTime.Trim() + " " + LongTimeString.Trim();   //2004-01-02 10:01:01 组成一个新的日期字符串
            DateTime NewDateAndTime = Convert.ToDateTime(DateAndTime);
            DateAndTime = NewDateAndTime.ToString("yyyyMMddhhmmss");
            return DateAndTime;

        }
    }
}
