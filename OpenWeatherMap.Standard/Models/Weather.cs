using System;

namespace OpenWeatherMap.Standard.Models
{
    /// <summary>
    /// weather model
    /// </summary>
    [Serializable]
    public class Weather : BaseModel
    {
        public Weather()
        {
            main = string.Empty;
            desc = string.Empty;
            icon = string.Empty;
            iconData = Array.Empty<byte>();
        }
        private int id;
        private string main, desc, icon;
        private byte[] iconData;

        /// <summary>
        /// id
        /// </summary>
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        /// <summary>
        /// main
        /// </summary>
        public string Main
        {
            get => main;
            set => SetProperty(ref main, value);
        }

        /// <summary>
        /// desc
        /// </summary>
        public string Description
        {
            get => desc;
            set => SetProperty(ref desc, value);
        }

        /// <summary>
        /// icon id
        /// </summary>
        public string Icon
        {
            get => icon;
            set => SetProperty(ref icon, value);
        }

        public byte[] IconData
        {
            get => iconData;
            set => SetProperty(ref iconData, value);
        }
    }

}
