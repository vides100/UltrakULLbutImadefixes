using HarmonyLib;
using System.IO;
using UltrakULL.audio;
using UltrakULL.json;
using UnityEngine;

using static UltrakULL.CommonFunctions;

namespace UltrakULL.Harmony_Patches.AudioSwaps
{
    [HarmonyPatch(typeof(Gabriel),"Start")]
    public static class GabrielAudioSwap
    {
        [HarmonyPostfix]
        public static void Gabriel_VoiceSwap(ref Gabriel __instance)
        {
            if(LanguageManager.configFile.Bind("General","activeDubbing","False").Value == "False" || isUsingEnglish())
                return;
            
            string gabeFirstFolder =  AudioSwapper.SpeechFolder + "gabrielBossFirst" + Path.DirectorySeparatorChar;
            
            var gabeBase = __instance.gabe;
            if (gabeBase == null) return;
            
            GabrielVoice voice = null;
            try
            {
                var voiceProperty = gabeBase.GetType().GetProperty("voice");
                if (voiceProperty != null)
                {
                    voice = voiceProperty.GetValue(gabeBase) as GabrielVoice;
                }
                else
                {
                    var voiceField = gabeBase.GetType().GetField("voice", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    if (voiceField != null)
                    {
                        voice = voiceField.GetValue(gabeBase) as GabrielVoice;
                    }
                }
            }
            catch
            {
                return;
            }
            
            if (voice == null) return;
            
            //Taunts
            AudioClip[] gabeTaunts = voice.taunt;
            
            //Line order is based on line order of the, so it's not alphabetical.
            string[] tauntLines =
            {
                "gabrielTaunt_YouDefyTheLight",
                "gabrielTaunt_AMereObject",
                "gabrielTaunt_ThereCanBeOnlyLight",
                "gabrielTaunt_Foolishness",
                "gabrielTaunt_AnImperfection",
                "gabrielTaunt_NotEvenMortal",
                "gabrielTaunt_YouAreLessThanNothing",
                "gabrielTaunt_YoureAnError",
                "gabrielTaunt_TheLightIsPerfection",
                "gabrielTaunt_YouAreOutclassed",
                "gabrielTaunt_YourCrimeIsExistence",
                "gabrielTaunt_YouMakeEven"
            };
            for(int x = 0; x < gabeTaunts.Length; x++)
            {
                int ix = x;
                string gabrielTauntString = gabeFirstFolder + tauntLines[ix];
                AudioSwapper.SwapClipWithFileAsync(gabeTaunts[ix], gabrielTauntString, (clip) => { try { gabeTaunts[ix] = clip; } catch { } });
            }
            
            //Phase change - need to use ref otherwise it gets swapped back to original
            AudioClip tmpPhase = voice.phaseChange;
            string gabrielPhaseChangeString = gabeFirstFolder + "gabrielPhaseChange";
            AudioSwapper.SwapClipWithFileAsync(tmpPhase, gabrielPhaseChangeString, (clip) => { try { voice.phaseChange = clip; } catch { } });

            //Big hurt
            AudioClip[] gabeBigHurt = voice.bigHurt;
            for(int x = 0; x < gabeBigHurt.Length; x++)
            {
                int ix = x;
                string gabrielBigHurtString = gabeFirstFolder + "gabrielBigHurt" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(gabeBigHurt[ix], gabrielBigHurtString, (clip) => { try { gabeBigHurt[ix] = clip; } catch { } });
            }

            //Hurt
            AudioClip[] gabeHurt = voice.hurt;
            for(int x = 0; x < gabeHurt.Length; x++)
            {
                int ix = x;
                string gabrielHurtString = gabeFirstFolder + "gabrielHurt" + (ix+1).ToString();
                AudioSwapper.SwapClipWithFileAsync(gabeHurt[ix], gabrielHurtString, (clip) => { try { gabeHurt[ix] = clip; } catch { } });
            }
        }
    }
}
