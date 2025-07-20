using Exiled.API.Features;
using HolographicDisplays.Extensions;
using HolographicDisplays.Holograms;
using System;

namespace HolographicDisplays
{
    public class Main : Plugin<Config, Translations>
    {
        public override string Author => "Vretu";
        public override string Name => "HolographicDisplays";
        public override string Prefix => "HD";
        public override Version Version => new Version(1, 4, 1);
        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 0);
        public static Main Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Placeholders.Events.RegisterEvents();
            Placeholders.Updater.RegisterEvents();
            if (Config.ServerSettings)
                ServerSettings.RegisterSettings();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Updater.Stop();
            Manager.DestroyAll();
            Placeholders.Events.UnregisterEvents();
            Placeholders.Updater.UnregisterEvents();
            if (Config.ServerSettings)
                ServerSettings.UnregisterSettings();
            base.OnDisabled();
        }

        private void OnRoundStarted()
        {
            Manager.Load();
            Updater.Start();
        }

        private void OnWaitingForPlayers()
        {
            Manager.DestroyAll();
            Updater.Stop();
        }
    }
}