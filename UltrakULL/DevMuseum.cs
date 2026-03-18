using TMPro;
using UltrakULL.json;
using UnityEngine;
using UnityEngine.UI;

namespace UltrakULL
{
	public class DevMuseum
	{
		public static string GetMessage(string message, string message2, string input)
		{
			if (message.Contains("RACE START"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_rocketRaceStart;
			}
			if (message.Contains("A R M B O Y"))
			{
				return LanguageManager.CurrentLanguage.act2.act2_heresyFirst_armboy;
			}
			if (message.Contains("TIME"))
			{
				string text = message.Split(':')[1];
				return LanguageManager.CurrentLanguage.misc.levelstats_time + ": " + text;
			}
			if (message.Contains("Chess"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_chessTip;
			}
			return "Unimplemented string";
		}

		public static string GetMuseumBook(string originalText)
		{
			if (originalText.Contains("HAKITA</color> - CREATOR OF ULTRAKILL</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita5 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita6 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita7 + "\n\n<size=18>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita8 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita9 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita10 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita11 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita12 + "</size>\n\n<i><color=orange>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHakita13 + "</color></i>";
			}
			if (originalText.Contains("FRANCIS XIE</color> - CONCEPT AND TEXTURE ARTIST</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookFrancisXie1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFrancisXie2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFrancisXie3 + "\n\n<size=18>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFrancisXie4 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFrancisXie5 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFrancisXie6 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFrancisXie7 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFrancisXie8 + "</size>\n\n<i><color=#4AACBD>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFrancisXie9 + "</color></i>";
			}
			if (originalText.Contains("JERICHO_RUS</color> - ILLUSTRATOR, CONCEPT AND TEXTURE ARTIST</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookJerichoRus1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookJerichoRus2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookJerichoRus3 + "\n\n<size=18>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookJerichoRus4 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookJerichoRus5 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookJerichoRus6 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookJerichoRus7 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookJerichoRus8 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookJerichoRus9 + "</size>\n\n<i><color=#5cc6f1>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookJerichoRus10 + "</color></i>";
			}
			if (originalText.Contains("BIGROCKBMP</color> - CONCEPT ARTIST</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookBigRockBMP1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookBigRockBMP2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookBigRockBMP3 + "\n\n<size=18>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookBigRockBMP4 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookBigRockBMP5 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookBigRockBMP6 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookBigRockBMP7 + "</size>\n\n<i><color=#DA6B6D>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookBigRockBMP8 + "</color></i>";
			}
			if (originalText.Contains("MAXIMILIAN OVESSON</color> - UI ARTIST</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookMaximilianOvesson1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookMaximilianOvesson2 + "\n\n<i><color=#8f65da>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookMaximilianOvesson3 + "</color></i>\n\n";
			}
			if (originalText.Contains("VICTORIA HOLLAND</color> - LEAD 3D ARTIST AND GRAPHICS PROGRAMMER</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookVictoriaHolland1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVictoriaHolland2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVictoriaHolland3 + "\n\n<size=18>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVictoriaHolland4 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVictoriaHolland5 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVictoriaHolland6 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVictoriaHolland7 + "</size>\n\n<i><color=#F5ABB9>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVictoriaHolland8 + "</color></i>\n\n";
			}
			if (originalText.Contains("TONI STIGELL</color> - 3D ARTIST</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookToniStigell1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookToniStigell2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookToniStigell3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookToniStigell4 + "\n\n<size=18>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookToniStigell5 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookToniStigell6 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookToniStigell7 + "</size>";
			}
			if (originalText.Contains("FLYINGDOG</color> - 3D ARTIST</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookFlyingDog1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFlyingDog2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFlyingDog3 + "\n\n<size=18>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFlyingDog4 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFlyingDog5 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookFlyingDog6 + "</size>";
			}
			if (originalText.Contains("SAMUEL JAMES BRYAN</color> - 3D ARTIST</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookSamuelJamesBryan1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookSamuelJamesBryan2 + "\n\n<i><color=orange>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookSamuelJamesBryan3 + "</color></i>";
			}
			if (originalText.Contains("<b><color=red>CAMERON MARTIN</color> - QUALITY ASSURANCE LEAD"))
			{
				return "<b>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamLine1 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamLine2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamLine3 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamLine4 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamLine5 + "</b>\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamDesc1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamDesc2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamDesc3 + "\n\n<color=red><i>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamQuote1 + "</i></color>\n\n<color=#6a36be><i>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamQuote2 + "</i></color>\n\n<color=#11c324><i>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamQuote3 + "</i></color>\n\n<color=#e28eb6><i>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamQuote4 + "</i></color>\n\n<color=#4480e6><i>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQATeamQuote5 + "</i></color>";
			}
			if (originalText.Contains("<b><color=orange>PITR</color> - LEAD PROGRAMMER</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookPitr1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookPitr2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookPitr3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookPitr4 + "\n\n<size=18>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookPitr5 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookPitr6 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookPitr7 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookPitr8 + "</size>";
			}
			if (originalText.Contains("<b><color=orange>HECKTECK</color> - LEAD PROGRAMMER</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookHeckteck1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHeckteck2 + "\n\n<i><color=orange>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHeckteck3 + "</color></i>";
			}
			if (originalText.Contains("HAZELUFF</color> - PROGRAMMER</b>"))
			{
				return "<b>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHazeluff1 + "</b>\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHazeluff2 + "\n\n<color=#6153AB><i>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHazeluff3 + "</i></color>";
			}
			if (originalText.Contains("CHIZHOV</color> - ADDITIONAL PROGRAMMER</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookCabalcrow1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookCabalcrow2 + "\n\n<i><color=#c0c0c0ff>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookCabalcrow3 + "</color></i>";
			}
			if (originalText.Contains("LUCAS VARNEY</color> - ADDITIONAL PROGRAMMER</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookLucasVarney1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookLucasVarney2 + "\n\n<i><color=#BD8BF3>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookLucasVarney3 + "</color></i>";
			}
			if (originalText.Contains("BEN MOIR</color> - ADDITIONAL PROGRAMMER</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookBenMoir1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookBenMoir2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookBenMoir3 + "\n\n<i><color=#3EF242>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookBenMoir4 + "</color></i>\n\n";
			}
			if (originalText.Contains("MEGANEKO</color> - GUEST COMPOSER</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookMeganeko1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookMeganeko2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookMeganeko3 + "\n\n<i><color=#E93436>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookMeganeko4 + "</color></i>\n\n";
			}
			if (originalText.Contains("KEYGEN CHURCH</color> - GUEST COMPOSER</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookKeygenChurch1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKeygenChurch2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKeygenChurch3 + "\n\n<i><color=#aa0000>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKeygenChurch4 + "</color></i>\n\n";
			}
			if (originalText.Contains("HEALTH</color> - GUEST COMPOSER</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookHealth1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHealth2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHealth3 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHealth4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHealth5 + "\n\n<i><color=red>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookHealth6 + "</color></i>";
			}
			if (originalText.Contains("KING GIZZARD & THE LIZARD WIZARD</color> - GUEST COMPOSER</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard3 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard4 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard5 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard6 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard7 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard8 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard9 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard10 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard11 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookKingGizzard12;
			}
			if (originalText.Contains("QUETZAL TIRADO</color> - GUEST MUSICIAN</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookQuetzalTirado1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQuetzalTirado2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQuetzalTirado3 + "\n\n<i><color=#AA4CAD>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookQuetzalTirado4 + "</color></i>";
			}
			if (originalText.Contains("SALAD</color> - HELPING HAND</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookSalad1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookSalad2 + "</size>\n\n<i><color=#20FF20>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookSalad3 + "</color></i>";
			}
			if (originalText.Contains("JACOB H.H.R.</color> - WRITER (PROSE & DIALOGUE)</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookJacobHHR1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookJacobHHR2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookJacobHHR3;
			}
			if (originalText.Contains("VVIZARD</color> - MUSEUM DEVELOPER</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookVVizard1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVVizard2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVVizard3 + "\n\n<i><color=#ee0c47>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVVizard4 + "</color></i>";
			}
			if (originalText.Contains("ADDITIONAL MUSIC CREDITS"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic5 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic6 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic7 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic8 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic9 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic10 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic11 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalMusic12;
			}
			if (originalText.Contains("COMMUNITY CYBER GRIND"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalCredits1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalCredits2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalCredits3 + "\n\nNO LOAFING\nDood\nSplendidLedraps\nJandy\nStuon\nDryzalar\nWakan\nSlimer\nWilliam\nBobot\nSpruce\nJacob\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalCredits4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalCredits5 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalCredits6 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalCredits7 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalCredits8 + "\n\n<b><color=orange>UltrakULL CREDITS</color>:</b>\n\nMod created by <color=orange>Clearwater</color>\nAdditional code contributions by <color=orange>Temperz87</color>, <color=orange>CoatlessAli</color> and <color=orange>Frizou</color>\nTranslations by various community members of the <color=orange>UltrakULL Translation Team</color>\nDocumentation contributions by <color=orange>Frizou</color>\n\n<color=orange>" + LanguageManager.CurrentLanguage.metadata.langDisplayName + "</color>:\n" + LanguageManager.CurrentLanguage.metadata.langAuthor;
			}
			if (originalText.Contains("STEPHAN WEYTE</color> - VOICE OF MINOS PRIME</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookStephanWeyte1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookStephanWeyte2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookStephanWeyte3 + "\n\n";
			}
			if (originalText.Contains("LENVAL BROWN</color> - VOICE OF SISYPHUS PRIME</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookLenvalBrown1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookLenvalBrown2;
			}
			if (originalText.Contains("GIANNI MATRAGRANO</color> - VOICE OF GABRIEL</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookGianniMatragrano1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookGianniMatragrano2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookGianniMatragrano3 + "\n\n<i><color=#20afdb>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookGianniMatragrano4 + "</color></i>";
			}
			if (originalText.Contains("MANDALORE</color> <color=#9884bb>HERRINGTON</color> - VOICE OF MYSTERIOUS DRUID KNIGHT</b>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_bookMandalore1 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookMandalore2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookMandalore3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookMandalore4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookMandalore5 + "\n\n<i>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookMandalore6 + "</i>\n\n<i><color=#eabbd7>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookMandalore7 + "</color></i>";
			}
			if (originalText.Contains("KENNADY RAY</color> - VOICE OF POWER</b>"))
			{
				return "<b><color=#FF00A1>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookPower1 + "</color></b>\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookPower2 + "\n\n<color=#FF00A1><i>\"" + LanguageManager.CurrentLanguage.devMuseum.museum_bookPower3 + "\"</i></color>";
			}
			if (originalText.Contains("VYLET PONY</color> - GUEST COMPOSER</b>"))
			{
				return "<b><color=#A26ADE>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVylet1 + "</color></b>\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVylet2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVylet3 + "\n\n<color=#A26ADE><i>\"" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVylet4 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookVylet5 + "\"</i></color>";
			}
			if (originalText.Contains("DOMENICO ANTONAZZO</color> - RIGGING</b>"))
			{
				return "<b><color=#979283>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalArt1 + "</color></b>\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalArt2 + "\n\n<color=#979283><i>\"" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalArt3 + "\"</i></color>\n\n<b><color=#c7a6ef>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalArt4 + "</color></b>\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalArt5 + "\n\n<color=#c7a6ef><i>\"" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalArt6 + "\"</i></color>\n\n<b><color=#b12b39>" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalArt7 + "</color></b>\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalArt8 + "\n\n<color=#b12b39><i>\"" + LanguageManager.CurrentLanguage.devMuseum.museum_bookAdditionalArt9 + "\"</i></color>";
			}
			if (originalText.Contains("FILTH</color>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_enemiesFilth1 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesFilth2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesFilth3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesFilth4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesFilth5 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesFilth6 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesFilth7;
			}
			if (originalText.Contains("STRAY</color>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_enemiesStray1 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesStray2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesStray3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesStray4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesStray5;
			}
			if (originalText.Contains("SCHISM</color>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSchism1 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSchism2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSchism3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSchism4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSchism5 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSchism6;
			}
			if (originalText.Contains("SWORDSMACHINE</color>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSwordsmachine1 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSwordsmachine2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSwordsmachine3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSwordsmachine4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSwordsmachine5 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesSwordsmachine6;
			}
			if (originalText.Contains("MALICIOUS FACE</color>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_enemiesMaliciousFace1 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesMaliciousFace2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesMaliciousFace3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesMaliciousFace4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesMaliciousFace5 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesMaliciousFace6 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_enemiesMaliciousFace7;
			}
			if (originalText.Contains("BEAMCUTTER</color>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBeamcutter1 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBeamcutter2 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBeamcutter3 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBeamcutter4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBeamcutter5 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBeamcutter6 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBeamcutter7 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBeamcutter8 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBeamcutter9;
			}
			if (originalText.Contains("BLACK HOLE CANNON</color>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBlackHoleCannon1 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBlackHoleCannon2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBlackHoleCannon3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBlackHoleCannon4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBlackHoleCannon5 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBlackHoleCannon6 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBlackHoleCannon7 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsBlackHoleCannon8;
			}
			if (originalText.Contains("REVOLVER</color>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_weaponsRevolver1 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsRevolver2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsRevolver3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsRevolver4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsRevolver5 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsRevolver6;
			}
			if (originalText.Contains("SHOTGUN</color>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_weaponsShotgun1 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsShotgun2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsShotgun3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsShotgun4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsShotgun5;
			}
			if (originalText.Contains("NAILGUN</color>"))
			{
				return LanguageManager.CurrentLanguage.devMuseum.museum_weaponsNailgun1 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsNailgun2 + "\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsNailgun3 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsNailgun4 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsNailgun5 + "\n\n" + LanguageManager.CurrentLanguage.devMuseum.museum_weaponsNailgun6;
			}
			return originalText ?? "";
		}

		private void PatchPlaques()
		{
			CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(((Component)CommonFunctions.GetInactiveRootObject("__Room_Courtyard").transform.GetChild(4).GetChild(0)).gameObject, "Canvas (2)"), "Text")).text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesMuseumTitle;
			GameObject gameObject = ((Component)CommonFunctions.GetInactiveRootObject("__Room_FrontDesk_1").transform.GetChild(1)).gameObject;
			GameObject gameObject2 = ((Component)gameObject.transform.GetChild(58)).gameObject;
			GameObject gameObject3 = ((Component)gameObject.transform.GetChild(0)).gameObject;
			GameObject gameObject4 = ((Component)gameObject.transform.GetChild(1)).gameObject;
			CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObject2, "Canvas (3)"), "Text")).text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesHakita1;
			CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObject2, "Canvas (3)"), "Text (1)")).text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesHakita2;
			CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObject3, "Canvas (3)"), "Text")).text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesArtRoom;
			CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObject4, "Canvas (3)"), "Text")).text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesNerdRoom;
			GameObject inactiveRootObject = CommonFunctions.GetInactiveRootObject("__DEV_SPACE_ALL");
			GameObject gameObjectChild = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (13)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild, "Text"));
			Text textfromGameObject2 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild, "Text (1)"));
			textfromGameObject.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesFrancisXie1;
			textfromGameObject2.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesFrancisXie2;
			GameObject gameObjectChild2 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (14)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject3 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild2, "Text"));
			Text textfromGameObject4 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild2, "Text (1)"));
			textfromGameObject3.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesJerichoRus1;
			textfromGameObject4.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesJerichoRus2;
			GameObject gameObjectChild3 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (12)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject5 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild3, "Text"));
			Text textfromGameObject6 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild3, "Text (1)"));
			textfromGameObject5.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesBigRockBMP1;
			textfromGameObject6.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesBigRockBMP2;
			GameObject gameObjectChild4 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (15)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject7 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild4, "Text"));
			Text textfromGameObject8 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild4, "Text (1)"));
			textfromGameObject7.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesMaxOvesson1;
			textfromGameObject8.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesMaxOvesson2;
			GameObject gameObjectChild5 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "dev Space Large"), "Dev Large Placard"), "Canvas (4)");
			Text textfromGameObject9 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild5, "Text"));
			Text textfromGameObject10 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild5, "Text (1)"));
			textfromGameObject9.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesVictoriaHolland1;
			textfromGameObject10.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesVictoriaHolland2;
			GameObject gameObjectChild6 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (23)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject11 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild6, "Text"));
			Text textfromGameObject12 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild6, "Text (1)"));
			textfromGameObject11.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesToniStigell1;
			textfromGameObject12.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesToniStigell2;
			GameObject gameObjectChild7 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (24)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject13 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild7, "Text"));
			Text textfromGameObject14 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild7, "Text (1)"));
			textfromGameObject13.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesFlyingdog1;
			textfromGameObject14.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesFlyingdog2;
			GameObject gameObjectChild8 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (22)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject15 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild8, "Text"));
			Text textfromGameObject16 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild8, "Text (1)"));
			textfromGameObject15.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesSamuelJamesBryan1;
			textfromGameObject16.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesSamuelJamesBryan2;
			GameObject gameObjectChild9 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (19)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject17 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild9, "Text"));
			Text textfromGameObject18 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild9, "Text (1)"));
			textfromGameObject17.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesCameronMartin1;
			textfromGameObject18.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesCameronMartin2;
			GameObject gameObjectChild10 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (16)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject19 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild10, "Text"));
			Text textfromGameObject20 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild10, "Text (1)"));
			textfromGameObject19.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesDaliaFigueroa1;
			textfromGameObject20.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesDaliaFigueroa2;
			GameObject gameObjectChild11 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (18)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject21 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild11, "Text"));
			Text textfromGameObject22 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild11, "Text (1)"));
			textfromGameObject21.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesTuckerWilkin1;
			textfromGameObject22.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesTuckerWilkin2;
			GameObject gameObjectChild12 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (17)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject23 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild12, "Text"));
			Text textfromGameObject24 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild12, "Text (1)"));
			textfromGameObject23.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesScottGurney1;
			textfromGameObject24.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesScottGurney2;
			GameObject gameObjectChild13 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (20)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject25 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild13, "Text"));
			Text textfromGameObject26 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild13, "Text (1)"));
			textfromGameObject25.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesAaronBurzynski1;
			textfromGameObject26.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesAaronBurzynski2;
			GameObject gameObjectChild14 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "dev Space Large (2)"), "Dev Large Placard"), "Canvas (4)");
			Text textfromGameObject27 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild14, "Text"));
			Text textfromGameObject28 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild14, "Text (1)"));
			textfromGameObject27.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesPitr1;
			textfromGameObject28.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesPitr2;
			GameObject gameObjectChild15 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "dev Space Large (2)"), "Dev Large Placard (1)"), "Canvas (4)");
			Text textfromGameObject29 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild15, "Text"));
			Text textfromGameObject30 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild15, "Text (1)"));
			textfromGameObject29.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesHeckteck1;
			textfromGameObject30.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesHeckteck2;
			GameObject gameObjectChild16 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (30)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject31 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild16, "Text"));
			Text textfromGameObject32 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild16, "Text (1)"));
			textfromGameObject31.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesHazeluff1;
			textfromGameObject32.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesHazeluff2;
			GameObject gameObjectChild17 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (10)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject33 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild17, "Text"));
			Text textfromGameObject34 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild17, "Text (1)"));
			textfromGameObject33.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesCabalcrow1;
			textfromGameObject34.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesCabalcrow2;
			GameObject gameObjectChild18 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (9)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject35 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild18, "Text"));
			Text textfromGameObject36 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild18, "Text (1)"));
			textfromGameObject35.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesLucasVarney1;
			textfromGameObject36.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesLucasVarney2;
			GameObject gameObjectChild19 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (1)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject37 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild19, "Text"));
			Text textfromGameObject38 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild19, "Text (1)"));
			textfromGameObject37.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesBenMoir1;
			textfromGameObject38.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesBenMoir2;
			GameObject gameObject5 = ((Component)CommonFunctions.GetInactiveRootObject("__Room_Large_Lower").transform.GetChild(4)).gameObject;
			Text textfromGameObject39 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObject5, "Wing name (4)"), "Canvas (5)"), "Text"));
			Text textfromGameObject40 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObject5, "Wing name (4)"), "Canvas (5)"), "Text (1)"));
			textfromGameObject39.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesDaveOshry1;
			textfromGameObject40.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesDaveOshry2;
			GameObject gameObject6 = ((Component)CommonFunctions.GetInactiveRootObject("__Room_Large_Lower").transform.GetChild(3)).gameObject;
			GameObject gameObject7 = ((Component)gameObject6.transform.GetChild(9)).gameObject;
			GameObject gameObject8 = ((Component)gameObject6.transform.GetChild(10)).gameObject;
			CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObject7, "Canvas (3)"), "Text")).text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesRestRoom;
			CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObject8, "Canvas (3)"), "Text")).text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesTalkRoom;
			GameObject gameObjectChild20 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (3)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject41 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild20, "Text"));
			Text textfromGameObject42 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild20, "Text (1)"));
			textfromGameObject41.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesMeganeko1;
			textfromGameObject42.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesMeganeko2;
			GameObject gameObjectChild21 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (2)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject43 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild21, "Text"));
			Text textfromGameObject44 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild21, "Text (1)"));
			textfromGameObject43.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesKeygenChurch1;
			textfromGameObject44.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesKeygenChurch2;
			GameObject gameObjectChild22 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (26)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject45 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild22, "Text"));
			Text textfromGameObject46 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild22, "Text (1)"));
			textfromGameObject45.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesHealth1;
			textfromGameObject46.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesHealth2;
			GameObject gameObjectChild23 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (27)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject47 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild23, "Text"));
			Text textfromGameObject48 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild23, "Text (1)"));
			textfromGameObject47.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesKingGizzard1;
			textfromGameObject48.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesKingGizzard2;
			GameObject gameObjectChild24 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (28)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject49 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild24, "Text"));
			Text textfromGameObject50 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild24, "Text (1)"));
			textfromGameObject49.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesQuetzalTirado1;
			textfromGameObject50.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesQuetzalTirado2;
			GameObject gameObjectChild25 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (8)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject51 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild25, "Text"));
			Text textfromGameObject52 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild25, "Text (1)"));
			textfromGameObject51.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesVyletPony1;
			textfromGameObject52.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesVyletPony2;
			GameObject gameObjectChild26 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (31)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject53 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild26, "Text"));
			Text textfromGameObject54 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild26, "Text (1)"));
			textfromGameObject53.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesKennadyRay1;
			textfromGameObject54.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesKennadyRay2;
			CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (25)"), "Dev Smalll placard (2)"), "Canvas (4)"), "Text")).text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesAdditionalArt;
			GameObject gameObjectChild27 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (4)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject55 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild27, "Text"));
			Text textfromGameObject56 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild27, "Text (1)"));
			textfromGameObject55.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesSalad1;
			textfromGameObject56.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesSalad2;
			GameObject gameObjectChild28 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (5)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject57 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild28, "Text"));
			Text textfromGameObject58 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild28, "Text (1)"));
			textfromGameObject57.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesJacobHHR1;
			textfromGameObject58.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesJacobHHR2;
			GameObject gameObjectChild29 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (6)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject59 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild29, "Text"));
			Text textfromGameObject60 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild29, "Text (1)"));
			textfromGameObject59.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesVVizard1;
			textfromGameObject60.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesVVizard2;
			GameObject gameObjectChild30 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (25)"), "Dev Smalll placard"), "Canvas (4)");
			GameObject gameObjectChild31 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (25)"), "Dev Smalll placard (1)"), "Canvas (4)");
			CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild30, "Text")).text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesAdditionalMusic;
			CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild31, "Text")).text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesAdditionalCredits;
			GameObject gameObjectChild32 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Prime 1 VA"), "Dev_Space_ (8)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject61 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild32, "Text"));
			Text textfromGameObject62 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild32, "Text (1)"));
			textfromGameObject61.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesStephanWeyte1;
			textfromGameObject62.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesStephanWeyte2;
			GameObject gameObjectChild33 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Prime 1 VA"), "SpoilerBlock"), "PuzzleScreen (1)"), "Canvas"), "Background");
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(gameObjectChild33, "Text"))).text = "<color=red>!" + LanguageManager.CurrentLanguage.devMuseum.museum_spoiler1 + " !</color>\n" + LanguageManager.CurrentLanguage.devMuseum.museum_spoiler2;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild33, "OpenButton"), "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_spoiler3;
			GameObject gameObjectChild34 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Prime 2 VA"), "Dev_Space_ (33)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject63 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild34, "Text"));
			Text textfromGameObject64 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild34, "Text (1)"));
			textfromGameObject63.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesLenvalBrown1;
			textfromGameObject64.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesLenvalBrown2;
			GameObject gameObjectChild35 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Prime 2 VA"), "SpoilerBlock"), "PuzzleScreen (1)"), "Canvas"), "Background");
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(gameObjectChild35, "Text"))).text = "<color=red>!" + LanguageManager.CurrentLanguage.devMuseum.museum_spoiler1 + " !</color>\n" + LanguageManager.CurrentLanguage.devMuseum.museum_spoiler2;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild35, "OpenButton"), "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_spoiler3;
			GameObject gameObjectChild36 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (7)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject65 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild36, "Text"));
			Text textfromGameObject66 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild36, "Text (1)"));
			textfromGameObject65.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesJoyYoung1;
			textfromGameObject66.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesJoyYoung2;
			GameObject gameObjectChild37 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "Dev_Space_ (21)"), "Dev Smalll placard"), "Canvas (4)");
			Text textfromGameObject67 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild37, "Text"));
			Text textfromGameObject68 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild37, "Text (1)"));
			textfromGameObject67.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesMandalore1;
			textfromGameObject68.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesMandalore2;
			GameObject gameObjectChild38 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(inactiveRootObject, "dev Space Large (1)"), "Dev Large Placard"), "Canvas (4)");
			Text textfromGameObject69 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild38, "Text"));
			Text textfromGameObject70 = CommonFunctions.GetTextfromGameObject(CommonFunctions.GetGameObjectChild(gameObjectChild38, "Text (1)"));
			textfromGameObject69.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesGianniMatragrano1;
			textfromGameObject70.text = LanguageManager.CurrentLanguage.devMuseum.museum_plaquesGianniMatragrano2;
			GameObject gameObjectChild39 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetInactiveRootObject("PuzzleScreen (2)"), "Canvas"), "Background"), "Start");
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(gameObjectChild39, "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_rocketRace1;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild39, "OpenButton"), "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_rocketRace2;
			GameObject gameObjectChild40 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetInactiveRootObject("__Room_Theater"), "Ultrakill Projector"), "PuzzleScreen"), "Canvas"), "Background");
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild40, "PlayButton"), "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_cinemaPlay;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild40, "StopButton"), "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_cinemaStop;
		}

		private void PatchChess()
		{
			GameObject gameObjectChild = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetInactiveRootObject("__Room_Aquarium"), "Geo"), "Chess");
			GameObject gameObjectChild2 = CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild, "PuzzleScreen"), "Canvas"), "Background"), "Main Window");
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(gameObjectChild2, "Versus Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessVs;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild2, "Start New Game Button"), "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessNewgame;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild2, "Black"), "Black Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessBlack;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild2, "White"), "White Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessWhite;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild2, "Black"), "Bot Button"), "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessBot;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild2, "Black"), "Player Button"), "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessPlayer;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild2, "White"), "Bot Button"), "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessBot;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild2, "White"), "Player Button"), "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessPlayer;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild2, "Settings"), "Main Window"), "Close Button"), "Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessSettingsclose;
			((TMP_Text)CommonFunctions.GetTextMeshProUGUI(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild2, "Settings"), "Main Window"), "Set Elo"), "Slider"), "Bot Text"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessBot + ":";
			((TMP_Text)GetTextMeshPro(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild, "WhiteWin"), "WinText"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessWhitewin;
			((TMP_Text)GetTextMeshPro(CommonFunctions.GetGameObjectChild(CommonFunctions.GetGameObjectChild(gameObjectChild, "BlackWin"), "WinText"))).text = LanguageManager.CurrentLanguage.devMuseum.museum_chessBlackwin;
		}

		private static TextMeshPro GetTextMeshPro(GameObject hogeobject)
		{
			if (!CommonFunctions.isUsingEnglish())
			{
				switch (LanguageManager.CurrentLanguage.metadata.langDisplayName)
				{
				case "Japanese":
				case "Traditional Chinese":
				case "Simplified Chinese":
					((TMP_Text)hogeobject.GetComponent<TextMeshPro>()).font = Core.CJKFontTMP;
					break;
				}
			}
			return hogeobject.GetComponent<TextMeshPro>();
		}

		public DevMuseum()
		{
			PatchPlaques();
			PatchChess();
		}
	}
}
