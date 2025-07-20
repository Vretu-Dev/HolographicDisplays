using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace HolographicDisplays.Placeholders
{
    public static class Events
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
    }
}
