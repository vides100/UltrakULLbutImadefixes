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
    //Changing the mission name when calling the original GetMissionName function
    [HarmonyPatch(typeof(GetMissionName))]
    public static class Patch_GetMissionName
    {
        [HarmonyPatch("GetMission")]
        [HarmonyPrefix]
        public static bool Prefix_GetMission(int missionNum, ref string __result)
        {
            if (isUsingEnglish())
            {
                return true;
            }
            if (SceneHelper.IsPlayingCustom)
            {
                __result = MapInfoBase.Instance?.levelName ?? "Unknown Level";
                return false; // Abort the original method
            }

            // Getting the modified values from the subpatched functions
            string missionNumber = GetMissionNumberOnly_Patched(missionNum);
            string missionName = GetMissionNameOnly_Patched(missionNum);

            // RTL support
            if (LanguageManager.IsRightToLeft)
            {
                // This is a new block that changes the "mission numbers". It has not been tested. If it causes bugs or is unnecessary, please remove it.
                string lvlNumber;
                if (missionNumber.Contains("-"))
                {
                    // For formats such as "P-1", "0-E", etc., leave it as is
                    lvlNumber = missionNumber;
                }
                else if (!string.IsNullOrEmpty(missionNumber))
                {
                    // For numeric formats, reverse the order of digits
                    char[] lnum = missionNumber.ToCharArray();
                    lvlNumber = new string(lnum.Reverse().ToArray());
                }
                else
                {
                    // If the mission number is empty
                    lvlNumber = "";
                }

                string lvlTitle = missionName;

                if (missionNum == 0)
                {
                    __result = LanguageManager.CurrentLanguage.levelNames.levelName_mainMenu;
                }
                else
                {
                    __result = $"{lvlTitle} :{lvlNumber}";
                }
            }
            else
            {
                if (missionNum == 0)
                {
                    __result = LanguageManager.CurrentLanguage.levelNames.levelName_mainMenu;
                }
                else
                {
                    __result = $"{missionNumber}: {missionName}";
                }
            }

            return false; // Completely replacing the original method
        }

        // Wrapper method for GetMissionNumberOnly patch
        private static string GetMissionNumberOnly_Patched(int missionNum)
        {
            string result = GetMissionName.GetMissionNumberOnly(missionNum);
            Prefix_GetMissionNumberOnly(missionNum, ref result);
            return result;
        }

        // Wrapper method for GetMissionNameOnly patch
        private static string GetMissionNameOnly_Patched(int missionNum)
        {
            string result = GetMissionName.GetMissionNameOnly(missionNum);
            Prefix_GetMissionNameOnly(missionNum, ref result);
            return result;
        }

        [HarmonyPatch("GetMissionNumberOnly")]
        [HarmonyPrefix]
        public static bool Prefix_GetMissionNumberOnly(int missionNum, ref string __result)
        {
            if (SceneHelper.IsPlayingCustom)
            {
                __result = "";
                return false;
            }

            if (LanguageManager.UsingHinduNumbers)
            {
                string missionNumber;
                switch (missionNum)
                {
                    case 1: missionNumber = "0-1"; break;
                    case 2: missionNumber = "0-2"; break;
                    case 3: missionNumber = "0-3"; break;
                    case 4: missionNumber = "0-4"; break;
                    case 5: missionNumber = "0-5"; break;
                    case 6: missionNumber = "1-1"; break;
                    case 7: missionNumber = "1-2"; break;
                    case 8: missionNumber = "1-3"; break;
                    case 9: missionNumber = "1-4"; break;
                    case 10: missionNumber = "2-1"; break;
                    case 11: missionNumber = "2-2"; break;
                    case 12: missionNumber = "2-3"; break;
                    case 13: missionNumber = "2-4"; break;
                    case 14: missionNumber = "3-1"; break;
                    case 15: missionNumber = "3-2"; break;
                    case 16: missionNumber = "4-1"; break;
                    case 17: missionNumber = "4-2"; break;
                    case 18: missionNumber = "4-3"; break;
                    case 19: missionNumber = "4-4"; break;
                    case 20: missionNumber = "5-1"; break;
                    case 21: missionNumber = "5-2"; break;
                    case 22: missionNumber = "5-3"; break;
                    case 23: missionNumber = "5-4"; break;
                    case 24: missionNumber = "6-1"; break;
                    case 25: missionNumber = "6-2"; break;
                    case 26: missionNumber = "7-1"; break;
                    case 27: missionNumber = "7-2"; break;
                    case 28: missionNumber = "7-3"; break;
                    case 29: missionNumber = "7-4"; break;
                    case 30: missionNumber = "8-1"; break;
                    case 31: missionNumber = "8-2"; break;
                    case 32: missionNumber = "8-3"; break;
                    case 33: missionNumber = "8-4"; break;
                    case 34: missionNumber = "9-1"; break;
                    case 35: missionNumber = "9-2"; break;
                    case 100: missionNumber = "0-E"; break;
                    case 101: missionNumber = "1-E"; break;
                    case 102: missionNumber = "2-E"; break;
                    case 103: missionNumber = "3-E"; break;
                    case 104: missionNumber = "4-E"; break;
                    case 105: missionNumber = "5-E"; break;
                    case 106: missionNumber = "6-E"; break;
                    case 107: missionNumber = "7-E"; break;
                    case 108: missionNumber = "8-E"; break;
                    case 109: missionNumber = "9-E"; break;
                    case 666: missionNumber = "P-1"; break;
                    case 667: missionNumber = "P-2"; break;
                    case 668: missionNumber = "P-3"; break;
                    default: missionNumber = ""; break;
                }

                __result = ConvertToHinduNumbers(missionNumber);
                return false;
            }

            return true; // The original function is performed if Hindu digits are not enabled
        }
        // Method for converting numbers to Hindu format
        private static string ConvertToHinduNumbers(string input)
        {
            return input
                .Replace('0', '٠')
                .Replace('1', '١')
                .Replace('2', '٢')
                .Replace('3', '٣')
                .Replace('4', '٤')
                .Replace('5', '٥')
                .Replace('6', '٦')
                .Replace('7', '٧')
                .Replace('8', '٨')
                .Replace('9', '٩');
        }

        [HarmonyPatch("GetMissionNameOnly")]
        [HarmonyPrefix]
        public static bool Prefix_GetMissionNameOnly(int missionNum, ref string __result)
        {
            if (SceneHelper.IsPlayingCustom)
            {
                __result = MapInfoBase.Instance?.levelName ?? "Unknown Level";
                return false;
            }

            // Custom mission name translation/replacement logic
            switch (missionNum)
            {
                case 0: __result = LanguageManager.CurrentLanguage.levelNames.levelName_mainMenu; return false;
                case 1: __result = LanguageManager.CurrentLanguage.levelNames.levelName_preludeFirst; return false;
                case 2: __result = LanguageManager.CurrentLanguage.levelNames.levelName_preludeSecond; return false;
                case 3: __result = LanguageManager.CurrentLanguage.levelNames.levelName_preludeThird; return false;
                case 4: __result = LanguageManager.CurrentLanguage.levelNames.levelName_preludeFourth; return false;
                case 5: __result = LanguageManager.CurrentLanguage.levelNames.levelName_preludeFifth; return false;
                case 6: __result = LanguageManager.CurrentLanguage.levelNames.levelName_limboFirst; return false;
                case 7: __result = LanguageManager.CurrentLanguage.levelNames.levelName_limboSecond; return false;
                case 8: __result = LanguageManager.CurrentLanguage.levelNames.levelName_limboThird; return false;
                case 9: __result = LanguageManager.CurrentLanguage.levelNames.levelName_limboFourth; return false;
                case 10: __result = LanguageManager.CurrentLanguage.levelNames.levelName_lustFirst; return false;
                case 11: __result = LanguageManager.CurrentLanguage.levelNames.levelName_lustSecond; return false;
                case 12: __result = LanguageManager.CurrentLanguage.levelNames.levelName_lustThird; return false;
                case 13: __result = LanguageManager.CurrentLanguage.levelNames.levelName_lustFourth; return false;
                case 14: __result = LanguageManager.CurrentLanguage.levelNames.levelName_gluttonyFirst; return false;
                case 15: __result = LanguageManager.CurrentLanguage.levelNames.levelName_gluttonySecond; return false;
                case 16: __result = LanguageManager.CurrentLanguage.levelNames.levelName_greedFirst; return false;
                case 17: __result = LanguageManager.CurrentLanguage.levelNames.levelName_greedSecond; return false;
                case 18: __result = LanguageManager.CurrentLanguage.levelNames.levelName_greedThird; return false;
                case 19: __result = LanguageManager.CurrentLanguage.levelNames.levelName_greedFourth; return false;
                case 20: __result = LanguageManager.CurrentLanguage.levelNames.levelName_wrathFirst; return false;
                case 21: __result = LanguageManager.CurrentLanguage.levelNames.levelName_wrathSecond; return false;
                case 22: __result = LanguageManager.CurrentLanguage.levelNames.levelName_wrathThird; return false;
                case 23: __result = LanguageManager.CurrentLanguage.levelNames.levelName_wrathFourth; return false;
                case 24: __result = LanguageManager.CurrentLanguage.levelNames.levelName_heresyFirst; return false;
                case 25: __result = LanguageManager.CurrentLanguage.levelNames.levelName_heresySecond; return false;
                case 26: __result = LanguageManager.CurrentLanguage.levelNames.levelName_violenceFirst; return false;
                case 27: __result = LanguageManager.CurrentLanguage.levelNames.levelName_violenceSecond; return false;
                case 28: __result = LanguageManager.CurrentLanguage.levelNames.levelName_violenceThird; return false;
                case 29: __result = LanguageManager.CurrentLanguage.levelNames.levelName_violenceFourth; return false;
                case 30: __result = LanguageManager.CurrentLanguage.levelNames.levelName_fraudFirst; return false;
                case 31: __result = LanguageManager.CurrentLanguage.levelNames.levelName_fraudSecond; return false;
                case 32: __result = LanguageManager.CurrentLanguage.levelNames.levelName_fraudThird; return false;
                case 33: __result = LanguageManager.CurrentLanguage.levelNames.levelName_fraudFourth; return false;
                case 34: __result = LanguageManager.CurrentLanguage.levelNames.levelName_treacheryFirst; return false;
                case 35: __result = LanguageManager.CurrentLanguage.levelNames.levelName_treacherySecond; return false;
                case 100: __result = LanguageManager.CurrentLanguage.levelNames.levelName_encorePrelude; return false;
                case 101: __result = LanguageManager.CurrentLanguage.levelNames.levelName_encoreLimbo; return false;
                case 102: __result = LanguageManager.CurrentLanguage.levelNames.levelName_encoreLust; return false;
                case 103: __result = LanguageManager.CurrentLanguage.levelNames.levelName_encoreGluttony; return false;
                case 104: __result = LanguageManager.CurrentLanguage.levelNames.levelName_encoreGreed; return false;
                case 105: __result = LanguageManager.CurrentLanguage.levelNames.levelName_encoreWrath; return false;
                case 106: __result = LanguageManager.CurrentLanguage.levelNames.levelName_encoreHeresy; return false;
                case 107: __result = LanguageManager.CurrentLanguage.levelNames.levelName_encoreViolence; return false;
                case 108: __result = LanguageManager.CurrentLanguage.levelNames.levelName_encoreFraud; return false;
                case 109: __result = LanguageManager.CurrentLanguage.levelNames.levelName_encoreTreachery; return false;
                case 666: __result = LanguageManager.CurrentLanguage.levelNames.levelName_primeFirst; return false;
                case 667: __result = LanguageManager.CurrentLanguage.levelNames.levelName_primeSecond; return false;
                case 668: __result = LanguageManager.CurrentLanguage.levelNames.levelName_primeThird; return false;
                default: __result = ""; return true;
            }
        }
    }

}
