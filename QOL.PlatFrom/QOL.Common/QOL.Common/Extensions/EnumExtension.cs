using System;

namespace QOL.Common.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举的显示名称。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayString(this Enum value)
        {
            return value.ToString();
        }
    }
}
