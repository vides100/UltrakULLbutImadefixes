using HarmonyLib;
using UltrakULL.audio;
using UnityEngine;
using System.IO;

namespace UltrakULL.Harmony_Patches
{
    [HarmonyPatch]
    public static class GabrielBoatRandomPitchAudioSwap
    {
        // === PATCH 1: Flicker.Start ===
        [HarmonyPatch(typeof(Flicker), "Start")]
        [HarmonyPostfix]
        public static void Flicker_Start_Postfix(Flicker __instance)
        {
            try
            {
                var aud = __instance.GetComponent<AudioSource>();
                if (aud == null || aud.clip == null)
                    return;

                string clipName = aud.clip.name;
                if (clipName != "gab_HologramFiltered")
                    return;
                string localizedPath = Path.Combine(AudioSwapper.SpeechFolder, "gabrielBoat", "gabrielBoat");

                AudioSwapper.SwapClipWithFileAsync(aud.clip, localizedPath, newClip =>
                {
                    if (newClip != null)
                    {
                        aud.clip = newClip;
                        Logging.Message($"[Flicker] Swapped clip '{clipName}' → '{newClip.name}'");
                    }
                });
            }
            catch (System.Exception e)
            {
                Logging.Warn("[Flicker Patch] Swap error: " + e.Message);
            }
        }

        // === PATCH 2: RandomPitch.Start ===
        [HarmonyPatch(typeof(RandomPitch), "Start")]
        [HarmonyPostfix]
        public static void RandomPitch_Start_Postfix(RandomPitch __instance)
        {
            try
            {
                var aud = __instance.GetComponent<AudioSource>();
                if (aud == null || aud.clip == null)
                    return;

                string clipName = aud.clip.name;
                if (clipName != "gab_HologramFiltered")
                    return;
                string localizedPath = Path.Combine(AudioSwapper.SpeechFolder, "gabrielBoat", "gabrielBoat");

                AudioSwapper.SwapClipWithFileAsync(aud.clip, localizedPath, newClip =>
                {
                    if (newClip != null)
                    {
                        aud.clip = newClip;
                        Logging.Message($"[RandomPitch] Swapped clip '{clipName}' → '{newClip.name}'");
                    }
                });
            }
            catch (System.Exception e)
            {
                Logging.Warn("[RandomPitch Patch] Swap error: " + e.Message);
            }
        }

        // === PATCH 3: RandomPitch.OnEnable ===
        [HarmonyPatch(typeof(RandomPitch), "OnEnable")]
        [HarmonyPostfix]
        public static void RandomPitch_OnEnable_Postfix(RandomPitch __instance)
        {
            try
            {
                var aud = __instance.GetComponent<AudioSource>();
                if (aud == null || aud.clip == null)
                    return;

                string clipName = aud.clip.name;
                if (clipName != "gab_HologramFiltered")
                    return;
                string localizedPath = Path.Combine(AudioSwapper.SpeechFolder, "gabrielBoat", "gabrielBoat");

                AudioSwapper.SwapClipWithFileAsync(aud.clip, localizedPath, newClip =>
                {
                    if (newClip != null)
                    {
                        aud.clip = newClip;
                        Logging.Message($"[RandomPitch Enable] Swapped clip '{clipName}' → '{newClip.name}'");
                    }
                });
            }
            catch (System.Exception e)
            {
                Logging.Warn("[RandomPitch OnEnable] Swap error: " + e.Message);
            }
        }
    }
}
