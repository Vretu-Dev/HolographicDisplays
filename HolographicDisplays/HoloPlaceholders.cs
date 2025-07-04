using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;

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

        public static string Replace(string text, Player player = null)
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
                       .Replace("{scientist_escaped}", ScientistEscaped.ToString());

            if (HolographicDisplays.Instance.Config.PlaceholderApi)
            {
                try
                {
                    text = PlaceholderAPI.API.PlaceholderAPI.SetPlaceholders(player, text);
                }
                catch (Exception ex)
                {
                    if (HolographicDisplays.Instance.Config.Debug)
                        Log.Warn($"PlaceholderAPI exception: {ex}");
                }
            }
            return text;
        }

        private static string GetRoundTime()
        {
            TimeSpan elapsed = Round.ElapsedTime;
            string elapsedFormatted = elapsed.ToString(@"mm\:ss");
            return elapsedFormatted;
        }

        private static string GetTime()
        {
            DateTime utc = DateTime.UtcNow.AddHours(HolographicDisplays.Instance.Config.TimeZone);
            return utc.ToString("HH:mm");
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
    }
}