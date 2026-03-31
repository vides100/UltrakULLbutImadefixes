using HarmonyLib;
using System.IO;
using UltrakULL.audio;
using UltrakULL.json;
using UnityEngine;

using static UltrakULL.CommonFunctions;

namespace UltrakULL.Harmony_Patches.AudioSwaps
{
    [HarmonyPatch(typeof(MinosPrime),"Start")]
    public class MinosPrimeAudioSwap
    {
        [HarmonyPostfix]
        public static void MinosPrime_VoiceSwap(ref MinosPrime __instance)
        {
            if(LanguageManager.configFile.Bind("General","activeDubbing","False").Value == "False" || isUsingEnglish())
            {
                return;
            }
            string minosPrimeFolder =  AudioSwapper.SpeechFolder + "minosPrime" + Path.DirectorySeparatorChar;


            //Rider Kick (Die)
            AudioClip[] minosPrimeKick = __instance.riderKickVoice;
            for(int x = 0; x < minosPrimeKick.Length; x++)
            {
                int ix = x;
                string minosPrimeKickString = minosPrimeFolder + "minosPrimeDie" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(minosPrimeKick[ix], minosPrimeKickString, (clip) => { try { minosPrimeKick[ix] = clip; } catch { } });
            }
            
            //Dropkick (Judgement)
            AudioClip[] minosPrimeJudgement = __instance.dropkickVoice;
            for(int x = 0; x < minosPrimeJudgement.Length; x++)
            {
                int ix = x;
                string minosPrimeJudgementString = minosPrimeFolder + "minosPrimeJudgement" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(minosPrimeJudgement[ix], minosPrimeJudgementString, (clip) => { try { minosPrimeJudgement[ix] = clip; } catch { } });
            }
            
            //Crush attack (Crush)
            AudioClip[] minosPrimeCrush = __instance.dropAttackVoice;
            for(int x = 0; x < minosPrimeCrush.Length; x++)
            {
                int ix = x;
                string minosPrimeCrushString = minosPrimeFolder + "minosPrimeCrush" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(minosPrimeCrush[ix], minosPrimeCrushString, (clip) => { try { minosPrimeCrush[ix] = clip; } catch { } });
            }
            
            //Punches/Boxing (Thy end is now)
            AudioClip[] minosPrimePunch = __instance.boxingVoice;
            for(int x = 0; x < minosPrimePunch.Length; x++)
            {
                int ix = x;
                string minosPrimePunchString = minosPrimeFolder + "minosPrimeThyEndIsNow" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(minosPrimePunch[ix], minosPrimePunchString, (clip) => { try { minosPrimePunch[ix] = clip; } catch { } });
            }
            
            //Combo (prepare thyself)
            AudioClip[] minosPrimeCombo = __instance.comboVoice;
            for(int x = 0; x < minosPrimeCombo.Length; x++)
            {
                int ix = x;
                string minosPrimeComboString = minosPrimeFolder + "minosPrimePrepareThyself" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(minosPrimeCombo[ix], minosPrimeComboString, (clip) => { try { minosPrimeCombo[ix] = clip; } catch { } });
            }
            
            
            //Phase change - need to use ref otherwise it gets swapped back to original
            var inst = __instance;
            AudioClip tmpPhase = inst.phaseChangeVoice;
            string minosPrimePhaseChangeString = minosPrimeFolder + "minosPrimePhaseChange";
            AudioSwapper.SwapClipWithFileAsync(tmpPhase, minosPrimePhaseChangeString, (clip) => { try { inst.phaseChangeVoice = clip; } catch { } });
            
            
            //Hurt
            AudioClip[] minosPrimeHurt = __instance.hurtVoice;
            for(int x = 0; x < minosPrimeHurt.Length; x++)
            {
                int ix = x;
                string minosPrimeHurtString = minosPrimeFolder + "minosPrimeHurt" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(minosPrimeHurt[ix], minosPrimeHurtString, (clip) => { try { minosPrimeHurt[ix] = clip; } catch { } });
            }
        }
    }
}
