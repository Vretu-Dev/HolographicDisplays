using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HolographicDisplays
{
    public static class HologramRotationUpdater
    {
        private static CoroutineHandle _handle;
        private static float _rotationUpdateTimer = 0f;
        private static float _fastUpdateTimer = 0f;
        private static float _slowUpdateTimer = 0f;

        private static readonly float FastUpdateInterval = 0.5f;
        private static float SlowUpdateInterval => HolographicDisplays.Instance.Config.PlaceholderUpdateInterval;
        private static float YieldStep => HolographicDisplays.Instance.Config.RotationUpdateInterval / 1000f;

        public static void Start()
        {
            if (_handle.IsRunning) return;
            _handle = Timing.RunCoroutine(RotationLoop());
        }

        public static void Stop()
        {
            if (_handle.IsRunning)
                Timing.KillCoroutines(_handle);
        }

        private static IEnumerator<float> RotationLoop()
        {
            string[] fast_placeholders = { "{round_time}" };
            string[] slow_placeholders = { "{players}", "{server_tps}", "{time}", "{total_escaped}", "{classd_escaped}, {scientist_escaped}" };
            Regex placeholderApiRegex = new Regex("%[^%]+%");

            while (true)
            {
                _rotationUpdateTimer += YieldStep;
                _fastUpdateTimer += YieldStep;
                _slowUpdateTimer += YieldStep;

                if (_rotationUpdateTimer >= YieldStep)
                {
                    foreach (var holo in HologramManager.Holograms)
                    {
                        holo.SyncRotationPerPlayer();
                    }
                    _rotationUpdateTimer = 0f;
                }

                if (_fastUpdateTimer >= FastUpdateInterval)
                {
                    foreach (var holo in HologramManager.Holograms)
                    {
                        if (holo.Toy != null && fast_placeholders.Any(ph => holo.Content.Contains(ph)))
                            holo.Toy.TextFormat = Placeholders.Replace(holo.Content);
                    }
                    _fastUpdateTimer = 0f;
                }

                if (_slowUpdateTimer >= SlowUpdateInterval)
                {
                    foreach (var holo in HologramManager.Holograms)
                    {
                        if (placeholderApiRegex.IsMatch(holo.Content))
                            holo.SyncTextPerPlayer();

                        else if (holo.Toy != null && slow_placeholders.Any(ph => holo.Content.Contains(ph)))
                            holo.Toy.TextFormat = Placeholders.Replace(holo.Content);

                    }
                    _slowUpdateTimer = 0f;
                }

                yield return Timing.WaitForSeconds(YieldStep);
            }
        }
    }
}