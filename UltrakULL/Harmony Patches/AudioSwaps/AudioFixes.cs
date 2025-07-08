using System;
using HarmonyLib;
using UltrakULL.audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UltrakULL.CommonFunctions;

namespace UltrakULL.Harmony_Patches.AudioSwaps
{
    //Fix for audio being unswapped when respawning. Needs testing.
    [HarmonyPatch(typeof(NewMovement), "Respawn")]
    public class RespawnAudioFixer
    {
        [HarmonyPostfix]
        public static async void Respawn_SwapperFix()
        {
            if (isUsingEnglish()) return;

            await System.Threading.Tasks.Task.Delay(500);
            SubtitledAudioSourcesReplacer.ReplaceSubsAndAudio();

            // A second attempt after 0.5 seconds, when everything is already activated
            DelayedSubRecheck();
        }

        private static async void DelayedSubRecheck()
        {
            await System.Threading.Tasks.Task.Delay(500);
            SubtitledAudioSourcesReplacer.ReplaceSubsAndAudio();
        }
    }
}