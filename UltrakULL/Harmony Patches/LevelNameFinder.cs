using System;
using HarmonyLib;
using TMPro;
using UltrakULL.json;
using UnityEngine.UI;

using static UltrakULL.CommonFunctions;

namespace UltrakULL.Harmony_Patches
{
    [HarmonyPatch(typeof(LevelNameFinder), "OnEnable")]
    public static class LevelNameFinderTranslation
    {
        [HarmonyPostfix]
        public static void OnEnable_Postfix(LevelNameFinder __instance, Text ___txt, TMP_Text ___txt2)
        {
            if(!isUsingEnglish())
            {
                //Now we dont need to check if the scene name contains "-E"
                {
                    ___txt2.text = "<color=red>" + LanguageManager.CurrentLanguage.shop.shop_returningTo +
                                   ":</color>\n" + LevelNames.GetLevelName(__instance.otherLevelNumber);
                }

            }
        }
    }
}
