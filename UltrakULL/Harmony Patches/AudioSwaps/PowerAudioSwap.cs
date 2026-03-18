using System.IO;
using BepInEx.Configuration;
using HarmonyLib;
using UltrakULL.audio;
using UltrakULL.json;
using UnityEngine;

//Thanks to Lukah (Discord: lukahgb) for the tip

namespace UltrakULL.Harmony_Patches.AudioSwaps
{
	[HarmonyPatch(typeof(AudioSource), "set_clip")]
	public static class PowerAudioSwap
	{
		private static bool _isReplacing;

		private static bool _isExporting;

		private static void Postfix(AudioSource __instance, AudioClip value)
		{
			if (_isReplacing || _isExporting || (Object)(object)value == (Object)null || LanguageManager.configFile.Bind<string>("General", "activeDubbing", "False", (ConfigDescription)null).Value == "False" || CommonFunctions.isUsingEnglish())
			{
				return;
			}
			string name = ((Object)value).name;
			if (!name.StartsWith("pow_") && !name.StartsWith("power_"))
			{
				return;
			}
			string path = Path.Combine(AudioSwapper.SpeechFolder, "power");
			if (File.Exists(Path.Combine(path, name + ".ogg")))
			{
				_isReplacing = true;
				Logging.Info("[UltrakULL] Trying swap " + name);
				AudioClip val = AudioSwapper.SwapClipWithFile(value, Path.Combine(path, name));
				if ((Object)(object)val != (Object)null && (Object)(object)val != (Object)(object)value)
				{
					__instance.clip = val;
				}
				_isReplacing = false;
				return;
			}
			Logging.Info("[UltrakULL] Replacement not found for " + name + ", exporting original clip to WAV.");
			_isExporting = true;
			try
			{
				CommonFunctions.ExportAudioClipToWav(value, name);
			}
			finally
			{
				_isExporting = false;
			}
		}
	}
}
