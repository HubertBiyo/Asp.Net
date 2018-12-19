using System;

namespace QOL.Common.Extensions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 读取首个标记了指定特性的属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static V GetPropertyValueMarkedByAttribute<T, V>(this object obj) where T : Attribute
        {
            object propertyValue = null;

            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                bool found = false;

                var attributes = property.GetCustomAttributes(typeof(T), false);
                foreach (var attr in attributes)
                {
                    if (attr is T)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    if (property.CanRead)
                    {
                        propertyValue = property.GetValue(obj, null);
                    }

                    break;
                }
            }

            return propertyValue is V ? (V)propertyValue : default(V);
        }
    }
}
