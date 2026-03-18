using System;
using System.Collections.Generic;
using HarmonyLib;
using TMPro;
using UltrakULL.json;
using UnityEngine;

namespace UltrakULL.Harmony_Patches
{
	public class TextMeshProFontSwap
	{
		[HarmonyPatch(typeof(TextMeshProUGUI), "OnEnable")]
		public static class TextMeshProFontSwapper
		{
			private static List<IntPtr> objectsFixed = new List<IntPtr>();

			[HarmonyPostfix]
			public static void SwapFont(ref TextMeshProUGUI __instance, IntPtr ___m_CachedPtr)
			{
				if ((objectsFixed.Count <= 0 || !objectsFixed.Contains(___m_CachedPtr)) && Core.TMPFontReady && (!CommonFunctions.isUsingEnglish() || !(CommonFunctions.GetCurrentSceneName() != "Main Menu")))
				{
					SwapTMPFont(ref __instance);
					objectsFixed.Add(___m_CachedPtr);
				}
			}
		}

		[HarmonyPatch(typeof(HudController))]
		public static class HudControllerPatch
		{
			public static bool isOverlaid = MonoSingleton<PrefsManager>.Instance.GetBool("hudAlwaysOnTop", false);

			[HarmonyPatch("SetAlwaysOnTop")]
			[HarmonyPrefix]
			public static bool SetAlwaysOnTop_Prefix(ref TMP_Text[] ___textElements, bool onTop, Material ___overlayTextMaterial, Material ___normalTextMaterial)
			{
				if (CommonFunctions.isUsingEnglish())
				{
					return true;
				}
				isOverlaid = onTop;
				if (___textElements.Length != 0)
				{
					TMP_Text[] array = ___textElements;
					foreach (TMP_Text val in array)
					{
						if (((Component)val.transform.parent).GetComponent<HealthBar>() != null && ((Component)val).gameObject.name.Equals("HP Text"))
						{
							val.fontSharedMaterial = (isOverlaid ? ___overlayTextMaterial : ___normalTextMaterial);
							continue;
						}
						TextMeshProUGUI __instance = ((Component)val).GetComponent<TextMeshProUGUI>();
						SwapTMPFont(ref __instance, isOverlaid, editOverlayStatus: true);
					}
				}
				return false;
			}
		}

		[HarmonyPatch(typeof(SubtitleController))]
		public static class SubtitleFontSwapper
		{
			[HarmonyPatch("DisplaySubtitle", new Type[]
			{
				typeof(string),
				typeof(AudioSource),
				typeof(bool)
			})]
			[HarmonyPrefix]
			public static bool SubtitlePostfix(SubtitleController __instance, string caption, AudioSource audioSource, bool ignoreSetting, Subtitle ___subtitleLine, Transform ___container, Subtitle ___previousSubtitle)
			{
				if (!__instance.SubtitlesEnabled && !ignoreSetting)
				{
					return false;
				}
				Subtitle val = UnityEngine.Object.Instantiate<Subtitle>(___subtitleLine, ___container, true);
				((Component)val).GetComponentInChildren<TMP_Text>().text = caption;
				TextMeshProUGUI __instance2 = ((Component)val).GetComponentInChildren<TextMeshProUGUI>();
				if (Core.TMPFontReady)
				{
					SwapTMPFont(ref __instance2);
				}
				if (audioSource != null)
				{
					val.distanceCheckObject = audioSource;
				}
				((Component)val).gameObject.SetActive(true);
				if (___previousSubtitle == null)
				{
					val.ContinueChain();
				}
				else
				{
					___previousSubtitle.nextInChain = val;
				}
				___previousSubtitle = val;
				return false;
			}
		}

		public static void SwapTMPFont(ref TextMeshProUGUI __instance, bool onTop = false, bool editOverlayStatus = false)
		{
			if (((Component)((TMP_Text)__instance).transform.parent).GetComponent<HealthBar>() != null && ((Component)__instance).gameObject.name.Equals("HP Text"))
			{
				return;
			}
			string text = null;
			if (((TMP_Text)__instance).transform.parent != null && ((TMP_Text)__instance).transform.parent.parent != null)
			{
				text = ((Component)((TMP_Text)__instance).transform.parent.parent).gameObject.name + "/" + ((Component)((TMP_Text)__instance).transform.parent).gameObject.name + "/" + ((Component)((TMP_Text)__instance).transform).gameObject.name;
			}
			string text2 = LanguageManager.CurrentLanguage.metadata.langName.ToLower().Substring(0, 2);
			bool isUnderlaid = ((Component)__instance).gameObject.name.Contains("NameText") || ((Component)__instance).gameObject.name.Contains("LayerText") || ((Component)((TMP_Text)__instance).transform.parent).gameObject.name.Contains("Cheats Info") || (text?.Equals("ReadingScanned/Panel/Text (1)") ?? false);
			bool isOverlay = onTop;
			Vector4 underlayColor = (Vector4)(((TMP_Text)__instance).fontMaterial != null ? ((TMP_Text)__instance).fontMaterial.GetVector("_UnderlayColor") : new Vector4(0f, 0f, 0f, 0f));
			switch (text2)
			{
			case "zh":
				TMPFontUtils.ApplyUnderlayAndZTest(__instance, underlayColor, isUnderlaid, isOverlay, editOverlayStatus, Core.CJKFontTMP, Core.CJKFontTMPOverlayMat, ((TMP_Asset)Core.CJKFontTMP).material);
				break;
			case "ja":
				TMPFontUtils.ApplyUnderlayAndZTest(__instance, underlayColor, isUnderlaid, isOverlay, editOverlayStatus, Core.JaFontTMP, Core.jaFontTMPOverlayMat, ((TMP_Asset)Core.JaFontTMP).material);
				break;
			case "ar":
			case "fa":
			case "ur":
			{
				TextAlignmentOptions alignment = ((TMP_Text)__instance).alignment;
				if ((int)alignment <= 513)
				{
					if ((int)alignment != 257)
					{
						if ((int)alignment == 513)
						{
							((TMP_Text)__instance).alignment = (TextAlignmentOptions)516;
						}
					}
					else
					{
						((TMP_Text)__instance).alignment = (TextAlignmentOptions)260;
					}
				}
				else if ((int)alignment != 1025)
				{
					if ((int)alignment == 2049)
					{
						((TMP_Text)__instance).alignment = (TextAlignmentOptions)2052;
					}
				}
				else
				{
					((TMP_Text)__instance).alignment = (TextAlignmentOptions)1028;
				}
				Core.GlobalFontTMP.fallbackFontAssetTable.Add(Core.ArabicFontTMP);
				if (CommonFunctions.GetCurrentSceneName() == "CreditsMuseum2" && ((TMP_Text)__instance).font.name == "GFS Garaldus")
				{
					TMPFontUtils.ApplyUnderlayAndZTest(__instance, underlayColor, isUnderlaid, isOverlay, editOverlayStatus, Core.MuseumFontTMP, Core.GlobalFontTMPOverlayMat, ((TMP_Asset)Core.GlobalFontTMP).material);
				}
				else
				{
					TMPFontUtils.ApplyUnderlayAndZTest(__instance, underlayColor, isUnderlaid, isOverlay, editOverlayStatus, Core.GlobalFontTMP, Core.GlobalFontTMPOverlayMat, ((TMP_Asset)Core.GlobalFontTMP).material);
				}
				break;
			}
			case "jr":
			case "he":
			case "yi":
			case "la":
			case "ro":
				TMPFontUtils.ApplyUnderlayAndZTest(__instance, underlayColor, isUnderlaid, isOverlay, editOverlayStatus, Core.HebrewFontTMP, Core.GlobalFontTMPOverlayMat, ((TMP_Asset)Core.GlobalFontTMP).material);
				break;
			default:
				if (CommonFunctions.GetCurrentSceneName() == "CreditsMuseum2" && ((TMP_Text)__instance).font.name == "GFS Garaldus")
				{
					TMPFontUtils.ApplyUnderlayAndZTest(__instance, underlayColor, isUnderlaid, isOverlay, editOverlayStatus, Core.MuseumFontTMP, Core.GlobalFontTMPOverlayMat, ((TMP_Asset)Core.GlobalFontTMP).material);
				}
				else
				{
					TMPFontUtils.ApplyUnderlayAndZTest(__instance, underlayColor, isUnderlaid, isOverlay, editOverlayStatus, Core.GlobalFontTMP, Core.GlobalFontTMPOverlayMat, ((TMP_Asset)Core.GlobalFontTMP).material);
				}
				break;
			}
		}
	}
}
