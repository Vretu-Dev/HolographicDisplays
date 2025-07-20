using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HolographicDisplays.Placeholders
{
    public static class Placeholders
    {
        public static readonly HashSet<string> Supported = new()
        {
            "{server_name}",
            "{players}",
            "{max_players}",
            "{server_tps}",
            "{server_maxtps}",
            "{round_time}",
            "{time}",
            "{total_escaped}",
            "{classd_escaped}",
            "{scientist_escaped}",
            "{players_alive}",
            "{warhead_status}"
        };

        public static readonly Dictionary<string, Func<string>> Functions = new()
        {
            ["{server_name}"] = () => Server.Name,
            ["{players}"] = () => Server.PlayerCount.ToString(),
            ["{max_players}"] = () => Server.MaxPlayerCount.ToString(),
            ["{server_tps}"] = () => Server.Tps.ToString(),
            ["{server_maxtps}"] = () => Server.MaxTps.ToString(),
            ["{round_time}"] = () => GetRoundTime(),
            ["{time}"] = () => GetTime(),
            ["{total_escaped}"] = () => Events.TotalEscaped.ToString(),
            ["{classd_escaped}"] = () => Events.ClassDEscaped.ToString(),
            ["{scientist_escaped}"] = () => Events.ScientistEscaped.ToString(),
            ["{players_alive}"] = () => GetAlivePlayers().ToString(),
            ["{warhead_status}"] = () => GetWarheadStatus()
        };

        public static string Replace(string text)
        {
            foreach (var key in Supported)
                text = text.Replace(key, Functions[key]());

            return text;
        }

        private static string GetRoundTime()
        {
            TimeSpan elapsed = Round.ElapsedTime;
            return ((int)elapsed.TotalMinutes).ToString();
        }

        private static string GetTime()
        {
            DateTime utc = DateTime.UtcNow.AddHours(Main.Instance.Config.TimeZone);
            return utc.ToString("HH:mm");
        }

        private static int GetAlivePlayers()
        {
            return Player.List.Count(p => p.IsAlive);
        }

        private static string GetWarheadStatus()
        {
            var name = Main.Instance.Translation.GetWarheadStatusName(Warhead.Status);
            var color = Main.Instance.Translation.GetWarheadStatusColor(Warhead.Status);
            return $"<color={color}>{name}</color>";
        }
    }
}