using Exiled.API.Features;
using System;

namespace HolographicDisplays
{
    public static class Placeholders
    {
        public static string Replace(string text)
        {
            text = text.Replace("%server_name%", Server.Name)
                       .Replace("%players%", Server.PlayerCount.ToString())
                       .Replace("%max_players%", Server.MaxPlayerCount.ToString())
                       .Replace("%server_tps%", Server.Tps.ToString())
                       .Replace("%server_maxtps%", Server.MaxTps.ToString())
                       .Replace("%round_time%", GetRoundTime());

            return text;
        }

        private static string GetRoundTime()
        {
            TimeSpan elapsed = Round.ElapsedTime;
            string elapsedFormatted = elapsed.ToString(@"mm\:ss");
            return elapsedFormatted;
        }
    }
}