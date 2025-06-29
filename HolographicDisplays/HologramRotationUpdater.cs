using MEC;
using System.Collections.Generic;

namespace HolographicDisplays
{
    public static class HologramRotationUpdater
    {
        private static CoroutineHandle _handle;

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
            while (true)
            {
                foreach (var holo in HologramManager.Holograms)
                {
                    holo.SyncRotationToNearest();
                }
                yield return Timing.WaitForSeconds(0.01f);
            }
        }
    }
}