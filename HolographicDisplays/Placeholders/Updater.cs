using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Warhead;
using HolographicDisplays.Holograms;
using MEC;
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
            Exiled.Events.Handlers.Warhead.Detonated += OnWarheadDetonation;
            Exiled.Events.Handlers.Warhead.Starting += OnWarheadStarting;
            Exiled.Events.Handlers.Warhead.Stopping += OnWarheadStopping;
        }

        public static void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Escaped -= OnPlayerEscaped;
            Exiled.Events.Handlers.Player.Verified -= OnVerifiedPlayer;
            Exiled.Events.Handlers.Player.Left -= OnLeftPlayer;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Warhead.Detonated -= OnWarheadDetonation;
            Exiled.Events.Handlers.Warhead.Starting -= OnWarheadStarting;
            Exiled.Events.Handlers.Warhead.Stopping -= OnWarheadStopping;
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

        private static void OnPlayerEscaped(EscapedEventArgs ev) => RefreshPlaceholders("{total_escaped}", "{classd_escaped}", "{scientist_escaped}");

        private static void OnVerifiedPlayer(VerifiedEventArgs ev) => RefreshPlaceholders("{players}");

        private static void OnLeftPlayer(LeftEventArgs ev) => RefreshPlaceholders("{players}");

        private static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleTypeId.Spectator || ev.NewRole == RoleTypeId.None)
                RefreshPlaceholders("{alive_players}");
        }

        private static void OnWarheadStarting(StartingEventArgs ev) => Timing.CallDelayed(0.1f, () => RefreshPlaceholders("{warhead_status}"));
        
        private static void OnWarheadStopping(StoppingEventArgs ev) => Timing.CallDelayed(0.1f, () => RefreshPlaceholders("{warhead_status}"));

        private static void OnWarheadDetonation() => RefreshPlaceholders("{warhead_status}");
    }
}
