﻿using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HolographicDisplays
{
    public static class HologramUpdater
    {
        private static CoroutineHandle _handle;
        private static float _rotationUpdateTimer = 0f;
        private static float _placeholderUpdateTimer = 0f;
        private static float _rainbowUpdateTimer = 0f;
        public static float AnimationTick = 0f;

        private static float PlaceholderUpdateInterval => HolographicDisplays.Instance.Config.PlaceholderUpdateInterval;
        private static float YieldStep => HolographicDisplays.Instance.Config.RotationUpdateInterval / 1000f;
        private static float RainbowUpdateInterval => HolographicDisplays.Instance.Config.RainbowUpdateInterval / 1000f;

        private static readonly Regex PlaceholderRegex = new Regex(@"\{[a-zA-Z0-9_\-]+\}", RegexOptions.Compiled);

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
                _rotationUpdateTimer += YieldStep;
                _placeholderUpdateTimer += YieldStep;
                
                if (_rotationUpdateTimer >= YieldStep)
                {
                    foreach (var holo in HologramManager.Holograms)
                    {
                        holo.SyncRotationPerPlayer();
                    }
                    _rotationUpdateTimer = 0f;
                }

                if (_placeholderUpdateTimer >= PlaceholderUpdateInterval)
                {
                    foreach (var holo in HologramManager.Holograms)
                    {
                        if (holo.Toy != null && PlaceholderRegex.IsMatch(holo.Content))
                            holo.Toy.TextFormat = Placeholders.Replace(holo.Content);
                    }
                    _placeholderUpdateTimer = 0f;
                }

                if (HolographicDisplays.Instance.Config.RainbowAnimation)
                {
                    _rainbowUpdateTimer += YieldStep;

                    if (_rainbowUpdateTimer >= RainbowUpdateInterval)
                    {
                        var rainbowHolograms = HologramManager.Holograms.Where(h => h.Toy != null && h.Content.Contains("{Rainbow:"));

                        foreach (var holo in rainbowHolograms)
                        {
                            holo.Toy.TextFormat = Placeholders.Replace(holo.Content);
                        }
                        AnimationTick += RainbowUpdateInterval;

                        _rainbowUpdateTimer = 0f;
                    }
                }

                yield return Timing.WaitForSeconds(YieldStep);
            }
        }
    }
}