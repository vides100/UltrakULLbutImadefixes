using System;
using System.IO;
using HarmonyLib;
using UltrakULL.audio;
using UltrakULL.json;
using UnityEngine;

using static UltrakULL.CommonFunctions;

namespace UltrakULL.Harmony_Patches.AudioSwaps
{
    [HarmonyPatch(typeof(Mandalore),"Start")]
    public static class MandaloreAudioSwap
    {
        [HarmonyPostfix]
        public static void Mandalore_AudioSwap(Mandalore __instance)
        {
            try
            {
                if(LanguageManager.configFile.Bind("General","activeDubbing","False").Value == "False" || isUsingEnglish())
                {
                    return;
                }

                //Mandalore uses an array for MandaloreVoice.
                //voices[0] - Mandalore, voices[1] = Owl.
                
                //NOTE - both audio files for Manda & Owl play at the SAME TIME.
                //This means each seperate audio file will need to have the relevant period of silence before/after speaking.
                string mandaloreFolder = AudioSwapper.SpeechFolder + "mandalore" + Path.DirectorySeparatorChar;
            
                //Attack 1 (Full auto)
                var inst = __instance;
                AudioClip tmpAttack1 = inst.voiceFull;
                string mandaloreAttack1String = mandaloreFolder + "mandaloreFullAuto";
                AudioSwapper.SwapClipWithFileAsync(tmpAttack1, mandaloreAttack1String, (clip) => { try { inst.voiceFull = clip; } catch { } });

                //Attack 2 (Fuller auto)
                AudioClip tmpAttack2 = inst.voiceFuller;
                string mandaloreAttack2String = mandaloreFolder + "mandaloreFullerAuto";
                AudioSwapper.SwapClipWithFileAsync(tmpAttack2, mandaloreAttack2String, (clip) => { try { inst.voiceFuller = clip; } catch { } });

                //Phase change 1 (speed increase)
                ref AudioClip mandalorePhaseChange1Manda = ref __instance.voices[0].secondPhase;
                ref AudioClip mandalorePhaseChange1Owl = ref __instance.voices[1].secondPhase;
                
                string mandalorePhaseChange1MandaString = mandaloreFolder + "mandalorePhaseChange1Manda";
                string mandalorePhaseChange1OwlString = mandaloreFolder + "mandalorePhaseChange1Owl";
                
                AudioClip tmpPhase1M = mandalorePhaseChange1Manda;
                AudioSwapper.SwapClipWithFileAsync(tmpPhase1M, mandalorePhaseChange1MandaString, (clip) => { try { inst.voices[0].secondPhase = clip; } catch { } });
                AudioClip tmpPhase1O = mandalorePhaseChange1Owl;
                AudioSwapper.SwapClipWithFileAsync(tmpPhase1O, mandalorePhaseChange1OwlString, (clip) => { try { inst.voices[1].secondPhase = clip; } catch { } });

                //Phase change 2 (max speed)
                ref AudioClip mandalorePhaseChange2Manda = ref __instance.voices[0].thirdPhase;
                ref AudioClip mandalorePhaseChange2Owl = ref __instance.voices[1].thirdPhase;
                
                string mandalorePhaseChange2MandaString = mandaloreFolder + "mandalorePhaseChange2Manda";
                string mandalorePhaseChange2OwlString = mandaloreFolder + "mandalorePhaseChange2Owl";
                
                AudioClip tmpPhase2M = mandalorePhaseChange2Manda;
                AudioSwapper.SwapClipWithFileAsync(tmpPhase2M, mandalorePhaseChange2MandaString, (clip) => { try { inst.voices[0].thirdPhase = clip; } catch { } });
                AudioClip tmpPhase2O = mandalorePhaseChange2Owl;
                AudioSwapper.SwapClipWithFileAsync(tmpPhase2O, mandalorePhaseChange2OwlString, (clip) => { try { inst.voices[1].thirdPhase = clip; } catch { } });
                
                //Phase change 3 (sanded)
                ref AudioClip mandalorePhaseChange3Manda = ref __instance.voices[0].finalPhase;
                ref AudioClip mandalorePhaseChange3Owl = ref __instance.voices[1].finalPhase;
                
                string mandalorePhaseChange3MandaString = mandaloreFolder + "mandalorePhaseChangeFinalManda";
                string mandalorePhaseChange3OwlString = mandaloreFolder + "mandalorePhaseChangeFinalOwl";
                
                AudioClip tmpPhase3M = mandalorePhaseChange3Manda;
                AudioSwapper.SwapClipWithFileAsync(tmpPhase3M, mandalorePhaseChange3MandaString, (clip) => { try { inst.voices[0].finalPhase = clip; } catch { } });
                AudioClip tmpPhase3O = mandalorePhaseChange3Owl;
                AudioSwapper.SwapClipWithFileAsync(tmpPhase3O, mandalorePhaseChange3OwlString, (clip) => { try { inst.voices[1].finalPhase = clip; } catch { } });
                
                //Defeated
                ref AudioClip mandaloreDefeatedManda = ref __instance.voices[0].death;
                ref AudioClip mandaloreDefeatedOwl = ref __instance.voices[1].death;
                
                string mandaloreDefeatedMandaString = mandaloreFolder + "mandaloreDefeatedManda";
                string mandaloreDefeatedOwlString = mandaloreFolder + "mandaloreDefeatedOwl";
                
                AudioClip tmpDefM = mandaloreDefeatedManda;
                AudioSwapper.SwapClipWithFileAsync(tmpDefM, mandaloreDefeatedMandaString, (clip) => { try { inst.voices[0].death = clip; } catch { } });
                AudioClip tmpDefO = mandaloreDefeatedOwl;
                AudioSwapper.SwapClipWithFileAsync(tmpDefO, mandaloreDefeatedOwlString, (clip) => { try { inst.voices[1].death = clip; } catch { } });
                
                //Respawn taunts
                // copy references to arrays (avoid capturing ref locals in lambdas)
                AudioClip[] mandaloreTauntManda = inst.voices[0].taunts;
                AudioClip[] mandaloreTauntOwl = inst.voices[1].taunts;
                
                string[] mandaTauntLines = 
                {
                    "mandaloreTaunt_YouCannotImagine",
                    "mandaloreTaunt_What",
                    "mandaloreTaunt_HoldStill"
                };
                
                string[] owlTauntLines = 
                {
                    "mandaloreTaunt_ImGonnaShootThem",
                    "mandaloreTaunt_WhyAreWeInThePast",
                    "mandaloreTaunt_ImGonnaPoisonYou",
                };

                int minLength = Math.Min(mandaloreTauntManda.Length, Math.Min(mandaloreTauntOwl.Length, mandaTauntLines.Length));
                for (int x = 0; x < minLength; x++)
                {
                    int ix = x;
                    switch(ix)
                    {
                        case 1:
                        {
                            AudioSwapper.SwapClipWithFileAsync(mandaloreTauntOwl[ix], mandaloreFolder + owlTauntLines[ix], (clip) => { try { inst.voices[1].taunts[ix] = clip; } catch { } });
                            break;
                        }
                        case 3:
                        {
                            AudioSwapper.SwapClipWithFileAsync(mandaloreTauntManda[ix], mandaloreFolder + mandaTauntLines[ix], (clip) => { try { inst.voices[0].taunts[ix] = clip; } catch { } });
                            break;
                        }
                        default:
                        {
                            AudioSwapper.SwapClipWithFileAsync(mandaloreTauntManda[ix], mandaloreFolder + mandaTauntLines[ix], (clip) => { try { inst.voices[0].taunts[ix] = clip; } catch { } });
                            AudioSwapper.SwapClipWithFileAsync(mandaloreTauntOwl[ix], mandaloreFolder + owlTauntLines[ix], (clip) => { try { inst.voices[1].taunts[ix] = clip; } catch { } });
                            break;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
