using System;
using TMPro;
using UltrakULL.json;
using UnityEngine;
using static UltrakULL.CommonFunctions;


namespace UltrakULL
{
    public static class Encore
    {
        public static void PatchEncore(ref GameObject canvasObj)
        {
            string currentLevel = GetCurrentSceneName();
            string levelName = EncoreStrings.GetLevelName();
            string levelChallenge = EncoreStrings.GetLevelChallenge(currentLevel); //We leave the part with the challenges patch if the game developers decide to add them.

            PatchResultsScreen(levelName, levelChallenge);
            Logging.Info("Current level: " + currentLevel);

            if (currentLevel.Contains("0-E"))
            {
                try
                {
                    GameObject heatResistanceWindow = GetGameObjectChild(GetGameObjectChild(canvasObj, "HurtScreen"), "Heat Resistance");

                    TextMeshProUGUI heatResistanceWarn = GetTextMeshProUGUI(GetGameObjectChild(heatResistanceWindow, "Warning"));
                    heatResistanceWarn.enableWordWrapping = false;
                    heatResistanceWarn.text = LanguageManager.CurrentLanguage.encore.encorePrelude_heatResistanceWarn;

                    TextMeshProUGUI heatResistanceText = GetTextMeshProUGUI(GetGameObjectChild(heatResistanceWindow, "Flavor Text"));
                    heatResistanceText.text = LanguageManager.CurrentLanguage.encore.encorePrelude_heatResistanceText;

                    TextMeshProUGUI heatResistanceTitle = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(heatResistanceWindow, "Meter"), "Label"));
                    heatResistanceTitle.text = LanguageManager.CurrentLanguage.encore.encorePrelude_heatResistanceTitle;

                    GameObject heatResistanceFixedWindow = GetGameObjectChild(GetGameObjectChild(canvasObj, "HurtScreen"), "Heat Fixed");

                    TextMeshProUGUI heatResistanceFixedWarn = GetTextMeshProUGUI(GetGameObjectChild(heatResistanceFixedWindow, "Warning"));
                    heatResistanceFixedWarn.enableWordWrapping = false;
                    heatResistanceFixedWarn.text = LanguageManager.CurrentLanguage.encore.encorePrelude_heatResistanceRepaired;

                    TextMeshProUGUI heatResistanceFixedText = GetTextMeshProUGUI(GetGameObjectChild(heatResistanceFixedWindow, "Flavor Text"));
                    heatResistanceFixedText.text = LanguageManager.CurrentLanguage.encore.encorePrelude_heatResistanceRepairedText;

                    TextMeshProUGUI heatResistanceFixedTitle = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(heatResistanceFixedWindow, "Meter"), "Label"));
                    heatResistanceFixedTitle.text = LanguageManager.CurrentLanguage.encore.encorePrelude_heatResistanceTitle;
                }
                catch (Exception e)
                {
                    Logging.Error("Error patching 0-E: " + e.Message);

                }
            }
            if (currentLevel.Contains("1-E"))
            {
                try
                {
                    GameObject warningCanvas = GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(GetInactiveRootObject("11 - Skull Room"), "11 Nonstuff"), "Room"), "Cube (24)"), "Canvas");
                    TextMeshProUGUI warningText = GetTextMeshProUGUI(GetGameObjectChild(warningCanvas, "Text (TMP)"));
                    warningText.text = LanguageManager.CurrentLanguage.encore.encoreLimbo_warningText;
                }
                catch (Exception e)
                {
                    Logging.Error("Error patching 1-E: " + e.Message);

                }
            }
        }
    }
}

