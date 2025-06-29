using MEC;
using System.Collections.Generic;
using System.Linq;

namespace HolographicDisplays
{
    public static class HologramRotationUpdater
    {
        private static CoroutineHandle _handle;
        private static float _textUpdateInterval = 0.5f;
        private static float _textUpdateTimer = 0f;

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
            string[] placeholders = { "%players%", "%server_tps%", "%round_time%" };

            while (true)
            {
                foreach (var holo in HologramManager.Holograms)
                {
                    holo.SyncRotationToNearest();
                }

                _textUpdateTimer += 0.01f;
                if (_textUpdateTimer >= _textUpdateInterval)
                {
                    foreach (var holo in HologramManager.Holograms)
                    {
                        if (holo.Toy != null && placeholders.Any(ph => holo.Content.Contains(ph)))
                            holo.Toy.TextFormat = Placeholders.Replace(holo.Content);
                    }
                    _textUpdateTimer = 0f;
                }
                yield return Timing.WaitForSeconds(0.01f);
            }
        }
    }
}