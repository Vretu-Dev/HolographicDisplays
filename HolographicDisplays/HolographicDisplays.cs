using System;
using LabApi.Features;
using LabApi.Loader;
using Loader = LabApi.Loader.Features.Plugins.Plugin;

namespace HolographicDisplays
{
    public class HolographicDisplays : Loader
    {
        public override string Author => "Vretu";
        public override string Name => "HolographicDisplays";
        public override string Description => "Displayed Holograms.";
        public override Version Version => new Version(1, 4, 1);
        public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);
        public static HolographicDisplays Instance { get; private set; }
        public Translations Translation { get; private set; }
        public Config Config { get; private set; }

        public override void Enable()
        {
            Instance = this;
            LabApi.Events.Handlers.ServerEvents.RoundStarted += OnRoundStarted;
            LabApi.Events.Handlers.ServerEvents.WaitingForPlayers += OnWaitingForPlayers;
            Placeholders.RegisterEvents();
        }

        public override void Disable()
        {
            Instance = null;
            LabApi.Events.Handlers.ServerEvents.RoundStarted -= OnRoundStarted;
            LabApi.Events.Handlers.ServerEvents.WaitingForPlayers -= OnWaitingForPlayers;
            HologramUpdater.Stop();
            HologramManager.DestroyAll();
            Placeholders.UnregisterEvents();
        }

        public override void LoadConfigs()
        {
            this.TryLoadConfig("config.yml", out Config config);
            Config = config ?? new Config();
            this.TryLoadConfig("translation.yml", out Translations translation);
            Translation = translation ?? new Translations();
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