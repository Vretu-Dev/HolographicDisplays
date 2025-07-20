using MEC;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HolographicDisplays.Holograms
{
    public static class Updater
    {
        private static CoroutineHandle _rotation;
        private static CoroutineHandle _placeholder;
        private static float PlaceholderUpdateInterval => Main.Instance.Config.PlaceholderUpdateInterval;

        public static void Start()
        {
            if (_rotation.IsRunning || _placeholder.IsRunning)
                return;

            _rotation = Timing.RunCoroutine(RotationLoop());
            _placeholder = Timing.RunCoroutine(PlaceholderLoop());
        }

        public static void Stop()
        {
            if (_rotation.IsRunning)
                Timing.KillCoroutines(_rotation);

            if (_placeholder.IsRunning)
                Timing.KillCoroutines(_rotation);
        }

        private static IEnumerator<float> RotationLoop()
        {
            while (true)
            {

                foreach (var holo in Manager.Holograms)
                    holo.SyncRotationPerPlayer();

                yield return Timing.WaitForOneFrame;
            }
        }

        private static IEnumerator<float> PlaceholderLoop()
        {
            while (true)
            {
                Placeholders.Updater.RefreshPlaceholders("{server_tps}", "{round_time}", "{time}");

                yield return Timing.WaitForSeconds(PlaceholderUpdateInterval);
            }
        }
    }
}