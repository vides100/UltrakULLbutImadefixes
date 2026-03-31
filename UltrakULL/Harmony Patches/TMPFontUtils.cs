using TMPro;
using UnityEngine;

namespace UltrakULL.Harmony_Patches
{
	public static class TMPFontUtils
	{
		public static void ApplyUnderlayAndZTest(TextMeshProUGUI instance, Vector4 underlayColor, bool isUnderlaid, bool isOverlay, bool editOverlayStatus, TMP_FontAsset fontAsset, Material overlayMat, Material normalMat)
		{




			((TMP_Text)instance).font = fontAsset;
			Material val = new Material((!editOverlayStatus) ? ((TMP_Text)instance).fontSharedMaterial : (isOverlay ? overlayMat : normalMat));
			if (isUnderlaid)
			{
				val.SetVector("_UnderlayColor", underlayColor);
			}
			else
			{
				val.SetVector("_UnderlayColor", new Vector4(0f, 0f, 0f, 0f));
			}
			if (editOverlayStatus)
			{
				val.SetFloat("_ZTest", isOverlay ? 8f : 4f);
			}
			((TMP_Text)instance).fontSharedMaterial = val;
		}
	}
}
