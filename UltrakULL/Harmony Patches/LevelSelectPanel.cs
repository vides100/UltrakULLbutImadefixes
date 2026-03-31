using HarmonyLib;
using UltrakULL.json;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

using static UltrakULL.CommonFunctions;
using System.Diagnostics.Eventing.Reader;

namespace UltrakULL.Harmony_Patches
{
	//@Override
	//Overrides checkScore function from the vanilla game. This translates level names, as well as if challenges have been completed or not. POSTFIX.
	[HarmonyPatch(typeof(LevelSelectPanel), "CheckScore")]
	public static class LocalizeGameProgressChallenges
	{
		[HarmonyPostfix]
		public static void CheckScore_MyPatchPostFix(LevelSelectPanel __instance)
		{
			if(isUsingEnglish())
			{
				return;
			}
			int num = __instance.levelNumber;
			RankData rank = GameProgressSaver.GetRank(num, false);
			try
			{
                // The level name replacement function has been moved to a separate Harmony Patch (GetMissionName.cs)
                if (rank.levelNumber == __instance.levelNumber || ((__instance.levelNumber == 666 || __instance.levelNumber == 100) && rank.levelNumber == __instance.levelNumber + __instance.levelNumberInLayer - 1))
				{
					if (__instance.challengeIcon)
					{
						if (LanguageManager.CurrentLanguage.frontend.level_challengeCompleted == null)
							return;
						if (rank.challenge)
						{
							__instance.challengeIcon.fillCenter = true;
							TextMeshProUGUI componentInChildren2 = __instance.challengeIcon.GetComponentInChildren<TextMeshProUGUI>();
							componentInChildren2.text = String.Join(" ", LanguageManager.CurrentLanguage.frontend.level_challengeCompleted.ToList()); //Challenge completed
						}
						else
						{
							__instance.challengeIcon.fillCenter = false;
							TextMeshProUGUI componentInChildren3 = __instance.challengeIcon.GetComponentInChildren<TextMeshProUGUI>();
							componentInChildren3.text = String.Join(" ", LanguageManager.CurrentLanguage.frontend.level_challenge.ToList()); //Challenge not completed
							componentInChildren3.color = Color.white;
						}
					}
				}
				else
				{

					TextMeshProUGUI componentInChildren3 = __instance.challengeIcon.GetComponentInChildren<TextMeshProUGUI>();
					componentInChildren3.text = String.Join(" ", LanguageManager.CurrentLanguage.frontend.level_challenge.ToList()); //Challenge not completed
					componentInChildren3.color = Color.white;
				}
			}
			catch (Exception e)
			{
				Logging.Error("Exception occured :  " + num);
                Logging.Error(e.ToString());
			}
		}
	}
}
