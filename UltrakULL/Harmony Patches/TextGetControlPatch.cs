using System;
using HarmonyLib;
using TMPro;
using UltrakULL.json;
using UnityEngine;
using UnityEngine.InputSystem;
using static UltrakULL.CommonFunctions;

namespace UltrakULL.Harmony_Patches
{
    [HarmonyPatch(typeof(TextGetControl), "Start")]
    public static class TextGetControlPatch
    {
        private static string LocalizeBindingString(string bindingString)
        {
            if (string.IsNullOrEmpty(bindingString))
                return bindingString;

            // Если используется английский язык, перевод не требуется
            if (isUsingEnglish())
                return bindingString;

            // Разделяем по " + " (с пробелами)
            string[] parts = bindingString.Split(new[] { " + " }, StringSplitOptions.None);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = GetLocalizedInput(parts[i].Trim());
            }
            return string.Join(" + ", parts);
        }

        [HarmonyPostfix]
        public static void TextGetControlStart_Postfix(TextGetControl __instance, ref TMP_Text ___text)
        {
            if (___text == null)
                return;

            ___text.text = LocalizeBindingString(___text.text);
        }
    }
}