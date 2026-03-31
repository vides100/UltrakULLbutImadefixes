using HarmonyLib;
using System.IO;
using UltrakULL.audio;
using UltrakULL.json;
using UnityEngine;

using static UltrakULL.CommonFunctions;

namespace UltrakULL.Harmony_Patches.AudioSwaps
{
    [HarmonyPatch(typeof(Radio), "Start")]
    public static class BigJohninatorAudioSwap
    {
        [HarmonyPostfix]
        static void Radio_SwapSongs(Radio __instance)
        {
            if (__instance.songs == null || __instance.songs.Length == 0)
                return;

            // If the voiceover is turned off or English is used, do not touch it
            if (LanguageManager.configFile.Bind("General", "activeDubbing", "False").Value == "False" || isUsingEnglish())
                return;

            string radioFolder = AudioSwapper.SpeechFolder + "BigJohninator" + Path.DirectorySeparatorChar;

            var inst = __instance;
            for (int i = 0; i < inst.songs.Length; i++)
            {
                int ix = i;
                var clip = inst.songs[ix];
                if (clip == null)
                    continue;

                string clipPath = radioFolder + clip.name;
                // Replace if there is a file (async)
                AudioSwapper.SwapClipWithFileAsync(clip, clipPath, (newClip) => { try { inst.songs[ix] = newClip; } catch { } });
            }
        }
    }
}
