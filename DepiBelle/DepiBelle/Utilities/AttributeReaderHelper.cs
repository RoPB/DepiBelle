using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugin.CloudFirestore.Attributes;

namespace DepiBelle.Utilities
{
    public static class AttributeReaderHelper
    {
    
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }

        public static TValue GetPropertyAttribute<TAttribute,TValue>(
            this PropertyInfo prop,
            Func<TAttribute, TValue> valueSelector
            ) where TAttribute : MapToAttribute
        {
            var att = prop.GetCustomAttributes(
               typeof(TAttribute), true
           ).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
    }

}
