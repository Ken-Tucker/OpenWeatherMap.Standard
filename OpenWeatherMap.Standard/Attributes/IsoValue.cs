using System;

namespace OpenWeatherMap.Standard.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class IsoValue : Attribute
    {
        public IsoValue(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}