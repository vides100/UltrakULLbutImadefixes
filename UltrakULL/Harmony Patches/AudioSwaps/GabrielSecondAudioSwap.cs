using HarmonyLib;
using System.IO;
using UltrakULL.audio;
using UltrakULL.json;
using UnityEngine;

using static UltrakULL.CommonFunctions;

namespace UltrakULL.Harmony_Patches.AudioSwaps
{
    [HarmonyPatch(typeof(GabrielSecond),"Awake")]
    public static class GabrielSecondAudioSwap
    {
        
        [HarmonyPostfix]
        public static void GabrielSecond_VoiceSwap(ref GabrielSecond __instance, ref GabrielVoice ___voice)
        {
            if(LanguageManager.configFile.Bind("General","activeDubbing","False").Value == "False" ||isUsingEnglish())
            {
                return;
            }
            string gabeSecondFolder =  AudioSwapper.SpeechFolder + "gabrielBossSecond" + Path.DirectorySeparatorChar;


            //Taunts
            AudioClip[] gabeSecondTaunts = ___voice.taunt;
            
            string[] tauntLines =
            {
                "gabrielSecondTaunt_IsThisWhatILostTo",
                "gabrielSecondTaunt_YoureGettingRusty",
                "gabrielSecondTaunt_LetsSettleThis",
                "gabrielSecondTaunt_NothingButScrap",
                "gabrielSecondTaunt_IllShowYouDivine",
                "gabrielSecondTaunt_TimeToRight",
                "gabrielSecondTaunt_YouNeedMorePower"
            };
            
            for(int x = 0; x < gabeSecondTaunts.Length; x++)
            {
                int ix = x;
                string gabrielSecondTauntString = gabeSecondFolder + tauntLines[ix];
                AudioSwapper.SwapClipWithFileAsync(gabeSecondTaunts[ix], gabrielSecondTauntString, (clip) => { try { gabeSecondTaunts[ix] = clip; } catch { } });
            }
            
            //Phase change - need to use ref otherwise it gets swapped back to original
            var voice = ___voice;
            AudioClip tmpPhase = voice.phaseChange;
            string gabrielSecondPhaseChangeString = gabeSecondFolder + "gabrielSecondPhaseChange";
            AudioSwapper.SwapClipWithFileAsync(tmpPhase, gabrielSecondPhaseChangeString, (clip) => { try { voice.phaseChange = clip; } catch { } });

            //Big hurt
            AudioClip[] gabeSecondBigHurt = ___voice.bigHurt;
            for(int x = 0; x < gabeSecondBigHurt.Length; x++)
            {
                int ix = x;
                string gabrielSecondBigHurtString = gabeSecondFolder + "gabrielSecondBigHurt" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(gabeSecondBigHurt[ix], gabrielSecondBigHurtString, (clip) => { try { gabeSecondBigHurt[ix] = clip; } catch { } });
            }

            //Hurt
            AudioClip[] gabeSecondHurt = ___voice.hurt;
            for(int x = 0; x < gabeSecondHurt.Length; x++)
            {
                int ix = x;
                string gabrielSecondHurtString = gabeSecondFolder + "gabrielSecondHurt" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(gabeSecondHurt[ix], gabrielSecondHurtString, (clip) => { try { gabeSecondHurt[ix] = clip; } catch { } });
            }
            
            //Taunts second phase
            string[] tauntLinesSecondPhase =
            {
                "gabrielSecondTaunt_IveNeverHadAFight",
                "gabrielSecondTaunt_ShowMeWhat",
                "gabrielSecondTaunt_NowThisIsAFight",
                "gabrielSecondTaunt_WhatIsThisFeeling",
                "gabrielSecondTaunt_ComeGetSomeBlood",
                "gabrielSecondTaunt_ComeOnMachine",
                "gabrielSecondTaunt_IllShowYouTrueSplendor"
            };
            
            AudioClip[] gabeSecondTauntsSecondPhase = ___voice.tauntSecondPhase;
            for(int x = 0; x < gabeSecondTauntsSecondPhase.Length; x++)
            {
                int ix = x;
                string gabeSecondTauntsSecondPhaseString = gabeSecondFolder + tauntLinesSecondPhase[ix];
                AudioSwapper.SwapClipWithFileAsync(gabeSecondTauntsSecondPhase[ix], gabeSecondTauntsSecondPhaseString, (clip) => { try { gabeSecondTauntsSecondPhase[ix] = clip; } catch { } });
            }
        }
    }
}
