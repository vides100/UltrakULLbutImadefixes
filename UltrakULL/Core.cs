using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net.Http;
using System.Threading.Tasks;
using HarmonyLib;
using Newtonsoft.Json;
using TMPro;
using UltrakULL.json;
using static UltrakULL.CommonFunctions;
using System.Linq;

namespace UltrakULL
{
    public static class Core
    {
        public static Font VcrFont;
        public static GameObject ultrakullLogo = null;

        public static bool updateAvailable;
        public static bool updateFailed;
        
        public static bool GlobalFontReady;
        public static bool TMPFontReady;
        
        public static Font GlobalFont;
        public static Font MuseumFont;
        public static TMP_FontAsset GlobalFontTMP;
        public static TMP_FontAsset MuseumFontTMP;
        public static TMP_FontAsset CJKFontTMP;
        public static TMP_FontAsset JaFontTMP;
        public static TMP_FontAsset ArabicFontTMP;
		public static TMP_FontAsset HebrewFontTMP;
        public static Material GlobalFontTMPOverlayMat;
        public static Material CJKFontTMPOverlayMat;
        public static Material jaFontTMPOverlayMat;
        public static Sprite[] CustomRankImages;

        private static bool ultrakullDropdownExpanded = false;

        public static Sprite ArabicUltrakillLogo;

		public static bool wasLanguageReset = false;
        
        private static readonly HttpClient Client = new HttpClient();
        
        //Encapsulation function to patch all of the front end.
        public static void PatchFrontEnd(GameObject frontEnd)
        {
            MainMenu.Patch(frontEnd);
            Options options = new Options(ref frontEnd);
        }

        public static async Task CheckForUpdates()
        {
            string updateUrl = "https://api.github.com/repos/clearwateruk/ultrakull/releases/latest";

            // We assume that the Client is configured once and is static
            if (!Client.DefaultRequestHeaders.UserAgent.Any())
            {
                Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                Client.DefaultRequestHeaders.UserAgent.ParseAdd("UltrakULL-Updater/1.0 (+https://github.com/clearwateruk/ultrakull)");
                Client.Timeout = TimeSpan.FromSeconds(5);
            }

            try
            {
                string responseJsonRaw = await Client.GetStringAsync(updateUrl);
                UpdateInfo responseJson = JsonConvert.DeserializeObject<UpdateInfo>(responseJsonRaw);

                if (responseJson == null || string.IsNullOrEmpty(responseJson.tag_name))
                    throw new Exception("Update info is missing or invalid.");

                string versionString = responseJson.tag_name.TrimStart('v', 'V');
                Logging.Message("Latest version on GitHub: " + versionString);
                Logging.Message("Current local version: " + MainPatch.GetVersion());

                Version onlineVersion = new Version(versionString);
                Version localVersion = new Version(MainPatch.GetVersion());

                updateAvailable = localVersion.CompareTo(onlineVersion) < 0;
                if (updateAvailable)
                    Logging.Warn("UPDATE AVAILABLE!");
                else
                    Logging.Message("No newer version detected. Assuming current version is up to date.");

                updateFailed = false;
            }
            catch (Exception e)
            {
                Logging.Error("Unable to acquire version info from GitHub.");
                Logging.Error(e.ToString());
                updateAvailable = false;
                updateFailed = true;
            }
        }


        //Patches all text strings in the pause menu.
        public static void PatchPauseMenu(ref GameObject canvasObj)
        {
            try
            {
                GameObject pauseMenu = GetGameObjectChild(canvasObj, "PauseMenu");

                //Title
                TextMeshProUGUI pauseText = GetTextMeshProUGUI(GetGameObjectChild(pauseMenu, "Text"));
                pauseText.text = "-- " + LanguageManager.CurrentLanguage.pauseMenu.pause_title + " --";

                //Resume
                TextMeshProUGUI continueText = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(pauseMenu, "Resume"), "Text"));
                continueText.text = LanguageManager.CurrentLanguage.pauseMenu.pause_resume;

                //Checkpoint
                TextMeshProUGUI checkpointText = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(pauseMenu, "Restart Checkpoint"), "Text"));
                checkpointText.text = LanguageManager.CurrentLanguage.pauseMenu.pause_respawn;
                if (GetCurrentSceneName().Contains("Intermission"))
                {
                    checkpointText.text = LanguageManager.CurrentLanguage.pauseMenu.pause_skip;
                }
                //Restart mission
                TextMeshProUGUI restartText = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(pauseMenu, "Restart Mission"), "Text"));
                restartText.text = LanguageManager.CurrentLanguage.pauseMenu.pause_restart;

                //Options
                TextMeshProUGUI optionsText = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(pauseMenu, "Options"), "Text"));
                optionsText.text = LanguageManager.CurrentLanguage.pauseMenu.pause_options;

                //Quit
                TextMeshProUGUI quitText = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(pauseMenu, "Quit Mission"), "Text"));
                quitText.text = LanguageManager.CurrentLanguage.pauseMenu.pause_quit;

                //Quit+Restart windows
                GameObject pauseDialogs = GetGameObjectChild(canvasObj, "PauseMenuDialogs");

                //Quit
                GameObject quitDialog = GetGameObjectChild(GetGameObjectChild(pauseDialogs, "Quit Confirm"), "Panel");
                TextMeshProUGUI quitDialogText = GetTextMeshProUGUI(GetGameObjectChild(quitDialog, "Text (2)"));
                quitDialogText.text = LanguageManager.CurrentLanguage.pauseMenu.pause_quitConfirm;

                TextMeshProUGUI quitDialogTooltip = GetTextMeshProUGUI(GetGameObjectChild(quitDialog, "Text (1)"));
                quitDialogTooltip.text = LanguageManager.CurrentLanguage.pauseMenu.pause_disableWindow;

                TextMeshProUGUI quitDialogYes = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(quitDialog, "Confirm"), "Text"));
                quitDialogYes.text = LanguageManager.CurrentLanguage.pauseMenu.pause_quitConfirmYes;

                TextMeshProUGUI quitDialogNo = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(quitDialog, "Cancel"), "Text"));
                quitDialogNo.text = LanguageManager.CurrentLanguage.pauseMenu.pause_quitConfirmNo;

                //Restart
                GameObject restartDialog = GetGameObjectChild(GetGameObjectChild(pauseDialogs, "Restart Confirm"), "Panel");

                TextMeshProUGUI restartDialogText = GetTextMeshProUGUI(GetGameObjectChild(restartDialog, "Text"));
                restartDialogText.text = LanguageManager.CurrentLanguage.pauseMenu.pause_restartConfirm;

                TextMeshProUGUI restartDialogTooltip = GetTextMeshProUGUI(GetGameObjectChild(restartDialog, "Text (1)"));
                restartDialogTooltip.text = LanguageManager.CurrentLanguage.pauseMenu.pause_disableWindow;

                TextMeshProUGUI restartDialogYes = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(restartDialog, "Confirm"), "Text"));
                restartDialogYes.text = LanguageManager.CurrentLanguage.pauseMenu.pause_restartConfirmYes;

                TextMeshProUGUI restartDialogNo = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(restartDialog, "Cancel"), "Text"));
                restartDialogNo.text = LanguageManager.CurrentLanguage.pauseMenu.pause_restartConfirmNo;
            }
            catch (Exception e)
            {
                Logging.Error("Failed to patch pause menu.");
                Logging.Error(e.ToString());
            }
        }
        
        public static void LoadFonts()
        {
            Logging.Message("Loading font resource bundle...");
            //Will load from the same directory that the dll is in.
            AssetBundle fontBundle = AssetBundle.LoadFromFile(Path.Combine(MainPatch.ModFolder,"ullfont.resource"));

            AssetBundle extraFontBundle = AssetBundle.LoadFromFile(Path.Combine(MainPatch.ModFolder, "arabfonts"));

            if (extraFontBundle == null)
            {
                Logging.Error("Failed to load Arabic / Hebrew fonts. :( (No extra AssetBundle found!)");
            }
            else
            {
                Logging.Message("Extra Fonts Asset Bundle has been loaded...");

                TMP_FontAsset arabicFontAsset = extraFontBundle.LoadAsset<TMP_FontAsset>("segoeui SDF Arabic");
				TMP_FontAsset hebrewFontAsset = extraFontBundle.LoadAsset<TMP_FontAsset>("segoeui SDF Hebrew");
				Sprite arabicLogo = extraFontBundle.LoadAsset<Sprite>("2023_improved_logo.png");

                Sprite rankD = extraFontBundle.LoadAsset<Sprite>("RankD.png");
                Sprite rankC = extraFontBundle.LoadAsset<Sprite>("RankC.png");
                Sprite rankB = extraFontBundle.LoadAsset<Sprite>("RankB.png");
                Sprite rankA = extraFontBundle.LoadAsset<Sprite>("RankA.png");
                Sprite rankS = extraFontBundle.LoadAsset<Sprite>("RankS.png");
                Sprite rankSS = extraFontBundle.LoadAsset<Sprite>("RankSS.png");
                Sprite rankSSS = extraFontBundle.LoadAsset<Sprite>("RankSSS.png");
                Sprite rankU = extraFontBundle.LoadAsset<Sprite>("RankU.png");

                CustomRankImages = new Sprite[8];
				CustomRankImages[0] = rankD;
				CustomRankImages[1] = rankC;
				CustomRankImages[2] = rankB;
				CustomRankImages[3] = rankA;
				CustomRankImages[4] = rankS;
				CustomRankImages[5] = rankSS;
				CustomRankImages[6] = rankSSS;
				CustomRankImages[7] = rankU;

				if (arabicFontAsset == null)
                {
                    Logging.Warn("There is no Arabic font in this AssetBundle!?");
                }
                else
                {
                    Logging.Message("Arabic Font has been loaded.");
                    ArabicFontTMP = arabicFontAsset;
                }

                if (arabicLogo == null)
                {
					Logging.Warn("There is no Arabic logo in this AssetBundle!?");
				}
                else
                {
                    ArabicUltrakillLogo = arabicLogo;
                }

				if (hebrewFontAsset == null)
				{
					Logging.Warn("There is no Hebrew font in this AssetBundle!?");
				}
				else
				{
					Logging.Message("Hebrew Font has been loaded.");
					HebrewFontTMP = hebrewFontAsset;
				}
			}

			if (fontBundle == null)
            {
                Logging.Error("FAILED TO LOAD");
            }
            else
            {
                Logging.Message("Font bundle loaded.");
                Logging.Message("Loading fonts from bundle...");
                
                Font font1 = fontBundle.LoadAsset<Font>("VCR_OSD_MONO_EXTENDED");
                Font font2 = fontBundle.LoadAsset<Font>("EBGaramond-Regular");
                TMP_FontAsset font1TMP = fontBundle.LoadAsset<TMP_FontAsset>("VCR_OSD_MONO_EXTENDED_TMP");
                TMP_FontAsset font2TMP = fontBundle.LoadAsset<TMP_FontAsset>("EBGaramond-Regular_TMP");
                Material font1TMPTopMat = fontBundle.LoadAsset<Material>("VCR_OSD_MONO_EXTENDED_TMP_Overlay_Material");
                
                TMP_FontAsset cjkFontTMP = fontBundle.LoadAsset<TMP_FontAsset>("NotoSans-CJK_TMP");
                TMP_FontAsset jafontTMP = fontBundle.LoadAsset<TMP_FontAsset>("JF-Dot-jiskan16s-2000_TMP");
                Material cjkFontTMPTopMat = fontBundle.LoadAsset<Material>("NotoSans-CJK_TMP_Overlay_Material");
                Material jaFontTMPTopMat = fontBundle.LoadAsset<Material>("JF-Dot-jiskan16s-2000_TMP_Overlay_Material");
                if (font1 && font2)
                {
                    Logging.Warn("Normal fonts loaded.");
                    GlobalFont = font1;
                    MuseumFont = font2;
                    GlobalFontReady = true;
                }
                else
                {
                    Logging.Error("FAILED TO LOAD NORMAL FONTS");
                    GlobalFontReady = false;
                }
                if(font1TMP && font2TMP && cjkFontTMP && jafontTMP && font1TMPTopMat && cjkFontTMPTopMat && jaFontTMPTopMat)
                {
                    Logging.Warn("Normal TMP fonts loaded.");
                    GlobalFontTMP = font1TMP;
                    MuseumFontTMP = font2TMP;
                    CJKFontTMP = cjkFontTMP;
                    JaFontTMP = jafontTMP;
                    GlobalFontTMPOverlayMat = font1TMPTopMat;
                    CJKFontTMPOverlayMat = cjkFontTMPTopMat;
                    jaFontTMPOverlayMat = jaFontTMPTopMat;
                    
                    TMPFontReady = true;
                }
                else
                {
                    Logging.Error("FAILED TO LOAD TMP FONTS");
                    TMPFontReady = false;
                }
                
            }
        }
        
        public static void HandleSceneSwitch(Scene scene,ref GameObject canvas)
        {

            //Logging.Message("Switching scenes...");
            string levelName = GetCurrentSceneName();
            if(levelName == "Intro" || levelName == "Bootstrap")
            { 
                //Don't do anything if we're still booting up the game.
                //Logging.Warn("In intro, not hooking yet");
                return;
            }
            
            //Each scene (level) has an object called Canvas. Most game objects are there.
            GameObject canvasObj = GetInactiveRootObject("Canvas");
            if (!canvasObj)
            {
                Logging.Fatal("UNABLE TO FIND CANVAS IN CURRENT SCENE");
                return;
            }
            else
            {
                switch(levelName) 
                { 
                case "Intro": { break; }
                case "Main Menu":
                    {
                        if (Core.wasLanguageReset)
                        {
                            Core.wasLanguageReset = false;
                            MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("<color=orange>The currently set language file could not be loaded.\nLanguage has been reset to English to avoid problems.</color>");
                        }

                        PatchFrontEnd(canvasObj);

                        if (ultrakullLogo != null)
                        {
                            GameObject.Destroy(ultrakullLogo);
                            ultrakullLogo = null;
                        }

                        ultrakullLogo = new GameObject("UltrakULL_Dropdown");
                        ultrakullLogo.transform.SetParent(canvasObj.transform, false);

                        RectTransform rootRect = ultrakullLogo.AddComponent<RectTransform>();
                        rootRect.anchorMin = new Vector2(1, 1);
                        rootRect.anchorMax = new Vector2(1, 1);
                        rootRect.pivot = new Vector2(1, 1);
                        rootRect.anchoredPosition = new Vector2(-20, -20);
                        rootRect.sizeDelta = new Vector2(250, 30);

                        Image buttonImage = ultrakullLogo.AddComponent<Image>();
                        buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 0.7f);
                        Button button = ultrakullLogo.AddComponent<Button>();

                        GameObject buttonTextObj = new GameObject("ButtonText");
                        buttonTextObj.transform.SetParent(ultrakullLogo.transform, false);
                        RectTransform buttonTextRect = buttonTextObj.AddComponent<RectTransform>();
                        buttonTextRect.anchorMin = Vector2.zero;
                        buttonTextRect.anchorMax = Vector2.one;
                        buttonTextRect.offsetMin = Vector2.zero;
                        buttonTextRect.offsetMax = Vector2.zero;

                        TextMeshProUGUI buttonText = buttonTextObj.AddComponent<TextMeshProUGUI>();
                        buttonText.text = "UltrakULL ▼";
                        buttonText.alignment = TextAlignmentOptions.MidlineRight;
                        buttonText.fontSize = 16;
                        buttonText.color = Color.white;

                        GameObject panel = new GameObject("DropdownPanel");
                        panel.transform.SetParent(ultrakullLogo.transform, false);
                        RectTransform panelRect = panel.AddComponent<RectTransform>();
                        panelRect.anchorMin = new Vector2(1, 1);
                        panelRect.anchorMax = new Vector2(1, 1);
                        panelRect.pivot = new Vector2(1, 1);
                        panelRect.anchoredPosition = new Vector2(0, -30);
                        panelRect.sizeDelta = new Vector2(rootRect.sizeDelta.x, updateAvailable ? 170 : 130);

                        Image panelBg = panel.AddComponent<Image>();
                        panelBg.color = new Color(0f, 0f, 0f, 0.75f);

                        GameObject panelTextObj = new GameObject("PanelText");
                        panelTextObj.transform.SetParent(panel.transform, false);
                        RectTransform panelTextRect = panelTextObj.AddComponent<RectTransform>();
                        panelTextRect.anchorMin = new Vector2(0, 0);
                        panelTextRect.anchorMax = new Vector2(1, 1);
                        panelTextRect.offsetMin = new Vector2(5, 5);
                        panelTextRect.offsetMax = new Vector2(-5, -5);

                        TextMeshProUGUI panelText = panelTextObj.AddComponent<TextMeshProUGUI>();
                        panelText.text = "<color=white>UltrakULL loaded.\nVersion: " + MainPatch.GetVersion() + "\nCurrent locale: " + LanguageManager.CurrentLanguage.metadata.langName;
                        panelText.alignment = TextAlignmentOptions.TopRight;
                        panelText.fontSize = 16;
                        panelText.color = Color.white;


                        if (updateAvailable)
                        {
                            panelText.text += "\n<color=green>UPDATE AVAILABLE!</color>";

                            GameObject updateLink = new GameObject("UpdateLink", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(Button));
                            updateLink.transform.SetParent(panel.transform, false);

                            RectTransform linkRect = updateLink.GetComponent<RectTransform>();
                            linkRect.anchorMin = new Vector2(1, 1);   
                            linkRect.anchorMax = new Vector2(1, 1);
                            linkRect.pivot = new Vector2(1, 1);       
                            linkRect.anchoredPosition = new Vector2(-5, -90); 
                            linkRect.sizeDelta = new Vector2(150, 24); 

                            TextMeshProUGUI linkText = updateLink.GetComponent<TextMeshProUGUI>();
                            linkText.font = GlobalFontTMP;
                            linkText.text = "<u><color=white>VIEW UPDATE</color></u>";
                            linkText.alignment = TextAlignmentOptions.TopRight;
                            linkText.fontSize = 16;
                            linkText.raycastTarget = true;

                            Button updateButton = updateLink.GetComponent<Button>();
                            updateButton.onClick.AddListener(() =>
                            {
                                Application.OpenURL("https://github.com/ClearwaterTM/UltrakULL/releases/latest");
                            });
                        }


                        if (!LanguageManager.FileMatchesMinimumRequiredVersion(LanguageManager.CurrentLanguage.metadata.minimumModVersion, MainPatch.GetVersion()) && !isUsingEnglish())
                        {
                            panelText.text += "\n<color=orange>This language file\nwas created for\nan older version of\nUltrakULL.\nPlease check for\nan update to this file!</color>";
                        }
                        else if (!updateAvailable && updateFailed)
                        {
                            panelText.text += "\n<color=red>Unable to check for updates.\nCheck console for info.</color>";
                        }

                        CanvasGroup panelGroup = panel.AddComponent<CanvasGroup>();
                        panelGroup.alpha = 0f;
                        panelGroup.interactable = false;
                        panelGroup.blocksRaycasts = false;

                        button.onClick.AddListener(() =>
                        {
                            ultrakullDropdownExpanded = !ultrakullDropdownExpanded;
                            panelGroup.alpha = ultrakullDropdownExpanded ? 1f : 0f;
                            panelGroup.interactable = ultrakullDropdownExpanded;
                            panelGroup.blocksRaycasts = ultrakullDropdownExpanded;
                            buttonText.text = ultrakullDropdownExpanded ? "UltrakULL ▲" : "UltrakULL ▼";
                        });

                        break;
                    }

                    default:
                    {
                        if (isUsingEnglish())
                        {
                            Logging.Warn("Current language is English, not patching.");
                            return;
                        }
                        
                        Logging.Message("Regular scene");
                        Logging.Message("Attempting to patch base elements");
                        try{PatchPauseMenu(ref canvasObj);} catch(Exception e){Console.WriteLine(e.ToString());}
                        try{Cheats.PatchCheatConsentPanel(ref canvasObj);;} catch(Exception e){Console.WriteLine(e.ToString());}
                        try{Sandbox.PatchAlterMenu();} catch(Exception e){ Console.WriteLine(e.ToString());}
                        try{HUDMessages.PatchDeathScreen(ref canvasObj);} catch(Exception e){Console.WriteLine(e.ToString());}
                        try{LevelStatWindow.PatchStats(ref canvasObj);} catch(Exception e){Console.WriteLine(e.ToString());}
                        try{HUDMessages.PatchMisc(ref canvasObj);} catch(Exception e){Console.WriteLine(e.ToString());}
                        try{Options options = new Options(ref canvasObj);} catch(Exception e){Console.WriteLine(e.ToString());}
        
                        Logging.Message("Base elements patched");
                        }
                        
                        
                        if (levelName.Contains("Tutorial"))
                            { 
                                Logging.Message("Tutorial");
                            }
                            else if (levelName.Contains("-S"))
                            {
                                Logging.Message("Secret");
                                SecretLevels secretLevels = new SecretLevels(ref canvasObj);
                            }
                            if(levelName.Contains("0-") & !levelName.Contains("-E"))
                            { 
                                Logging.Message("Prelude");
                                Prelude preludePatchClass = new Prelude(ref canvasObj);
                            }
                            else if((levelName.Contains("1-") & !levelName.Contains("-E")) || (levelName.Contains("2-") & !levelName.Contains("-E")) || (levelName.Contains("3-") & !levelName.Contains("-E")) )
                            {
                                Logging.Message("Act 1");
                                Act1.PatchAct1(ref canvasObj);
                            }
                            else if((levelName.Contains("4-") & !levelName.Contains("-E")) || (levelName.Contains("5-") & !levelName.Contains("-E")) || (levelName.Contains("6-") & !levelName.Contains("-E")) )
                            {
                                Logging.Message("Act 2");
                                Act2.PatchAct2(ref canvasObj);
                            }
                            else if((levelName.Contains("7-") & !levelName.Contains("-E")) || (levelName.Contains("8-") & !levelName.Contains("-E")) || (levelName.Contains("9-") & !levelName.Contains("-E")) )
                            {
                                Logging.Message("Act 3");
                                if(LanguageManager.CurrentLanguage.act3 != null)
                                {
                                    Act3.PatchAct3(ref canvasObj);
                                }
                                else
                                {
                                    Logging.Warn("Category is not found in the language file!");
                                }
                            }
                            else if (levelName.Contains("P-"))
                            {
                                Logging.Message("Prime");
                                PrimeSanctum primeSanctumClass = new PrimeSanctum();
                            }
                            else if (levelName.Contains("-E"))
                            {
                                Logging.Message("Encore");
                                if (LanguageManager.CurrentLanguage.encore != null)
                                {
                                    Encore.PatchEncore(ref canvasObj);
                                }

                            }
                            else if (levelName == "uk_construct")
                            { 
                                Logging.Message("Sandbox");
                                Sandbox sandbox = new Sandbox(ref canvasObj);
                            }
                            else if (levelName == "Endless")
                            {
                                Logging.Message("CyberGrind");
                                CyberGrind.PatchCg();
                            }
                            else if (levelName.Contains("Intermission") || levelName.Contains("EarlyAccessEnd"))
                            {
                                Logging.Message("Intermission");
                                Intermission intermission = new Intermission(ref canvasObj);
                            }
                            else if (levelName == "CreditsMuseum2")
                            {
                                Logging.Message("DevMuseum");
                                DevMuseum devMuseum = new DevMuseum();
                            }
                        break;
                }
            }
        }

        public static async void ApplyPostInitFixes(GameObject canvasObj)
        {
            /*await Task.Delay(250);
            if (GetCurrentSceneName() == "Main Menu")
            {
                //Open Language Folder button in Options->Language
                TextMeshProUGUI openLangFolderText = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(canvasObj,"OptionsMenu"), "Language Page"),"Scroll Rect (1)"),"Contents"),"OpenLangFolder"),"Slot Text")); 
                openLangFolderText.text = "<color=#03fc07>Open language folder</color>";
                
            }*/
        }
    }
}
