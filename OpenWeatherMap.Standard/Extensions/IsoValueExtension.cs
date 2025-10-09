using OpenWeatherMap.Standard.Attributes;
using System;

namespace OpenWeatherMap.Standard.Extensions
{
    public static class IsoValueExtension
    {
        public static string GetStringValue(this Enum? value)
        {
            if (value is null)
                return string.Empty;
            
            var stringValue = value.ToString();
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());

            if (fieldInfo?.GetCustomAttributes(typeof(IsoValue), false) is IsoValue[] attrs && attrs.Length > 0)
                stringValue = attrs[0].Value;

            return stringValue;
        }
    }
}