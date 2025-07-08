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

            text = Regex.Replace(text, @"\{([a-zA-Z0-9_\-]+|add((?:,\{[a-zA-Z0-9_]+\})+))\}", m => HandleRolePlaceholder(m.Groups[1].Value));

            text = Regex.Replace(text, @"\{Rainbow:([^\}]+)\}", m =>
            {
                string rainbowText = m.Groups[1].Value;
                float tick = HologramUpdater.AnimationTick;
                return AnimatedRainbow(rainbowText, tick);
            });

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

        private static string HandleRolePlaceholder(string value)
        {
            if (value.StartsWith("add"))
            {
                var roles = Regex.Matches(value, @"\{([A-Za-z0-9_]+)\}")
                    .Cast<Match>()
                    .Select(x => x.Groups[1].Value)
                    .ToList();

                int count = 0;
                foreach (var roleName in roles)
                {
                    if (Enum.TryParse<RoleTypeId>(roleName, true, out var role))
                        count += Player.List.Count(p => p.Role.Type == role);
                }
                return count.ToString();
            }
            else if (Enum.TryParse<RoleTypeId>(value, true, out var singleRole))
            {
                return Player.List.Count(p => p.Role.Type == singleRole).ToString();
            }
            return "0";
        }


        private static string AnimatedRainbow(string input, float tick)
        {
            string[] palette = null;
            var paletteStr = HolographicDisplays.Instance.Config.RainbowPalette;
            if (!string.IsNullOrWhiteSpace(paletteStr))
                palette = paletteStr.Split(',').Select(s => s.Trim()).Where(s => s.Length >= 7 && s.StartsWith("#")).ToArray();

            var result = "";
            float speed = HolographicDisplays.Instance.Config.AnimationSpeed;
            float timeShift = (tick * speed) % 360f;

            for (int i = 0; i < input.Length; i++)
            {
                string color;
                if (palette != null && palette.Length > 0)
                {
                    int paletteIndex = (i + (int)(timeShift / (360f / palette.Length))) % palette.Length;
                    color = palette[paletteIndex];
                }
                else
                {
                    float hue = ((360f / input.Length) * i + timeShift) % 360f;
                    color = HSVToHex(hue);
                }

                result += $"<color={color}>{input[i]}</color>";
            }
            return result;
        }

        private static string HSVToHex(float h, float s = 1f, float v = 1f)
        {
            int hi = (int)(h / 60f) % 6;
            float f = h / 60f - hi;
            v = v * 255f;
            int vi = (int)v;
            int p = (int)(v * (1 - s));
            int q = (int)(v * (1 - f * s));
            int t = (int)(v * (1 - (1 - f) * s));

            int r = 0, g = 0, b = 0;
            switch (hi)
            {
                case 0: r = vi; g = t; b = p; break;
                case 1: r = q; g = vi; b = p; break;
                case 2: r = p; g = vi; b = t; break;
                case 3: r = p; g = q; b = vi; break;
                case 4: r = t; g = p; b = vi; break;
                case 5: r = vi; g = p; b = q; break;
            }
            return $"#{r:X2}{g:X2}{b:X2}";
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