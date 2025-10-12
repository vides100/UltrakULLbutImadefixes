    using HarmonyLib;
using UltrakULL.audio;
using UltrakULL.json;
using UnityEngine;

using static UltrakULL.CommonFunctions;

namespace UltrakULL.Harmony_Patches.AudioSwaps
{
    [HarmonyPatch(typeof(SisyphusPrime),"Start")]
    public class SisyphusPrimeAudioSwap
    {
        [HarmonyPostfix]
        public static void SisyphusPrimeAudioSwapPatch(ref SisyphusPrime __instance)
        {
            if(LanguageManager.configFile.Bind("General","activeDubbing","False").Value == "False" || isUsingEnglish())
            {
                return;
            }

            string sisyphusPrimeFolder =  AudioSwapper.SpeechFolder + "sisyphusPrime\\";
            
            AudioClip[] begoneAttacks  = __instance.clapVoice;
            for(int x = 0; x < begoneAttacks.Length; x++)
            {
                int ix = x; // capture
                string minosPrimeKickString = sisyphusPrimeFolder + "sisyphusBegone" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(begoneAttacks[ix], minosPrimeKickString, (clip) => { try { begoneAttacks[ix] = clip; } catch { } });
            }
            
            AudioClip[] thisWillHurtAttack = __instance.explosionVoice;
            for(int x = 0; x < thisWillHurtAttack.Length; x++)
            {
                int ix = x;
                string thisWillHurtString = sisyphusPrimeFolder + "sisyphusThisWillHurt";
                AudioSwapper.SwapClipWithFileAsync(thisWillHurtAttack[ix], thisWillHurtString, (clip) => { try { thisWillHurtAttack[ix] = clip; } catch { } });
            }
            
            AudioClip[] grunt = __instance.hurtVoice;
            for(int x = 0; x < grunt.Length; x++)
            {
                int ix = x;
                string gruntString = sisyphusPrimeFolder + "sisyphusGrunt";
                AudioSwapper.SwapClipWithFileAsync(grunt[ix], gruntString, (clip) => { try { grunt[ix] = clip; } catch { } });
            }
            
            AudioClip[] stompAttacks = __instance.stompComboVoice;
            for(int x = 0; x < stompAttacks.Length; x++)
            {
                int ix = x;
                string sisyphusPrimeStompString = sisyphusPrimeFolder + "sisyphusYouCantEscape" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(stompAttacks[ix], sisyphusPrimeStompString, (clip) => { try { stompAttacks[ix] = clip; } catch { } });
            }
            
            AudioClip[] taunts = __instance.tauntVoice;
            for(int x = 0; x < taunts.Length; x++)
            {
                int ix = x;
                string sisyphusPrimeTauntString = sisyphusPrimeFolder + "sisyphusNiceTry" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(taunts[ix], sisyphusPrimeTauntString, (clip) => { try { taunts[ix] = clip; } catch { } });
            }
            
            AudioClip[] uppercutAttacks = __instance.uppercutComboVoice;
            for(int x = 0; x < uppercutAttacks.Length; x++)
            {
                int ix = x;
                string sisyphusPrimeUppercutString = sisyphusPrimeFolder + "sisyphusDestroy" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(uppercutAttacks[ix], sisyphusPrimeUppercutString, (clip) => { try { uppercutAttacks[ix] = clip; } catch { } });
            }
            // ref field: assign via temporary and callback
            var inst = __instance; // copy to local to allow capture
            AudioClip tmpPhaseChange = inst.phaseChangeVoice;
            string sisyphusPrimePhaseChangeString = sisyphusPrimeFolder + "sisyphusYesThatsIt";
            AudioSwapper.SwapClipWithFileAsync(tmpPhaseChange, sisyphusPrimePhaseChangeString, (clip) => { try { inst.phaseChangeVoice = clip; } catch { } });
        }
    }
}