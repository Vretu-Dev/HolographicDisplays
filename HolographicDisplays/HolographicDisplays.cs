﻿using Exiled.API.Features;
using System;

namespace HolographicDisplays
{
    public class HolographicDisplays : Plugin<Config, Translations>
    {
        public override string Author => "Vretu";
        public override string Name => "HolographicDisplays";
        public override string Prefix => "HD";
        public override Version Version => new Version(1, 4, 1);
        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 0);
        public static HolographicDisplays Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Placeholders.RegisterEvents();
            if (Config.ServerSettings)
                ServerSettings.RegisterSettings();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            HologramUpdater.Stop();
            HologramManager.DestroyAll();
            Placeholders.UnregisterEvents();
            if (Config.ServerSettings)
                ServerSettings.UnregisterSettings();
            base.OnDisabled();
        }

        private void OnRoundStarted()
        {
            HologramManager.Load();
            HologramUpdater.Start();
        }

        private void OnWaitingForPlayers()
        {
            HologramManager.DestroyAll();
            HologramUpdater.Stop();
        }
    }
}