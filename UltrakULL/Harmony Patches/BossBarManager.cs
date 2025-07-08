using System.Collections.Generic;
using HarmonyLib;
using UltrakULL.json;

using static UltrakULL.CommonFunctions;

namespace UltrakULL.Harmony_Patches
{
    [HarmonyPatch]
    public static class LocalizeBossBar
    {
        // Патч для CreateBossBar
        [HarmonyPatch(typeof(BossBarManager), "CreateBossBar")]
        [HarmonyPrefix]
        public static void CreateBossBar_Prefix(BossHealthBar bossBar)
        {
            LocalizeName(bossBar);
        }

        // Патч для UpdateBossBar
        [HarmonyPatch(typeof(BossBarManager), "UpdateBossBar")]
        [HarmonyPrefix]
        public static void UpdateBossBar_Prefix(BossHealthBar bossBar)
        {
            LocalizeName(bossBar);
        }

        public static class BossNameHelper
        {
            public static bool IsAlreadyLocalized(string name)
            {
                var enemyNames = LanguageManager.CurrentLanguage.enemyNames;

                foreach (var field in enemyNames.GetType().GetFields())
                {
                    var value = field.GetValue(enemyNames)?.ToString();
                    if (!string.IsNullOrEmpty(value) && value == name)
                    {
                        return true; // Совпало — это перевод
                    }
                }

                return false; // Не совпало — значит оригинал
            }
        }

        private static readonly HashSet<string> loggedNames = new HashSet<string>();

        private static void LocalizeName(BossHealthBar bossBar)
        {
            if (!BossNameHelper.IsAlreadyLocalized(bossBar.bossName))
            {
                string translatedName = BossStrings.GetBossName(bossBar.bossName);
                if (!string.IsNullOrEmpty(translatedName))
                {
                    bossBar.bossName = translatedName;
                }
                else
                {
                    if (loggedNames.Add(bossBar.bossName)) // добавит и вернет true, если не было
                    {
                        Logging.Warn($"Boss name '{bossBar.bossName}' not found in localization. Using default.");
                    }
                }
            }
            else
            {
                if (loggedNames.Add(bossBar.bossName))
                {
                    Logging.Info($"Boss name '{bossBar.bossName}' is already localized. Skipping translation.");
                }
            }
        }
    }
}
