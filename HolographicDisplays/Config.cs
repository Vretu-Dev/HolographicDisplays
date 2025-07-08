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

        [Description("Is the animation for {Rainbow:} is to be active if false is a static color.")]
        public bool RainbowAnimation { get; set; } = true;

        [Description("Interval for updating hologram with Rainbow Animation in milliseconds.")]
        public int RainbowUpdateInterval { get; set; } = 25;
        
        [Description("Speed of the rainbow animation.")]
        public float AnimationSpeed { get; set; } = 100f;
        [Description("Custom rainbow palette. Comma separated hex colors (e.g. #FF0000,#00FF00,#0000FF). If empty, HSV rainbow will be used.")]
        public string RainbowPalette { get; set; } = "";

        [Description("UTC Time Zone | 2 = UTC+2")]
        public int TimeZone { get; set; } = 2;
    }
}