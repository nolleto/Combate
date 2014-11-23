using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tela.Enums
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }        
    }

    public static class EnumUtil
    {
        public static T[] GetArray<T>(this T e)
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToArray();
        }
    }
}
