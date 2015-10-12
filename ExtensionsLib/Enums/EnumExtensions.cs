using System;
using System.ComponentModel;
using System.Reflection;

namespace ExtensionsLib.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T? ToEnum<T>(this string value) where T : struct
        {
            if (string.IsNullOrEmpty(value)) return default(T);
                T result;

            return Enum.TryParse<T>(value, true, out result) ? result : default(T);
        }

    }
}
