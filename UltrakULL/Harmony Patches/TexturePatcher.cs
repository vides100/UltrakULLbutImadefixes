using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BepInEx;
using HarmonyLib;
using UltrakULL.json;
using UnityEngine;
using UnityEngine.UI;
using static UltrakULL.CommonFunctions;
using Object = UnityEngine.Object;

namespace UltrakULL.Harmony_Patches
{
    [HarmonyPatch]
    public static class TexturePatcher
    {
        public static string texturesFolder = Path.Combine(Paths.ConfigPath, "ultrakull", "textures", LanguageManager.CurrentLanguage.metadata.langName) + Path.DirectorySeparatorChar;
        private static bool initialized = false;
        private static Dictionary<string, Dictionary<string, (string filename, string type)>> levelTextureMappings;
        private static Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
        private static MonoBehaviour coroutineStarter;
        private static Dictionary<string, Texture2D> currentReplacements;
        private static string currentLevel;
        private static bool isProcessing = false;
        private static CancellationTokenSource cancellationTokenSource;
        private static Dictionary<string, Sprite> rankSprites = new Dictionary<string, Sprite>();
        private static readonly HashSet<int> processedObjectIds = new HashSet<int>();
        private static readonly HashSet<int> processedRawImages = new HashSet<int>();




        private static readonly Dictionary<string, (string filename, string type)> globalTextureReplacements = new Dictionary<string, (string, string)>
        {
            { "checkpoint", ("Checkpoint", "texture") },
            { "T_ShopTerminal", ("T_ShopTerminal", "texture") },
            { "T_ShopTerminal_Emission", ("T_ShopTerminal_Emission", "texture") },
            { "RankD", ("RankD", "sprite") },
            { "RankC", ("RankC", "sprite") },
            { "RankB", ("RankB", "sprite") },
            { "RankA", ("RankA", "sprite") },
            { "RankS", ("RankS", "sprite") },
            { "RankSS", ("RankSS", "sprite") },
            { "RankSSS", ("RankSSS", "sprite") },
            { "RankU", ("RankU", "sprite") }
        };

        private static readonly HashSet<string> ignoredScenes = new HashSet<string>
        {
            "Bootstrap", "Intro", "Loading"
        };

        private static void EnsureTexturesFolderExists()
        {
            if (!Directory.Exists(texturesFolder))
            {
                Directory.CreateDirectory(texturesFolder);
                Logging.Message($"[TexturePatcher] No texture folder found. A new folder has been created: {texturesFolder}");
            }
        }

        [HarmonyPrepare]
        private static void Prepare()
        {
            cancellationTokenSource = new CancellationTokenSource();
            EnsureTexturesFolderExists();
            InitializeTextureMappings();
            Logging.Message("[TexturePatcher] Module initialized");
        }

        private static void InitializeTextureMappings()
        {
            if (initialized) return;

            levelTextureMappings = new Dictionary<string, Dictionary<string, (string filename, string type)>>
            {
                    { "Main Menu", new Dictionary<string, (string, string)> { { "TextmodeLogo", ("TextmodeLogo", "sprite") }, { "TextmodeCircuit", ("TextmodeCircuit", "texture") } } },
                    { "Tutorial", new Dictionary<string, (string, string)> { { "", ("Batch Tutorial", "texture") } } },
                    { "Level 0-1", new Dictionary<string, (string, string)> { { "logowideborderless", ("TextmodeLogo", "sprite") }, { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") }, { "SignWarning", ("SignWarning", "texture") }, { "SignCoolingChamber", ("SignCoolingChamber", "texture") }, { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") }, { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") } } },
                    { "Level 0-2", new Dictionary<string, (string, string)> { { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") }, { "SignWarning", ("SignWarning", "texture") }, { "SignCoolingChamber", ("SignCoolingChamber", "texture") }, { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") }, { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") } } },
                    { "Level 0-3", new Dictionary<string, (string, string)> { { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") }, { "SignWarning", ("SignWarning", "texture") }, { "SignCoolingChamber", ("SignCoolingChamber", "texture") }, { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") }, { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") } } },
                    { "Level 0-4", new Dictionary<string, (string, string)> { { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") }, { "SignWarning", ("SignWarning", "texture") }, { "SignCoolingChamber", ("SignCoolingChamber", "texture") }, { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") }, { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") } } },
                    { "Level 0-5", new Dictionary<string, (string, string)> { { "abandonhope2", ("abandonhope2", "texture") }, { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") }, { "SignWarning", ("SignWarning", "texture") }, { "SignCoolingChamber", ("SignCoolingChamber", "texture") }, { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") }, { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") } } },
                    { "Level 1-4", new Dictionary<string, (string, string)> { { "forgiveme", ("forgiveme", "texture") } } },
                    { "Level 2-2", new Dictionary<string, (string, string)> { { "electricitybox", ("electricitybox", "texture") }, { "", ("Batch 2-2", "texture") } } },
                    { "Level 2-3", new Dictionary<string, (string, string)> { { "", ("Batch 2-3", "texture") }, { "watercontrol1", ("watercontrol1", "texture") }, { "watercontrol2", ("watercontrol2", "texture") } } },
                    { "Level 4-3", new Dictionary<string, (string, string)> { { "traitor", ("traitor", "texture") }, { "", ("Batch 4-3", "texture") } } },
                    { "Level 5-1", new Dictionary<string, (string, string)> { { "WaterProcessingAttention", ("WaterProcessingAttention", "texture") }, { "", ("Batch 5-1", "texture") } } },
                    { "Level 5-S", new Dictionary<string, (string, string)> { { "", ("Batch 5-S", "texture") } } },
                    { "Level 7-2", new Dictionary<string, (string, string)> { { "", ("Batch 7-2", "texture") } } },
                    { "Level 7-3", new Dictionary<string, (string, string)> { { "marble_inverted 3", ("marble_inverted 3", "texture") } } },
                    { "Level 7-4", new Dictionary<string, (string, string)> { { "HotPipeSign", ("HotPipeSign", "texture") }, { "T_Cent_PlantRoom", ("T_Cent_PlantRoom", "texture") } } },
                    { "Level 7-S", new Dictionary<string, (string, string)> { { "T_Placard", ("T_Placard", "texture") } } },
                    { "Level 0-E", new Dictionary<string, (string, string)> { { "exit", ("exit", "texture") }, { "abandonhope2", ("abandonhope2", "texture") }, { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") }, { "SignWarning", ("SignWarning", "texture") }, { "SignCoolingChamber", ("SignCoolingChamber", "texture") }, { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") }, { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") } } },
                    { "uk_construct", new Dictionary<string, (string, string)> { { "garry", ("garry", "sprite") } } },
                    { "CreditsMuseum2", new Dictionary<string, (string, string)> { { "sign_map_Texture_2", ("sign_map_Texture_К2", "texture") }, { "poster", ("poster", "texture") }, { "Staff only sign_texture", ("Staff only sign_texture", "texture") } } }
            };

            initialized = true;
            Logging.Message($"[TexturePatcher] Loaded {globalTextureReplacements.Count} global and {levelTextureMappings.Count} level-specific mappings");
        }

        [HarmonyPatch(typeof(SceneHelper), "OnSceneLoaded")]
        [HarmonyPostfix]
        private static void OnSceneLoaded()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            if (coroutineStarter == null)
            {
                var go = new GameObject("TexturePatcher_CoroutineStarter");
                coroutineStarter = go.AddComponent<DummyMonoBehaviour>();
            }

            coroutineStarter.StartCoroutine(ProcessSceneChange());
        }

        private static IEnumerator ProcessSceneChange()
        {
            if (isProcessing) yield break;
            isProcessing = true;

            string sceneName = GetCurrentSceneName();
            if (string.IsNullOrEmpty(sceneName) || cancellationTokenSource.IsCancellationRequested)
            {
                Logging.Message("[TexturePatcher] Scene loading aborted");
                isProcessing = false;
                yield break;
            }

            Logging.Message($"[TexturePatcher] Processing scene: {sceneName}");
            if (ShouldIgnoreScene(sceneName))
            {
                Logging.Message($"[TexturePatcher] Ignoring scene: {sceneName}");
                isProcessing = false;
                yield break;
            }

            if (!initialized)
            {
                Logging.Message("[TexturePatcher] Not initialized");
                isProcessing = false;
                yield break;
            }

            yield return null;

            if (currentLevel != sceneName)
            {
                textureCache.Clear();
                currentLevel = sceneName;
            }

            currentReplacements = new Dictionary<string, Texture2D>();
            processedObjectIds.Clear();
            yield return coroutineStarter.StartCoroutine(LoadTextures(globalTextureReplacements));

            var levelSpecific = GetLevelSpecificTextures(sceneName);
            if (levelSpecific != null && levelSpecific.Count > 0)
                yield return coroutineStarter.StartCoroutine(LoadTextures(levelSpecific));

            if (currentReplacements.Count == 0)
            {
                isProcessing = false;
                yield break;
            }

            yield return coroutineStarter.StartCoroutine(ReplaceTexturesInScene(true));
            yield return UpdateStyleHUD();
            ReplaceUISprites();
            coroutineStarter.StartCoroutine(BackgroundTextureCheck());
            isProcessing = false;
        }

        private static Dictionary<string, (string filename, string type)> GetLevelSpecificTextures(string sceneName)
        {
            foreach (var kv in levelTextureMappings)
                if (sceneName.Contains(kv.Key))
                    return kv.Value;
            return null;
        }

        private static IEnumerator LoadTextures(Dictionary<string, (string filename, string type)> textureMap)
        {
            foreach (var mapping in textureMap)
            {
                if (cancellationTokenSource.IsCancellationRequested) yield break;
                if (currentReplacements.ContainsKey(mapping.Key)) continue;

                Texture2D loaded = null;
                yield return coroutineStarter.StartCoroutine(LoadTexture(mapping.Value.filename, tex => loaded = tex));
                if (loaded != null)
                {
                    loaded.filterMode = FilterMode.Point;

                    if (mapping.Value.type == "sprite")
                    {
                        float ppu = loaded.height;
                        var sprite = Sprite.Create(
                            loaded,
                            new Rect(0, 0, loaded.width, loaded.height),
                            new Vector2(0.5f, 0.5f), ppu);
                        rankSprites[mapping.Key] = sprite;
                        Logging.Message($"[TexturePatcher] Loaded sprite: {mapping.Value.filename} as {mapping.Key}");
                    }
                    else
                    {
                        currentReplacements[mapping.Key] = loaded;
                        Logging.Message($"[TexturePatcher] Loaded texture: {mapping.Value.filename} as {mapping.Key}");
                    }
                }
            }
        }


        private static IEnumerator BackgroundTextureCheck()
        {
            while (!cancellationTokenSource.IsCancellationRequested && !string.IsNullOrEmpty(currentLevel))
            {
                if (GetCurrentSceneName().Contains("4-S"))
                    yield return new WaitForSeconds(3f);
                else
                    yield return new WaitForSeconds(0.25f);
                if (currentReplacements != null && currentReplacements.Count > 0)
                {
                    yield return coroutineStarter.StartCoroutine(ReplaceTexturesInScene(false));
                    yield return UpdateStyleHUD();
                }
            }
        }


        private static readonly string[] TextureProps = { "_MainTex", "_BaseMap", "_DetailAlbedoMap", "_Texture", "_MainTexture", "_EmissiveTex" };

        private static IEnumerator ReplaceTexturesInScene(bool isInitialPass)
        {
            if (currentReplacements == null || cancellationTokenSource.IsCancellationRequested)
                yield break;

            Camera mainCam = Camera.main;
            int processed = 0;
            const int maxPerFrame = 5;

            var allRenderers = Object.FindObjectsOfType<Renderer>();
            var allRawImages = Object.FindObjectsOfType<UnityEngine.UI.RawImage>();

            // Обработка Renderer'ов
            foreach (var rend in allRenderers)
            {
                int id = rend.gameObject.GetInstanceID();

                if (mainCam && (rend.gameObject == mainCam.gameObject || rend.GetComponentInParent<Camera>() != null))
                    continue;

                if (!processedObjectIds.Add(id) || !rend)
                    continue;

                var materials = rend.materials;
                bool modified = false;

                foreach (var mat in materials)
                {
                    if (mat == null) continue;

                    foreach (var prop in TextureProps)
                    {
                        if (!mat.HasProperty(prop)) continue;

                        var curTex = mat.GetTexture(prop) as Texture2D;
                        if (curTex == null) continue;

                        if (currentReplacements.TryGetValue(curTex.name, out var repl) ||
                            currentReplacements.TryGetValue(curTex.name.ToLower(), out repl))
                        {
                            mat.SetTexture(prop, repl);
                            modified = true;
                            processed++;

                            if (processed >= maxPerFrame)
                            {
                                processed = 0;
                                yield return null;
                            }
                        }
                    }
                }

                if (modified)
                    rend.materials = materials;
            }

            // Обработка RawImage
            if (GetCurrentSceneName().Contains("Main Menu"))
            {
                foreach (var raw in allRawImages)
                {
                    int id = raw.gameObject.GetInstanceID();
                    if (!processedObjectIds.Add(id) || raw == null)
                        continue;

                    var curTex = raw.texture as Texture2D;
                    if (curTex == null) continue;

                    if (currentReplacements.TryGetValue(curTex.name, out var repl) ||
                        currentReplacements.TryGetValue(curTex.name.ToLower(), out repl))
                    {
                        raw.texture = repl;
                        processed++;

                        if (processed >= maxPerFrame)
                        {
                            processed = 0;
                            yield return null;
                        }
                    }
                }
            }
        }




        private static IEnumerator UpdateStyleHUD()
        {
            var hud = MonoSingleton<StyleHUD>.Instance;
            if (hud == null || rankSprites.Count == 0) yield break;

            for (int i = 0; i < hud.ranks.Count; i++)
            {
                string name = GetRankNameByIndex(i);
                if (rankSprites.TryGetValue(name, out var spr))
                    hud.ranks[i].sprite = spr;
            }
            string curName = GetRankNameByIndex(hud.rankIndex);
            if (rankSprites.TryGetValue(curName, out var curSpr))
                hud.rankImage.sprite = curSpr;
            yield return null;
        }

        private static string GetRankNameByIndex(int i)
        {
            switch (i)
            {
                case 0: return "RankD";
                case 1: return "RankC";
                case 2: return "RankB";
                case 3: return "RankA";
                case 4: return "RankS";
                case 5: return "RankSS";
                case 6: return "RankSSS";
                case 7: return "RankU";
                default: return "RankD";
            }
        }

        [HarmonyPatch(typeof(StyleHUD), "Start")]
        private static class StyleHUD_Start_Patch
        {
            private static void Postfix(StyleHUD __instance)
            {
                if (__instance == null || rankSprites.Count == 0) return;
                string name = GetRankNameByIndex(__instance.rankIndex);
                if (rankSprites.TryGetValue(name, out var spr))
                    __instance.rankImage.sprite = spr;
            }
        }

        private static IEnumerator LoadTexture(string filename, Action<Texture2D> callback)
        {
            if (cancellationTokenSource.IsCancellationRequested) { callback(null); yield break; }
            if (textureCache.TryGetValue(filename, out var cached)) { callback(cached); yield break; }

            string found = null;
            foreach (var ext in new[] { ".png", ".jpg", ".jpeg", ".tga" })
            {
                string p = Path.Combine(texturesFolder, filename + ext);
                if (File.Exists(p)) { found = p; break; }
                p = Path.Combine(texturesFolder, filename);
                if (File.Exists(p)) { found = p; break; }
            }
            if (found == null) { Logging.Warn($"[TexturePatcher] File not found: {filename}"); callback(null); yield break; }

            byte[] data = null; bool done = false; Exception err = null;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try { data = File.ReadAllBytes(found); }
                catch (Exception e) { err = e; }
                finally { done = true; }
            });
            while (!done && !cancellationTokenSource.IsCancellationRequested) yield return null;
            if (cancellationTokenSource.IsCancellationRequested) { callback(null); yield break; }
            if (err != null) { Logging.Warn($"[TexturePatcher] Error loading {filename}: {err.Message}"); callback(null); yield break; }
            if (data == null || data.Length == 0) { Logging.Warn($"[TexturePatcher] Empty file: {filename}"); callback(null); yield break; }

            // without mipmaps to preserve sharpness
            var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false)
            {
                name = Path.GetFileNameWithoutExtension(found),
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };
            if (!tex.LoadImage(data)) { Logging.Warn($"[TexturePatcher] Decode failed: {filename}"); callback(null); yield break; }
            textureCache[filename] = tex;
            callback(tex);
        }

        // Basic method structure to replace sprites with UI
        private static void ReplaceUISprites()
        {
            Image[] images = GameObject.FindObjectsOfType<Image>(true);

            foreach (var img in images)
            {
                if (img.sprite == null) continue;
                string key = img.sprite.name;
                if (rankSprites.TryGetValue(key, out var replacement))
                {
                    img.sprite = replacement;
                    Logging.Message($"[TexturePatcher] Replaced sprite: {key} in the UI object {img.gameObject.name}");
                }
            }
        }

        private static bool ShouldIgnoreScene(string sceneName)
            => ignoredScenes.Any(i => sceneName.Equals(i, StringComparison.OrdinalIgnoreCase));

        private class DummyMonoBehaviour : MonoBehaviour
        {
            private void OnDestroy()
            {
                cancellationTokenSource?.Cancel();
                currentLevel = null;
                currentReplacements = null;
                rankSprites.Clear();
                //textureCache.Clear();
                Logging.Message("[TexturePatcher] CoroutineStarter destroyed and cache cleared");
            }
        }
    }
}