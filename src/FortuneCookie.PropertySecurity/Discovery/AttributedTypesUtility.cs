using System;

namespace FortuneCookie.PropertySecurity.Discovery
{
    internal static class AttributedTypesUtility
    {
        public static T GetAttribute<T>(Type type)
        {
            object[] attributes = type.GetCustomAttributes(true);
            foreach (object attributeInType in attributes)
            {
                if (attributeInType is T)
                    return (T)attributeInType;
            }

            return default(T);
        }
    }
}