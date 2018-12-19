using System.Collections.Generic;
using System.Dynamic;

namespace QOL.Common.Extensions
{
    public static class ExpandoObjectExtension
    {
        /// <summary>
        /// 判断ExpandoObject是否包含指定的成员
        /// </summary>
        /// <param name="expando"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static bool HasMember(this ExpandoObject expando, string memberName)
        {
            var dic = expando as IDictionary<string, object>;
            return dic.ContainsKey(memberName);
        }
    }
}
