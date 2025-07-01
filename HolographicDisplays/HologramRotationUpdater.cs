using MEC;
using System.Collections.Generic;
using System.Linq;

namespace HolographicDisplays
{
    public static class HologramRotationUpdater
    {
        private static CoroutineHandle _handle;
        private static float _fastUpdateInterval = 0.5f;
        private static float _slowUpdateInterval => HolographicDisplays.Instance.Config.PlaceholderUpdateInterval;
        private static float _fastUpdateTimer = 0f;
        private static float _slowUpdateTimer = 0f;

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
            string[] slow_placeholders = { "{players}", "{server_tps}", "{time}" };

            while (true)
            {
                foreach (var holo in HologramManager.Holograms)
                {
                    holo.SyncRotationPerPlayer();
                }

                _fastUpdateTimer += 0.05f;
                _slowUpdateTimer += 0.05f;

                if (_fastUpdateTimer >= _fastUpdateInterval)
                {
                    foreach (var holo in HologramManager.Holograms)
                    {
                        if (holo.Toy != null && fast_placeholders.Any(ph => holo.Content.Contains(ph)))
                            holo.Toy.TextFormat = Placeholders.Replace(holo.Content);
                    }
                    _fastUpdateTimer = 0f;
                }

                if (_slowUpdateTimer >= _slowUpdateInterval)
                {
                    foreach (var holo in HologramManager.Holograms)
                    {
                        if (holo.Toy != null && slow_placeholders.Any(ph => holo.Content.Contains(ph)))
                            holo.Toy.TextFormat = Placeholders.Replace(holo.Content);
                    }
                    _slowUpdateTimer = 0f;
                }

                yield return Timing.WaitForSeconds(0.05f);
            }
        }
    }
}