using BepInEx.Configuration;
using HarmonyLib;
using UltrakULL.audio;
using UltrakULL.json;
using UnityEngine;

namespace UltrakULL.Harmony_Patches.AudioSwaps
{
	[HarmonyPatch(typeof(SisyphusPrime), "Start")]
	public class SisyphusPrimeAudioSwap
	{
		[HarmonyPostfix]
		public static void SisyphusPrimeAudioSwapPatch(ref SisyphusPrime __instance)
		{
			if (LanguageManager.configFile.Bind<string>("General", "activeDubbing", "False", (ConfigDescription)null).Value == "False" || CommonFunctions.isUsingEnglish())
			{
				return;
			}
			string text = AudioSwapper.SpeechFolder + "sisyphusPrime\\";
			AudioClip[] begoneAttacks = __instance.clapVoice;
			for (int i = 0; i < begoneAttacks.Length; i++)
			{
				int ix = i;
				string audioFilePath = text + "sisyphusBegone" + (ix + 1);
				AudioSwapper.SwapClipWithFileAsync(begoneAttacks[ix], audioFilePath, delegate(AudioClip clip)
				{
					try
					{
						begoneAttacks[ix] = clip;
					}
					catch
					{
					}
				});
			}
			AudioClip[] thisWillHurtAttack = __instance.explosionVoice;
			for (int num = 0; num < thisWillHurtAttack.Length; num++)
			{
				int ix2 = num;
				string audioFilePath2 = text + "sisyphusThisWillHurt";
				AudioSwapper.SwapClipWithFileAsync(thisWillHurtAttack[ix2], audioFilePath2, delegate(AudioClip clip)
				{
					try
					{
						thisWillHurtAttack[ix2] = clip;
					}
					catch
					{
					}
				});
			}
			AudioClip[] grunt = __instance.hurtVoice;
			for (int num2 = 0; num2 < grunt.Length; num2++)
			{
				int ix3 = num2;
				string audioFilePath3 = text + "sisyphusGrunt";
				AudioSwapper.SwapClipWithFileAsync(grunt[ix3], audioFilePath3, delegate(AudioClip clip)
				{
					try
					{
						grunt[ix3] = clip;
					}
					catch
					{
					}
				});
			}
			AudioClip[] stompAttacks = __instance.stompComboVoice;
			for (int num3 = 0; num3 < stompAttacks.Length; num3++)
			{
				int ix4 = num3;
				string audioFilePath4 = text + "sisyphusYouCantEscape" + (ix4 + 1);
				AudioSwapper.SwapClipWithFileAsync(stompAttacks[ix4], audioFilePath4, delegate(AudioClip clip)
				{
					try
					{
						stompAttacks[ix4] = clip;
					}
					catch
					{
					}
				});
			}
			AudioClip[] taunts = __instance.tauntVoice;
			for (int num4 = 0; num4 < taunts.Length; num4++)
			{
				int ix5 = num4;
				string audioFilePath5 = text + "sisyphusNiceTry" + (ix5 + 1);
				AudioSwapper.SwapClipWithFileAsync(taunts[ix5], audioFilePath5, delegate(AudioClip clip)
				{
					try
					{
						taunts[ix5] = clip;
					}
					catch
					{
					}
				});
			}
			AudioClip[] uppercutAttacks = __instance.uppercutComboVoice;
			for (int num5 = 0; num5 < uppercutAttacks.Length; num5++)
			{
				int ix6 = num5;
				string audioFilePath6 = text + "sisyphusDestroy" + (ix6 + 1);
				AudioSwapper.SwapClipWithFileAsync(uppercutAttacks[ix6], audioFilePath6, delegate(AudioClip clip)
				{
					try
					{
						uppercutAttacks[ix6] = clip;
					}
					catch
					{
					}
				});
			}
			SisyphusPrime inst = __instance;
			AudioClip phaseChangeVoice = inst.phaseChangeVoice;
			string audioFilePath7 = text + "sisyphusYesThatsIt";
			AudioSwapper.SwapClipWithFileAsync(phaseChangeVoice, audioFilePath7, delegate(AudioClip clip)
			{
				try
				{
					inst.phaseChangeVoice = clip;
				}
				catch
				{
				}
			});
		}
	}
}
