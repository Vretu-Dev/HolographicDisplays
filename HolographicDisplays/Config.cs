using Exiled.API.Interfaces;
using System.ComponentModel;
using UnityEngine.PlayerLoop;

namespace HolographicDisplays
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Enable PlaceholderAPI support (if installed).")]
        public bool PlaceholderApi { get; internal set; } = false;
        
        [Description("Enable Server Settings GUI.")]
        public bool ServerSettings { get; internal set; } = true;

        [Description("Interval for updating hologram placeholders in seconds.")]
        public float PlaceholderUpdateInterval { get; set; } = 2f;
        
        [Description("Interval for updating hologram rotation (player-facing) in milliseconds.")]
        public int RotationUpdateInterval { get; set; } = 10;

        [Description("UTC Time Zone | 2 = UTC+2")]
        public int TimeZone { get; set; } = 2;
    }
}