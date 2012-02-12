using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FortuneCookie.PropertySecurity.Discovery
{
    internal static class AttributedTypesUtility
    {
        public static T GetAttributeFromType<T>(Type type)
        {
            object[] attributes = type.GetCustomAttributes(false);
            foreach (object attributeInType in attributes)
            {
                if (attributeInType is T)
                    return (T)attributeInType;
            }

            return default(T);
        }

        public static IEnumerable<T> GetAttributesFromProperty<T>(PropertyInfo propertyInfo)
        {
            object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(T), true);
            return customAttributes.Cast<T>().ToList();
        }

        public static bool PropertyHasAttribute<T>(PropertyInfo propertyInfo)
        {
            object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(T), true);
            return customAttributes.Any();
        } 

        public static PropertyInfo[] GetPublicOrPrivateProperties(Type type)
        {
            return type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        }


    }
}