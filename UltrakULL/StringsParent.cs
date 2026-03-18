using UltrakULL.json;
using UnityEngine;

namespace UltrakULL
{
	public static class StringsParent
	{
		private static string fullMessage;

		public static string GetMessage(string message, string message2, string input)
		{
			string currentSceneName = CommonFunctions.GetCurrentSceneName();
			if (input != null && input.Length > 0)
			{
				input = CommonFunctions.GetLocalizedInput(input);
			}
			if (message.Contains("WARNING") || message.Contains("fall") || message.Contains("free"))
			{
				Logging.Warn("[StringsParent] Level: " + currentSceneName + " | message: '" + message + "' | message2: '" + message2 + "' | input: '" + input + "'");
			}
			if (message.Contains("versions"))
			{
				return LanguageManager.CurrentLanguage.misc.hud_alternateVersion;
			}
			if (message.Contains("ALTERNATE NAILGUN"))
			{
				return LanguageManager.CurrentLanguage.act2.act2_greedFourth_alternateNailgun;
			}
			if (currentSceneName.Contains("Tutorial"))
			{
				GameObject canvasObj = CommonFunctions.GetInactiveRootObject("Canvas");
				new TutorialStrings(ref canvasObj);
				fullMessage = TutorialStrings.GetMessage(message, message2, input);
				if (!fullMessage.Contains("Unimplemented string"))
				{
					return fullMessage;
				}
			}
			if (currentSceneName.Contains("0-") && !currentSceneName.Contains("0-E"))
			{
				fullMessage = PreludeStrings.GetMessage(message, message2, input);
				if (!fullMessage.Contains("Unimplemented string"))
				{
					return fullMessage;
				}
			}
			if ((currentSceneName.Contains("1-") && !currentSceneName.Contains("1-E")) || (currentSceneName.Contains("2-") && !currentSceneName.Contains("2-E")) || (currentSceneName.Contains("3-") && !currentSceneName.Contains("3-E")))
			{
				fullMessage = Act1Strings.GetMessage(message, message2, input);
				if (!fullMessage.Contains("Unimplemented string"))
				{
					return fullMessage;
				}
			}
			if ((currentSceneName.Contains("4-") && !currentSceneName.Contains("4-E")) || (currentSceneName.Contains("5-") && !currentSceneName.Contains("5-E")) || (currentSceneName.Contains("6-") && !currentSceneName.Contains("6-E")))
			{
				fullMessage = Act2Strings.GetMessage(message, message2, input);
				if (!fullMessage.Contains("Unimplemented string"))
				{
					return fullMessage;
				}
			}
			if ((currentSceneName.Contains("7-") && !currentSceneName.Contains("7-E")) || (currentSceneName.Contains("8-") && !currentSceneName.Contains("8-E")) || (currentSceneName.Contains("9-") && !currentSceneName.Contains("9-E")))
			{
				fullMessage = Act3Strings.GetMessage(message, message2, input);
				if (!fullMessage.Contains("Unimplemented string"))
				{
					return fullMessage;
				}
			}
			if (currentSceneName.Contains("CreditsMuseum2"))
			{
				fullMessage = DevMuseum.GetMessage(message, message2, input);
				if (!fullMessage.Contains("Unimplemented string"))
				{
					return fullMessage;
				}
			}
			if (currentSceneName.Contains("-E"))
			{
				fullMessage = EncoreStrings.GetMessage(message, message2, input);
				if (!fullMessage.Contains("Unimplemented string"))
				{
					return fullMessage;
				}
			}
			if (message.Contains("V-Rank"))
			{
				return message;
			}
			if (message.Contains("PUNCH"))
			{
				return "<color=red>" + LanguageManager.CurrentLanguage.misc.hud_noArm1 + "</color>\n" + LanguageManager.CurrentLanguage.misc.hud_noArm2;
			}
			if (message.Contains("MAJOR"))
			{
				return "<color=#4C99E6>" + LanguageManager.CurrentLanguage.misc.hud_majorAssists + "</color>";
			}
			if (message.Contains("200"))
			{
				return LanguageManager.CurrentLanguage.misc.hud_overhealOrb1 + "\n" + LanguageManager.CurrentLanguage.misc.hud_overhealOrb2;
			}
			if (message.Contains("ERROR"))
			{
				return "<color=red>" + LanguageManager.CurrentLanguage.misc.hud_itemGrabError + "</color>";
			}
			if (message.Contains("TAB"))
			{
				return LanguageManager.CurrentLanguage.misc.hud_levelStats1 + "\n" + LanguageManager.CurrentLanguage.misc.hud_levelStats2;
			}
			if (message.Contains("Whoops"))
			{
				return LanguageManager.CurrentLanguage.misc.hud_outOfBounds;
			}
			if (message.Contains("CLASH"))
			{
				return LanguageManager.CurrentLanguage.misc.hud_clashMode;
			}
			if (message.Contains("DRONE HAUNTING"))
			{
				return LanguageManager.CurrentLanguage.misc.hud_droneHaunting;
			}
			if (message.Contains("EQUIPPED"))
			{
				return LanguageManager.CurrentLanguage.misc.hud_weaponVariation;
			}
			if (message.Contains("Altered"))
			{
				return "<color=red>" + LanguageManager.CurrentLanguage.misc.enemyAlter_alteredDestroyed + "</color>";
			}
			if (message.Contains("INSUFFICIENT LIGHT"))
			{
				return LanguageManager.CurrentLanguage.primeSanctum.primeSanctum_first_insufficientlight;
			}
			if (message.Contains("=>"))
			{
				return message;
			}
			Logging.Warn("Unimplemented string in \"" + CommonFunctions.GetCurrentSceneName() + "\": " + message);
			return message;
		}

		public static string GetLevelTip(string tipDescriptionText)
		{
			string currentSceneName = CommonFunctions.GetCurrentSceneName();
			if (currentSceneName.Contains("0-2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_preludeSecond1 + "\n\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_preludeSecond2;
			}
			if (currentSceneName.Contains("0-3"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_preludeThird1 + "\n\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_preludeThird2 + "\n\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_preludeThird3;
			}
			if (currentSceneName.Contains("0-4"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_preludeFourth1 + "\n\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_preludeFourth2;
			}
			if (currentSceneName.Contains("0-5"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_preludeFifth;
			}
			if (currentSceneName.Contains("1-1"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_limboFirst;
			}
			if (currentSceneName.Contains("1-2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_limboSecond;
			}
			if (currentSceneName.Contains("1-3"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_limboThird1 + "\n\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_limboThird2;
			}
			if (currentSceneName.Contains("1-4"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_limboFourth;
			}
			if (currentSceneName.Contains("2-1"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_lustFirst;
			}
			if (currentSceneName.Contains("2-2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_lustSecond1 + "\n\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_lustSecond2 + "\n\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_lustSecond3;
			}
			if (currentSceneName.Contains("2-3"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_lustThird;
			}
			if (currentSceneName.Contains("2-4"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_lustFourth1 + "\n\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_lustFourth2;
			}
			if (currentSceneName.Contains("3-1"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_gluttonyFirst;
			}
			if (currentSceneName.Contains("3-2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_gluttonySecond1 + "\n\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_gluttonySecond2;
			}
			if (currentSceneName.Contains("4-1"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_greedFirst;
			}
			if (currentSceneName.Contains("4-2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_greedSecond;
			}
			if (currentSceneName.Contains("4-3"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_greedThird;
			}
			if (currentSceneName.Contains("4-4"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_greedFourth;
			}
			if (currentSceneName.Contains("5-1"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_wrathFirst;
			}
			if (currentSceneName.Contains("5-2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_wrathSecond;
			}
			if (currentSceneName.Contains("5-3"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_wrathThird;
			}
			if (currentSceneName.Contains("5-4"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_wrathFourth1 + "\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_wrathFourth2;
			}
			if (currentSceneName.Contains("6-1"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_heresyFirst1 + "\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_heresyFirst2;
			}
			if (currentSceneName.Contains("6-2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_heresySecond1 + "\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_heresySecond2;
			}
			if (currentSceneName.Contains("7-1"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_violenceFirst;
			}
			if (currentSceneName.Contains("7-2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_violenceSecond;
			}
			if (currentSceneName.Contains("7-3"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_violenceThird;
			}
			if (currentSceneName.Contains("7-4"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_violenceFourth;
			}
			if (currentSceneName.Contains("7-S"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_violenceSecret;
			}
			if (currentSceneName.Contains("8-1"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_fraudFirst;
			}
			if (currentSceneName.Contains("8-2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_fraudSecond;
			}
			if (currentSceneName.Contains("8-3"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_fraudThird;
			}
			if (currentSceneName.Contains("8-4"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_fraudFourth;
			}
			if (currentSceneName.Contains("9-1"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_treacheryFirst;
			}
			if (currentSceneName.Contains("9-2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_treacherySecond;
			}
			if (currentSceneName.Contains("uk_construct"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_sandbox1 + "\n<color=orange>↑ ↑ ↓ ↓ ← → ← → B A</color>\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_sandbox2;
			}
			if (currentSceneName.Contains("0-E"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_encorePrelude1 + "\n\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_encorePrelude2;
			}
			if (currentSceneName.Contains("1-E"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_encoreLimbo;
			}
			if (currentSceneName.Contains("2-E"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_encoreLust;
			}
			if (currentSceneName.Contains("3-E"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_encoreGluttony;
			}
			if (currentSceneName.Contains("4-E"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_encoreGreed;
			}
			if (currentSceneName.Contains("5-E"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_encoreWrath;
			}
			if (currentSceneName.Contains("6-E"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_encoreHeresy;
			}
			if (currentSceneName.Contains("7-E"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_encoreViolence;
			}
			if (currentSceneName.Contains("8-E"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_encoreFraud;
			}
			if (currentSceneName.Contains("9-E"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_encoreTreachery;
			}
			if (currentSceneName.Contains("P-1"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_primeFirst1 + "\n\n" + LanguageManager.CurrentLanguage.levelTips.leveltips_primeFirst2;
			}
			if (currentSceneName.Contains("P-2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_primeSecond;
			}
			if (currentSceneName.Contains("P-3"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_primeThird;
			}
			if (currentSceneName.Contains("Endless"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_cybergrind;
			}
			if (currentSceneName.Contains("CreditsMuseum2"))
			{
				return LanguageManager.CurrentLanguage.levelTips.leveltips_devMuseum;
			}
			Logging.Warn("The source of the Level tip is not specified in the mod. We returned the value as it was. Level name: " + currentSceneName + ". tipDescriptionText= \"" + tipDescriptionText + "\"");
			return tipDescriptionText;
		}
	}
}
