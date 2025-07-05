using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HolographicDisplays
{
    public static class Placeholders
    {
        public static int TotalEscaped { get; set; }
        public static int ClassDEscaped { get; set; }
        public static int ScientistEscaped { get; set; }

        public static void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.Escaping += OnPlayerEscaped;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        }
        public static void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Escaping -= OnPlayerEscaped;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private static void OnPlayerEscaped(EscapingEventArgs ev)
        {
            if (ev.Player == null || !ev.IsAllowed)
                return;

            TotalEscaped++;

            if (ev.Player.Role.Type == RoleTypeId.ClassD)
                ClassDEscaped++;
            else if (ev.Player.Role.Type == RoleTypeId.Scientist)
                ScientistEscaped++;
        }

        private static void OnWaitingForPlayers()
        {
            TotalEscaped = 0;
            ClassDEscaped = 0;
            ScientistEscaped = 0;
        }

        public static string Replace(string text)
        {
            text = text.Replace("{server_name}", Server.Name)
                       .Replace("{players}", Server.PlayerCount.ToString())
                       .Replace("{max_players}", Server.MaxPlayerCount.ToString())
                       .Replace("{server_tps}", Server.Tps.ToString())
                       .Replace("{server_maxtps}", Server.MaxTps.ToString())
                       .Replace("{round_time}", GetRoundTime())
                       .Replace("{time}", GetTime())
                       .Replace("{total_escaped}", TotalEscaped.ToString())
                       .Replace("{classd_escaped}", ClassDEscaped.ToString())
                       .Replace("{scientist_escaped}", ScientistEscaped.ToString())
                       .Replace("{players_alive}", GetAlivePlayers().ToString())
                       .Replace("{warhead_status}", GetWarheadStatus());

            text = GetCountRoleType(text);

            if (HolographicDisplays.Instance.Config.PlaceholderApi)
                text = PlaceholderAPISupport(text);

            return text;
        }

        private static string GetRoundTime()
        {
            TimeSpan elapsed = Round.ElapsedTime;
            return ((int)elapsed.TotalMinutes).ToString();
        }

        private static string GetTime()
        {
            DateTime utc = DateTime.UtcNow.AddHours(HolographicDisplays.Instance.Config.TimeZone);
            return utc.ToString("HH:mm");
        }

        private static int GetAlivePlayers()
        {
            return Player.List.Count(p => p.IsAlive);
        }

        private static string GetWarheadStatus()
        {
            var name = HolographicDisplays.Instance.Translation.GetWarheadStatusName(Warhead.Status);
            var color = HolographicDisplays.Instance.Translation.GetWarheadStatusColor(Warhead.Status);
            return $"<color={color}>{name}</color>";
        }

        private static string GetCountRoleType(string text)
        {
            return Regex.Replace(text, @"\{([a-zA-Z0-9_\-]+)\}", m =>
            {
                string roleName = m.Groups[1].Value;
                if (Enum.TryParse<RoleTypeId>(roleName, true, out var role))
                {
                    int count = Player.List.Count(p => p.Role.Type == role);
                    return count.ToString();
                }
                return m.Value;
            });
        }

        private static string PlaceholderAPISupport(string text)
        {
            try
            {
                return PlaceholderAPI.API.PlaceholderAPI.SetPlaceholders(text);
            }
            catch (Exception ex)
            {
                Log.Warn($"PlaceholderAPI exception: {ex}");
            }

            return text;
        }
    }
}