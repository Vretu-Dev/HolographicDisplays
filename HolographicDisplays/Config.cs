using Exiled.API.Interfaces;
using System.ComponentModel;
using UnityEngine.PlayerLoop;

namespace HolographicDisplays
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        
        [Description("Enable Server Settings GUI.")]
        public bool ServerSettings { get; internal set; } = false;

        [Description("Interval for updating hologram placeholders in seconds.")]
        public float PlaceholderUpdateInterval { get; set; } = 3f;

        [Description("UTC Time Zone | 2 = UTC+2")]
        public int TimeZone { get; set; } = 2;
    }
}