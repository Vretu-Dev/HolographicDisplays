using Exiled.Events.EventArgs.Player;
using HolographicDisplays.Holograms;
using PlayerRoles;

namespace HolographicDisplays.Placeholders
{
    public static class Updater
    {
        public static void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.Escaped += OnPlayerEscaped;
            Exiled.Events.Handlers.Player.Verified += OnVerifiedPlayer;
            Exiled.Events.Handlers.Player.Left += OnLeftPlayer;
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
        }

        public static void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Escaped -= OnPlayerEscaped;
            Exiled.Events.Handlers.Player.Verified -= OnVerifiedPlayer;
            Exiled.Events.Handlers.Player.Left -= OnLeftPlayer;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
        }

        public static void RefreshPlaceholders(params string[] placeholders)
        {
            foreach (var holo in Manager.Holograms)
            {
                if (holo.Toy == null) continue;

                foreach (var ph in placeholders)
                {
                    if (Placeholders.Functions.ContainsKey(ph) && holo.Content.Contains(ph))
                    {
                        holo.Toy.TextFormat = Placeholders.Replace(holo.Content);
                        break;
                    }
                }
            }
        }

        private static void OnPlayerEscaped(EscapedEventArgs ev)
        {
            RefreshPlaceholders("{total_escaped}", "{classd_escaped}", "{scientist_escaped}");
        }

        private static void OnVerifiedPlayer(VerifiedEventArgs ev)
        {
            RefreshPlaceholders("{players}");
        }

        private static void OnLeftPlayer(LeftEventArgs ev)
        {
            RefreshPlaceholders("{players}");
        }

        private static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleTypeId.Spectator)
                RefreshPlaceholders("{alive_players}");
        }
    }
}
