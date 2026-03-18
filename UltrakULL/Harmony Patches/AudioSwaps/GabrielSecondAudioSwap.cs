using System;
using System.IO;
using HarmonyLib;
using UltrakULL.audio;
using UltrakULL.json;
using UnityEngine;

using static UltrakULL.CommonFunctions;

namespace UltrakULL.Harmony_Patches.AudioSwaps
{
    [HarmonyPatch(typeof(GabrielSecond), "Awake")]
    public static class GabrielSecondAudioSwap
    {
        [HarmonyPostfix]
        public static void GabrielSecond_VoiceSwap(ref GabrielSecond __instance)
        {
            if (LanguageManager.configFile.Bind("General", "activeDubbing", "False").Value == "False" || isUsingEnglish())
            {
                return;
            }
            GabrielVoice voice = __instance.GetComponent<GabrielVoice>();
            if (voice == null)
            {
                Debug.LogWarning("[UltrakULL] GabrielVoice component not found on GabrielSecond!");
                return;
            }
            string gabeSecondFolder = AudioSwapper.SpeechFolder + "gabrielBossSecond" + Path.DirectorySeparatorChar;
            
            // Taunts
            AudioClip[] gabeSecondTaunts = voice.taunt;
            string[] tauntLines = new string[]
            {
                "gabrielSecondTaunt_IsThisWhatILostTo",
                "gabrielSecondTaunt_YoureGettingRusty",
                "gabrielSecondTaunt_LetsSettleThis",
                "gabrielSecondTaunt_NothingButScrap",
                "gabrielSecondTaunt_IllShowYouDivine",
                "gabrielSecondTaunt_TimeToRight",
                "gabrielSecondTaunt_YouNeedMorePower"
            };
            for (int i = 0; i < gabeSecondTaunts.Length; i++)
            {
                int ix = i;
                string gabrielSecondTauntString = gabeSecondFolder + tauntLines[ix];
                AudioSwapper.SwapClipWithFileAsync(gabeSecondTaunts[ix], gabrielSecondTauntString, (clip) =>
                {
                    try { gabeSecondTaunts[ix] = clip; }
                    catch { }
                });
            }
            
            // Phase change
            AudioClip tmpPhase = voice.phaseChange;
            string gabrielSecondPhaseChangeString = gabeSecondFolder + "gabrielSecondPhaseChange";
            AudioSwapper.SwapClipWithFileAsync(tmpPhase, gabrielSecondPhaseChangeString, (clip) =>
            {
                try { voice.phaseChange = clip; }
                catch { }
            });
            
            // Big hurt
            AudioClip[] gabeSecondBigHurt = voice.bigHurt;
            for (int i = 0; i < gabeSecondBigHurt.Length; i++)
            {
                int ix = i;
                string gabrielSecondBigHurtString = gabeSecondFolder + "gabrielSecondBigHurt" + (ix + 1).ToString();
                AudioSwapper.SwapClipWithFileAsync(gabeSecondBigHurt[ix], gabrielSecondBigHurtString, (clip) =>
                {
                    try { gabeSecondBigHurt[ix] = clip; }
                    catch { }
                });
            }
            
            // Hurt
            AudioClip[] gabeSecondHurt = voice.hurt;
            for (int i = 0; i < gabeSecondHurt.Length; i++)
            {
                int ix = i;
                string gabrielSecondHurtString = gabeSecondFolder + "gabrielSecondHurt" + (ix + 1).ToString();
                AudioSwapper.SwapClipWithFileAsync(gabeSecondHurt[ix], gabrielSecondHurtString, (clip) =>
                {
                    try { gabeSecondHurt[ix] = clip; }
                    catch { }
                });
            }
            
            // Taunts second phase
            string[] tauntLinesSecondPhase = new string[]
            {
                "gabrielSecondTaunt_IveNeverHadAFight",
                "gabrielSecondTaunt_ShowMeWhat",
                "gabrielSecondTaunt_NowThisIsAFight",
                "gabrielSecondTaunt_WhatIsThisFeeling",
                "gabrielSecondTaunt_ComeGetSomeBlood",
                "gabrielSecondTaunt_ComeOnMachine",
                "gabrielSecondTaunt_IllShowYouTrueSplendor"
            };
            AudioClip[] gabeSecondTauntsSecondPhase = voice.tauntSecondPhase;
            for (int i = 0; i < gabeSecondTauntsSecondPhase.Length; i++)
            {
                int ix = i;
                string gabeSecondTauntsSecondPhaseString = gabeSecondFolder + tauntLinesSecondPhase[ix];
                AudioSwapper.SwapClipWithFileAsync(gabeSecondTauntsSecondPhase[ix], gabeSecondTauntsSecondPhaseString, (clip) =>
                {
                    try { gabeSecondTauntsSecondPhase[ix] = clip; }
                    catch { }
                });
            }
        }
    }
}
