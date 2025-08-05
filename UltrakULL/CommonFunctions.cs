using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using TMPro;
using UltrakULL.json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UltrakULL
{
	public static class CommonFunctions
	{

    private static readonly Dictionary<string, string> LocalizedInputs = new Dictionary<string, string>()
		{
			{ "Space", LanguageManager.CurrentLanguage.inputStrings.input_space },
			{ "Enter", LanguageManager.CurrentLanguage.inputStrings.input_enter },
			{ "Tab", LanguageManager.CurrentLanguage.inputStrings.input_tab },
			{ "Esc", LanguageManager.CurrentLanguage.inputStrings.input_esc },
			{ "Left Shift", LanguageManager.CurrentLanguage.inputStrings.input_leftShift },
			{ "Right Shift", LanguageManager.CurrentLanguage.inputStrings.input_rightShift },
			{ "Left Control", LanguageManager.CurrentLanguage.inputStrings.input_leftControl },
			{ "Right Control", LanguageManager.CurrentLanguage.inputStrings.input_rightControl },
			{ "Left Alt", LanguageManager.CurrentLanguage.inputStrings.input_leftAlt },
			{ "Right Alt", LanguageManager.CurrentLanguage.inputStrings.input_rightAlt },
			{ "LMB", LanguageManager.CurrentLanguage.inputStrings.input_LMB },
			{ "RMB", LanguageManager.CurrentLanguage.inputStrings.input_RMB },
			{ "MMB", LanguageManager.CurrentLanguage.inputStrings.input_MMB },
			{ "Up Arrow", LanguageManager.CurrentLanguage.inputStrings.input_arrowUp },
			{ "Down Arrow", LanguageManager.CurrentLanguage.inputStrings.input_arrowDown },
			{ "Left Arrow", LanguageManager.CurrentLanguage.inputStrings.input_arrowLeft },
			{ "Right Arrow", LanguageManager.CurrentLanguage.inputStrings.input_arrowRight },
            { "Forward", LanguageManager.CurrentLanguage.inputStrings.input_forward },
            { "Back", LanguageManager.CurrentLanguage.inputStrings.input_back },
            { "NO BINDING", LanguageManager.CurrentLanguage.inputStrings.input_noBinding },
        };

        public static string GetLocalizedInput(string input)
        {
            if (input.Length == 1 && char.IsLetter(input[0]))
                return input;

			Logging.Message("[GetLocalizedInput] Input in Message = " + input);
            if (LocalizedInputs.TryGetValue(input, out string localized))
                return localized;

            return input;
        }

        public static bool isUsingEnglish()
		{
			return (LanguageManager.CurrentLanguage.metadata.langDisplayName == "English");
		}
		
		public static ColorBlock UkButtonColors = new ColorBlock()
		{
			normalColor = new Color(0, 0, 0, 0.512f),
			highlightedColor = new Color(1, 1, 1, 0.502f),
			pressedColor = new Color(1, 0, 0, 1),
			selectedColor = new Color(0, 0, 0, 0.512f),
			disabledColor = new Color(0.7843f, 0.7843f, 0.7843f, 0.502f),
			colorMultiplier = 1f,
			fadeDuration = 0.1f
		};
		
		public static string PreviousHudMessage;
		
		public static IEnumerator WaitforSeconds(float seconds)
		{
			yield return new WaitForSeconds(seconds);
		}

		public static void HandleError(Exception e, string missingID = "")
		{  
			Logging.Error(e.ToString());
		}

		public static GameObject GetInactiveRootObject(string objectName)
		{
			List<GameObject> rootList = new List<GameObject>();
			SceneManager.GetActiveScene().GetRootGameObjects(rootList);
			foreach (GameObject child in rootList)
			{
				if (child.name == objectName)
				{
					return child;
				}
			}
			return null;
		}
		
		public static string GetCurrentSceneName()
		{
			return SceneHelper.CurrentScene;
		}
		
		//NOTE - below code was borrowed from ZedDev's UKUIHelper, but with some things modified/removed to prevent errors.
		
		public static GameObject CreateButton(string buttonText = "Text",string buttonName = "Button")
		{
		
			ColorBlock colors = new ColorBlock()
			{
				normalColor = new Color(0,0,0,0.512f),
				highlightedColor = new Color(1,1,1,0.502f),
				pressedColor = new Color(1,0,0,1),
				selectedColor = new Color(0,0,0,0.512f),
				disabledColor = new Color(0.7843f,0.7843f,0.7843f,0.502f),
				colorMultiplier = 1f,
				fadeDuration = 0.1f
			};
		
		  GameObject button = new GameObject();
		  button.name = buttonName;
		  button.AddComponent<RectTransform>();
		  button.AddComponent<CanvasRenderer>();
		  button.AddComponent<Image>();
		  button.AddComponent<Button>();
		  button.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
		  button.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
		  button.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
		  //button.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
		  button.GetComponent<Image>().type = Image.Type.Sliced;
		  button.GetComponent<Button>().targetGraphic = button.GetComponent<Image>();
		  GameObject text = CreateText();
		  button.GetComponent<Button>().colors = colors;

		  text.name = "Text";
		  text.GetComponent<RectTransform>().SetParent(button.GetComponent<RectTransform>());
		  text.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
		  text.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
		  text.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

		  text.GetComponent<Text>().text = buttonText;
		  text.GetComponent<Text>().font = Core.VcrFont;
		  text.GetComponent<Text>().fontSize = 32;
		  text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
		  text.GetComponent<Text>().color = Color.white;
		  return button;
		}
		
		public static GameObject CreateText() //Obsolete
		{
			GameObject text = new GameObject();
			text.name = "Text";
			text.AddComponent<RectTransform>();
			text.AddComponent<CanvasRenderer>();
			text.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
			text.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
			text.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
			//text.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
			text.AddComponent<Text>();
			text.GetComponent<Text>().text = "Text";
			text.GetComponent<Text>().font = Core.VcrFont;
			text.GetComponent<Text>().fontSize = 32;
			text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
			text.GetComponent<Text>().color = Color.black;
			return text;
		}

		public static void PatchResultsScreen(string name, string challenge)
		{
			string levelName = name;
			string levelChallenge = challenge;

			GameObject coreGame = GameObject.Find("Player");

			GameObject resultsPanel = GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(coreGame, "Main Camera"), "HUD Camera"), "HUD"), "FinishCanvas"), "Panel"); 

			//Level title
			GameObject resultsTitle = GetGameObjectChild(resultsPanel, "Title");
			TextMeshProUGUI resultsTitleLevelName = GetTextMeshProUGUI(GetGameObjectChild(resultsTitle, "Text"));
			resultsTitleLevelName.text = levelName;

			//Disable the levelFinderComponent, so the level name doesn't get reverted when the results panel appears.
			LevelNameFinder finder = resultsTitleLevelName.GetComponent<LevelNameFinder>();
			if (finder != null)
			{
				finder.enabled = false;
			}

			//Time
			//For some bizzare reason, the timer is labelled as "ff". Hakita were you cutting corners? :D
			GameObject timeTitle = GetGameObjectChild(resultsPanel, "ff");
			TextMeshProUGUI timeTitleText = GetTextMeshProUGUI(GetGameObjectChild(timeTitle, "Text"));
			timeTitleText.text = LanguageManager.CurrentLanguage.misc.stats_time;

			//Kills
			GameObject killsTitle = GetGameObjectChild(resultsPanel, "Kills - Info");
			TextMeshProUGUI killsTitleText = GetTextMeshProUGUI(GetGameObjectChild(killsTitle, "Text"));
			killsTitleText.text = LanguageManager.CurrentLanguage.misc.stats_kills;

			//Style
			GameObject styleTitle = GetGameObjectChild(resultsPanel, "Style - Info");
			TextMeshProUGUI styleTitleText = GetTextMeshProUGUI(GetGameObjectChild(styleTitle, "Text"));
			styleTitleText.text = LanguageManager.CurrentLanguage.misc.stats_style;

			//Secrets
			GameObject secretsTitle = GetGameObjectChild(resultsPanel, "Secrets -  Title");
			TextMeshProUGUI secretsTitleText = GetTextMeshProUGUI(GetGameObjectChild(secretsTitle, "Text"));
			secretsTitleText.text = LanguageManager.CurrentLanguage.misc.stats_secrets;

			//Challenge title
			GameObject challengeTitle = GetGameObjectChild(resultsPanel, "Challenge - Title");
			TextMeshProUGUI challengeTitleText = GetTextMeshProUGUI(GetGameObjectChild(challengeTitle, "Text"));
			challengeTitleText.text = LanguageManager.CurrentLanguage.misc.stats_challenge;

			//Challenge description
			GameObject challengeDescription = GetGameObjectChild(resultsPanel, "Challenge");
			TextMeshProUGUI challengeDescriptionText = GetTextMeshProUGUI(GetGameObjectChild(challengeDescription, "ChallengeText"));
			challengeDescriptionText.text = levelChallenge;

			//Total points
			TextMeshProUGUI totalPointsText = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(resultsPanel, "Total Points"),"Text (1)"));
			totalPointsText.text = LanguageManager.CurrentLanguage.cyberGrind.cybergrind_total + ":";
		}


        public static GameObject GetGameObjectChild(GameObject parentObject, string childToFind)
        {
            if (parentObject == null)
            {
                Logging.Error($"[GetGameObjectChild] ERROR - parentObject is NULL (looking for '{childToFind}')");
                return null;
            }

            Transform parentTransform = parentObject.transform;
            Transform directChild = null;

            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);
                if (child.name == childToFind)
                {
                    directChild = child;
                    break;
                }
            }

            if (directChild == null)
            {
                Logging.Error($"[GetGameObjectChild] ERROR - Direct child '{childToFind}' not found under '{parentObject.name}'");
                Logging.Info($"[GetGameObjectChild] Direct children of '{parentObject.name}':");
                PrintChildrenTree(parentTransform, 1);
                return null;
            }

            return directChild.gameObject;
        }

        public static Transform RecursiveFindChild(Transform parent, string childName)
        {
            foreach (Transform child in parent)
            {
                if (child.name == childName)
                    return child;

                Transform result = RecursiveFindChild(child, childName);
                if (result != null)
                    return result;
            }
            return null;
        }
        private static void PrintChildrenTree(Transform parent, int indentLevel)
        {
            string indent = new string(' ', indentLevel * 2);

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                Logging.Info($"{indent}- {child.name}");
                if (child.childCount > 0)
                {
                    PrintChildrenTree(child, indentLevel + 1);
                }
            }
        }

        public static Text GetTextfromGameObject(GameObject objectToUse)
		{
			return objectToUse.GetComponent<Text>();
		}

        public static TextMeshProUGUI GetTextMeshProUGUI(GameObject objectToUse)
        {
            if (objectToUse == null)
            {
                Logging.Error("[GetTextMeshProUGUI] GameObject is NULL");
                return null;
            }

            TextMeshProUGUI tmp = objectToUse.GetComponent<TextMeshProUGUI>();
            if (tmp == null)
                Logging.Warn($"[GetTextMeshProUGUI] '{objectToUse.name}' not have any TextMeshProUGUI's");

            return tmp;
        }

        public static IEnumerable<CodeInstruction> IL(params (OpCode, object)[] instructions)
		{
			return instructions.Select(i => new CodeInstruction(i.Item1, i.Item2)).ToList();
		}

        public static GameObject GetObject(string path)
        {
            string rootPath, restPath = null;

            if (!path.Contains('/'))
                rootPath = path;
            else
            {
                var pathParts = path.Split(new[] { '/' }, 2);
                rootPath = pathParts[0];
                restPath = pathParts[1];
            }

            // Get ALL root objects, even inactive ones
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var root in roots)
            {
                if (root.name != rootPath)
                    continue;

                if (restPath == null)
                    return root;

                var result = FindChildRecursive(root.transform, restPath);
                if (result != null)
                    return result.gameObject;
            }

            return null;
        }

        private static Transform FindChildRecursive(Transform parent, string path)
        {

            // Support for paths of the form "A/B/C"
            var split = path.Split('/');
            return FindRecursiveInternal(parent, split, 0);
        }

        private static Transform FindRecursiveInternal(Transform current, string[] split, int index)
        {
            if (current == null || index >= split.Length)
                return current;

            var child = current.Find(split[index]);
            if (child == null)
            {

                // Trying to manually traverse all children (in case the object is disabled)
                foreach (Transform t in current)
                {
                    if (t.name == split[index])
                    {
                        child = t;
                        break;
                    }
                }
            }

            return FindRecursiveInternal(child, split, index + 1);
        }
    }
}
