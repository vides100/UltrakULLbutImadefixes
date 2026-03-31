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
using BepInEx;
using BepInEx.Configuration;

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
            string rssUrl = "https://github.com/ClearwaterUK/UltrakULL/releases.atom";
            // Increase timeout to 10 seconds for better reliability
            Client.Timeout = TimeSpan.FromSeconds(10);
            // Add User-Agent header to avoid being blocked by GitHub
            if (!Client.DefaultRequestHeaders.Contains("User-Agent"))
            {
                Client.DefaultRequestHeaders.Add("User-Agent", "UltrakULL-Update-Checker/1.0");
            }

            try
            {
                string rssContent = await Client.GetStringAsync(rssUrl);
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml(rssContent);

                // Create namespace manager for Atom
                var nsManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace("atom", "http://www.w3.org/2005/Atom");

                // Get latest release entry using namespace
                var latest = doc.SelectSingleNode("//atom:entry[1]", nsManager);
                if (latest == null)
                    throw new Exception("No releases found in RSS feed");

                string title = latest.SelectSingleNode("atom:title", nsManager)?.InnerText ?? "";
                string updated = latest.SelectSingleNode("atom:updated", nsManager)?.InnerText ?? "";

                // Parse version from title (usually the tag)
                string versionString = title.TrimStart('v', 'V');
                // Remove any suffix after hyphen (e.g., "-beta.2") or plus (e.g., "+build")
                int hyphenIndex = versionString.IndexOf('-');
                if (hyphenIndex >= 0)
                    versionString = versionString.Substring(0, hyphenIndex);
                int plusIndex = versionString.IndexOf('+');
                if (plusIndex >= 0)
                    versionString = versionString.Substring(0, plusIndex);
                // Ensure version string is valid for Version class
                Logging.Message("Latest version from RSS (cleaned): " + versionString);
                Logging.Message("Current local version: " + MainPatch.GetVersion());

                Version onlineVersion = new Version(versionString);
                // Clean local version similarly
                string localVersionString = MainPatch.GetVersion();
                localVersionString = localVersionString.TrimStart('v', 'V');
                hyphenIndex = localVersionString.IndexOf('-');
                if (hyphenIndex >= 0)
                    localVersionString = localVersionString.Substring(0, hyphenIndex);
                plusIndex = localVersionString.IndexOf('+');
                if (plusIndex >= 0)
                    localVersionString = localVersionString.Substring(0, plusIndex);
                Version localVersion = new Version(localVersionString);

                // Simple version compare - update available if online version is newer
                updateAvailable = localVersion.CompareTo(onlineVersion) < 0;

                if (updateAvailable)
                    Logging.Warn("UPDATE AVAILABLE!");
                else
                    Logging.Message("No newer version detected. Assuming current version is up to date.");

                updateFailed = false;
            }
            catch (TaskCanceledException)
            {
                Logging.Error("Update check timed out after 10 seconds.");
                updateAvailable = false;
                updateFailed = true;
            }
            catch (HttpRequestException hre)
            {
                Logging.Error("Network error while checking for updates: " + hre.Message);
                updateAvailable = false;
                updateFailed = true;
            }
            catch (Exception e)
            {
                Logging.Error("Unable to check for updates via RSS feed.");
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
                //SKIP button 
                if (GetCurrentSceneName().Contains("Intermission"))
                {
                    TextMeshProUGUI skipText = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(pauseMenu, "Restart Checkpoint (1)"), "Text"));
                    skipText.text = LanguageManager.CurrentLanguage.pauseMenu.pause_skip;
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
                                Application.OpenURL("https://github.com/ClearwaterUK/UltrakULL/releases/latest");
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
            await Task.Delay(250); // Fix warning about async without await
            /*if (GetCurrentSceneName() == "Main Menu")
            {
                //Open Language Folder button in Options->Language
                TextMeshProUGUI openLangFolderText = GetTextMeshProUGUI(GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(GetGameObjectChild(canvasObj,"OptionsMenu"), "Language Page"),"Scroll Rect (1)"),"Contents"),"OpenLangFolder"),"Slot Text")); 
                openLangFolderText.text = "<color=#03fc07>Open language folder</color>";
                
            }*/
        }

        public static void AddKoreanGlyphs()
        {
            if (GlobalFontTMP == null) return;
            if (LanguageManager.CurrentLanguage.metadata.langName != "ko-KR") return;

            Logging.Message("Korean detected, adding glyphs...");

            var multiAtlasField = typeof(TMP_FontAsset).GetField("m_IsMultiAtlasTexturesEnabled",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (multiAtlasField != null)
                multiAtlasField.SetValue(GlobalFontTMP, true);

            string charsToAdd = " 가각간갇갈갉갊감갑값갓갔강갖갗같갚갛개객갠갤갬갭갯갰갱갸갹갼걀걋걍걔걘걜거걱건걷걸걺검겁것겄겅겆겉겊겋게겐겔겜겝겟겠겡겨격겪견겯결겸겹겻겼경곁계곈곌곕곗고곡곤곧골곪곬곯곰곱곳공곶과곽관괄괆괌괍괏광괘괜괠괩괬괭괴괵괸괼굄굅굇굉교굔굘굡굣구국군굳굴굵굶굻굼굽굿궁궂궈궉권궐궜궝궤궷귀귁귄귈귐귑귓규균귤그극근귿글긁금급긋긍긔기긱긴긷길긺김깁깃깅깆깊까깍깎깐깔깖깜깝깟깠깡깥깨깩깬깰깸깹깻깼깽꺄꺅꺌꺼꺽꺾껀껄껌껍껏껐껑께껙껜껨껫껭껴껸껼꼇꼈꼍꼐꼬꼭꼰꼲꼴꼼꼽꼿꽁꽂꽃꽈꽉꽐꽜꽝꽤꽥꽹꾀꾄꾈꾐꾑꾕꾜꾸꾹꾼꿀꿇꿈꿉꿋꿍꿎꿔꿜꿨꿩꿰꿱꿴꿸뀀뀁뀄뀌뀐뀔뀜뀝뀨끄끅끈끊끌끎끓끔끕끗끙끝끼끽낀낄낌낍낏낑나낙낚난낟날낡낢남납낫났낭낮낯낱낳내낵낸낼냄냅냇냈냉냐냑냔냘냠냥너넉넋넌널넒넓넘넙넛넜넝넣네넥넨넬넴넵넷넸넹녀녁년녈념녑녔녕녘녜녠노녹논놀놂놈놉놋농높놓놔놘놜놨뇌뇐뇔뇜뇝뇟뇨뇩뇬뇰뇹뇻뇽누눅눈눋눌눔눕눗눙눠눴눼뉘뉜뉠뉨뉩뉴뉵뉼늄늅늉느늑는늘늙늚늠늡늣능늦늪늬늰늴니닉닌닐닒님닙닛닝닢다닥닦단닫달닭닮닯닳담답닷닸당닺닻닿대댁댄댈댐댑댓댔댕댜더덕덖던덛덜덞덟덤덥덧덩덫덮데덱덴델뎀뎁뎃뎄뎅뎌뎐뎔뎠뎡뎨뎬도독돈돋돌돎돐돔돕돗동돛돝돠돤돨돼됐되된될됨됩됫됬됴두둑둔둘둠둡둣둥둬뒀뒈뒝뒤뒨뒬뒵뒷뒹듀듄듈듐듕드득든듣들듦듬듭듯등듸디딕딘딛딜딤딥딧딨딩딪따딱딴딸땀땁땃땄땅땋때땍땐땔땜땝땟땠땡떠떡떤떨떪떫떰떱떳떴떵떻떼떽뗀뗄뗌뗍뗏뗐뗑뗘뗬또똑똔똘똥똬똴뙈뙤뙨뚜뚝뚠뚤뚫뚬뚱뛔뛰뛴뛸뜀뜁뜅뜨뜩뜬뜯뜰뜸뜹뜻띄띈띌띔띕띠띤띨띰띱띳띵라락란랄람랍랏랐랑랒랖랗래랙랜랠램랩랫랬랭랴략랸럇량러럭런럴럼럽럿렀렁렇레렉렌렐렘렙렛렝려력련렬렴렵렷렸령례롄롑롓로록론롤롬롭롯롱롸롼뢍뢨뢰뢴뢸룀룁룃룅료룐룔룝룟룡루룩룬룰룸룹룻룽뤄뤘뤠뤼뤽륀륄륌륏륑류륙륜률륨륩륫륭르륵른를름릅릇릉릊릍릎리릭린릴림립릿링마막만많맏말맑맒맘맙맛망맞맡맣매맥맨맬맴맵맷맸맹맺먀먁먈먕머먹먼멀멂멈멉멋멍멎멓메멕멘멜멤멥멧멨멩며멱면멸몃몄명몇몌모목몫몬몰몲몸몹못몽뫄뫈뫘뫙뫼묀묄묍묏묑묘묜묠묩묫무묵묶문묻물묽묾뭄뭅뭇뭉뭍뭏뭐뭔뭘뭡뭣뭬뮈뮌뮐뮤뮨뮬뮴뮷므믄믈믐믓미믹민믿밀밂밈밉밋밌밍및밑바박밖밗반받발밝밞밟밤밥밧방밭배백밴밸뱀뱁뱃뱄뱅뱉뱌뱍뱐뱝버벅번벋벌벎범법벗벙벚베벡벤벧벨벰벱벳벴벵벼벽변별볍볏볐병볕볘볜보복볶본볼봄봅봇봉봐봔봤봬뵀뵈뵉뵌뵐뵘뵙뵤뵨부북분붇불붉붊붐붑붓붕붙붚붜붤붰붸뷔뷕뷘뷜뷩뷰뷴뷸븀븃븅브븍븐블븜븝븟비빅빈빌빎빔빕빗빙빚빛빠빡빤빨빪빰빱빳빴빵빻빼빽뺀뺄뺌뺍뺏뺐뺑뺘뺙뺨뻐뻑뻔뻗뻘뻠뻣뻤뻥뻬뼁뼈뼉뼘뼙뼛뼜뼝뽀뽁뽄뽈뽐뽑뽕뾔뾰뿅뿌뿍뿐뿔뿜뿟뿡쀼쁑쁘쁜쁠쁨쁩삐삑삔삘삠삡삣삥사삭삯산삳살삵삶삼삽삿샀상샅새색샌샐샘샙샛샜생샤샥샨샬샴샵샷샹섀섄섈섐섕서석섞섟선섣설섦섧섬섭섯섰성섶세섹센셀셈셉셋셌셍셔셕션셜셤셥셧셨셩셰셴셸솅소속솎손솔솖솜솝솟송솥솨솩솬솰솽쇄쇈쇌쇔쇗쇘쇠쇤쇨쇰쇱쇳쇼쇽숀숄숌숍숏숑수숙순숟술숨숩숫숭숯숱숲숴쉈쉐쉑쉔쉘쉠쉥쉬쉭쉰쉴쉼쉽쉿슁슈슉슐슘슛슝스슥슨슬슭슴습슷승시식신싣실싫심십싯싱싶싸싹싻싼쌀쌈쌉쌌쌍쌓쌔쌕쌘쌜쌤쌥쌨쌩썅써썩썬썰썲썸썹썼썽쎄쎈쎌쏀쏘쏙쏜쏟쏠쏢쏨쏩쏭쏴쏵쏸쐈쐐쐤쐬쐰쐴쐼쐽쑈쑤쑥쑨쑬쑴쑵쑹쒀쒔쒜쒸쒼쓩쓰쓱쓴쓸쓺쓿씀씁씌씐씔씜씨씩씬씰씸씹씻씽아악안앉않알앍앎앓암압앗았앙앝앞애액앤앨앰앱앳앴앵야약얀얄얇얌얍얏양얕얗얘얜얠얩어억언얹얻얼얽얾엄업없엇었엉엊엌엎에엑엔엘엠엡엣엥여역엮연열엶엷염엽엾엿였영옅옆옇예옌옐옘옙옛옜오옥온올옭옮옰옳옴옵옷옹옻와왁완왈왐왑왓왔왕왜왝왠왬왯왱외왹왼욀욈욉욋욍요욕욘욜욤욥욧용우욱운울욹욺움웁웃웅워웍원월웜웝웠웡웨웩웬웰웸웹웽위윅윈윌윔윕윗윙유육윤율윰윱윳융윷으윽은을읊음읍읏응읒읓읔읕읖읗의읜읠읨읫이익인일읽읾잃임입잇있잉잊잎자작잔잖잗잘잚잠잡잣잤장잦재잭잰잴잼잽잿쟀쟁쟈쟉쟌쟎쟐쟘쟝쟤쟨쟬저적전절젊점접젓정젖제젝젠젤젬젭젯젱져젼졀졈졉졌졍졔조족존졸졺좀좁좃종좆좇좋좌좍좔좝좟좡좨좼좽죄죈죌죔죕죗죙죠죡죤죵주죽준줄줅줆줌줍줏중줘줬줴쥐쥑쥔쥘쥠쥡쥣쥬쥰쥴쥼즈즉즌즐즘즙즛증지직진짇질짊짐집짓징짖짙짚짜짝짠짢짤짧짬짭짯짰짱째짹짼쨀쨈쨉쨋쨌쨍쨔쨘쨩쩌쩍쩐쩔쩜쩝쩟쩠쩡쩨쩽쪄쪘쪼쪽쫀쫄쫌쫍쫏쫑쫓쫘쫙쫠쫬쫴쬈쬐쬔쬘쬠쬡쭁쭈쭉쭌쭐쭘쭙쭝쭤쭸쭹쮜쮸쯔쯤쯧쯩찌찍찐찔찜찝찡찢찧차착찬찮찰참찹찻찼창찾채책챈챌챔챕챗챘챙챠챤챦챨챰챵처척천철첨첩첫첬청체첵첸첼쳄쳅쳇쳉쳐쳔쳤쳬쳰촁초촉촌촐촘촙촛총촤촨촬촹최쵠쵤쵬쵭쵯쵱쵸춈추축춘출춤춥춧충춰췄췌췐취췬췰췸췹췻췽츄츈츌츔츙츠측츤츨츰츱츳층치칙친칟칠칡침칩칫칭카칵칸칼캄캅캇캉캐캑캔캘캠캡캣캤캥캬캭컁커컥컨컫컬컴컵컷컸컹케켁켄켈켐켑켓켕켜켠켤켬켭켯켰켱켸코콕콘콜콤콥콧콩콰콱콴콸쾀쾅쾌쾡쾨쾰쿄쿠쿡쿤쿨쿰쿱쿳쿵쿼퀀퀄퀑퀘퀭퀴퀵퀸퀼큄큅큇큉큐큔큘큠크큭큰클큼큽킁키킥킨킬킴킵킷킹타탁탄탈탉탐탑탓탔탕태택탠탤탬탭탯탰탱탸턍터턱턴털턺텀텁텃텄텅테텍텐텔템텝텟텡텨텬텼톄톈토톡톤톨톰톱톳통톺톼퇀퇘퇴퇸툇툉툐투툭툰툴툼툽툿퉁퉈퉜퉤튀튁튄튈튐튑튕튜튠튤튬튱트특튼튿틀틂틈틉틋틔틘틜틤틥티틱틴틸팀팁팃팅파팍팎판팔팖팜팝팟팠팡팥패팩팬팰팸팹팻팼팽퍄퍅퍼퍽펀펄펌펍펏펐펑페펙펜펠펨펩펫펭펴편펼폄폅폈평폐폘폡폣포폭폰폴폼폽폿퐁퐈퐝푀푄표푠푤푭푯푸푹푼푿풀풂품풉풋풍풔풩퓌퓐퓔퓜퓟퓨퓬퓰퓸퓻퓽프픈플픔픕픗피픽핀필핌핍핏핑하학한할핥함합핫항핳해핵핸핼햄햅햇했행햐향허헉헌헐헒험헙헛헝헤헥헨헬헴헵헷헹혀혁현혈혐협혓혔형혜혠혤혭호혹혼홀홅홈홉홋홍홑화확환활홧황홰홱홴횃횅회획횐횔횝횟횡효횬횰횹횻후훅훈훌훑훔훗훙훠훤훨훰훵훼훽휀휄휑휘휙휜휠휨휩휫휭휴휵휸휼흄흇흉흐흑흔흖흗흘흙흠흡흣흥흩희흰흴흼흽힁히힉힌힐힘힙힛힝힣ㄱㄴㄷㄹㅁㅂㅅㅇㅈㅊㅋㅌㅍㅎㄲㄸㅃㅆㅉㄺㅀㄻㄼㅄㄳㄶㄵㄽㅏㅑㅓㅕㅗㅛㅜㅠㅡㅣㅒㅖABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890~`!@#$%^&*(){}[]-_=+;:'\",<.>/?\\|";
            GlobalFontTMP.TryAddCharacters(charsToAdd, out string missingChars);

            if (!string.IsNullOrEmpty(missingChars))
                Logging.Warn("Missing chars: " + missingChars.Length);
            else
                Logging.Message("Korean glyphs added");
        }
    }
}
