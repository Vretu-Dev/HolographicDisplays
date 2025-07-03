using Exiled.API.Features;
using System;

namespace HolographicDisplays
{
    public class HolographicDisplays : Plugin<Config>
    {
        public override string Author => "Vretu";
        public override string Name => "HolographicDisplays";
        public override string Prefix => "HD";
        public override Version Version => new Version(1, 3, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 0);
        public static HolographicDisplays Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Placeholders.RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            HologramRotationUpdater.Stop();
            HologramManager.DestroyAll();
            Placeholders.UnregisterEvents();
            base.OnDisabled();
        }

        private void OnRoundStarted()
        {
            HologramManager.Load();
            HologramRotationUpdater.Start();
        }

        private void OnWaitingForPlayers()
        {
            HologramManager.DestroyAll();
            HologramRotationUpdater.Stop();
        }
    }
}