using System.Collections.Generic;
using HarmonyLib;
using UltrakULL.json;
using UnityEngine;

namespace UltrakULL.Harmony_Patches.Subtitles
{
	[HarmonyPatch(typeof(GabrielVoice), "Start")]
	public static class GabrielSubtitlesSwap
	{
		private static readonly List<string> FirstEncounterTauntsOrder = new List<string>
		{
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt2,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt3,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt8,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt6,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt9,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt4,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt5,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt7,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt1,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt12,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt10,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_taunt11
		};

		private static readonly List<string> SecondEncounterTauntsOrder = new List<string>
		{
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt6,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt5,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt8,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt4,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt9,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt7,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt1
		};

		private static readonly List<string> SecondEncounterPhaseTwoTauntsOrder = new List<string>
		{
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt11,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt12,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt13,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt3,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt2,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt10,
			LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondTaunt14
		};

		[HarmonyPostfix]
		public static void GabrielVoice_Start(ref GabrielVoice __instance, ref string[] ___taunts, ref string[] ___tauntsSecondPhase)
		{
			if (((Object)((Component)__instance).gameObject).name.Contains("2nd"))
			{
				__instance.phaseChangeSubtitle = LanguageManager.CurrentLanguage.subtitles.subtitles_gabrielSecondPhaseChange;
				for (int i = 0; i < SecondEncounterTauntsOrder.Count; i++)
				{
					___taunts[i] = SecondEncounterTauntsOrder[i];
				}
				for (int j = 0; j < SecondEncounterPhaseTwoTauntsOrder.Count; j++)
				{
					___tauntsSecondPhase[j] = SecondEncounterPhaseTwoTauntsOrder[j];
				}
				SubtitledAudioSourcesReplacer.ReplaceSubsAndAudio();
			}
			else
			{
				__instance.phaseChangeSubtitle = LanguageManager.CurrentLanguage.subtitles.subtitles_gabriel_phaseChange;
				for (int k = 0; k < FirstEncounterTauntsOrder.Count; k++)
				{
					___taunts[k] = FirstEncounterTauntsOrder[k];
				}
				SubtitledAudioSourcesReplacer.ReplaceSubsAndAudio();
			}
		}
	}
}
