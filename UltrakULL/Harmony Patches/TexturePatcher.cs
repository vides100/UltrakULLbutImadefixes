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
        private static string texturesFolder => Path.Combine(Paths.ConfigPath, "ultrakull", "textures", LanguageManager.CurrentLanguage.metadata.langName) + Path.DirectorySeparatorChar;
        private static bool initialized = false;
        private static Dictionary<string, Dictionary<string, (string filename, string type)>> levelTextureMappings;
        private static Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
        private static MonoBehaviour coroutineStarter;
        private static Dictionary<string, Texture2D> currentReplacements;
        private static string currentLevel = string.Empty;
        private static Coroutine backgroundCheckCoroutine;
        private static bool isProcessing = false;
        private static CancellationTokenSource cancellationTokenSource;
        private static Dictionary<string, Sprite> rankSprites = new Dictionary<string, Sprite>();
        private static readonly HashSet<int> processedObjectIds = new HashSet<int>();
        private static readonly HashSet<int> processedRawImages = new HashSet<int>();
        private static Coroutine backgroundChecker;

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
                Logging.Message($"[TexturePatcher] Created texture folder: {texturesFolder}");
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
                    { "Level 0-1", new Dictionary<string, (string, string)> { { "logowideborderless", ("logowideborderless", "sprite") }, { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") }, { "SignWarning", ("SignWarning", "texture") }, { "SignCoolingChamber", ("SignCoolingChamber", "texture") }, { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") }, { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") } } },
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
            cancellationTokenSource?.Dispose();
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
                ResetInternalState();
                yield break;
            }

            if (ShouldIgnoreScene(sceneName))
            {
                Logging.Message($"[TexturePatcher] Ignoring scene: {sceneName}");
                ResetInternalState();
                yield break;
            }

            if (!initialized)
            {
                Logging.Warn("[TexturePatcher] Not initialized");
                ResetInternalState();
                yield break;
            }

            Logging.Message($"[TexturePatcher] Processing scene: {sceneName}");
            yield return null;

            if (currentLevel != sceneName)
            {
                ResetInternalState();
                currentLevel = sceneName;
            }

            currentReplacements = new Dictionary<string, Texture2D>();
            yield return LoadTextures(globalTextureReplacements);

            var levelSpecific = GetLevelSpecificTextures(sceneName);
            if (levelSpecific?.Count > 0)
                yield return LoadTextures(levelSpecific);

            if (currentReplacements.Count == 0 && rankSprites.Count == 0)
            {
                Logging.Warn("[TexturePatcher] No textures or rank sprites were loaded, skipping patching");
                isProcessing = false;
                yield break;
            }

            yield return ReplaceTexturesInScene(true);
            yield return UpdateStyleHUD();
            ReplaceUISprites();

            if (backgroundChecker != null)
                coroutineStarter.StopCoroutine(backgroundChecker);

            backgroundChecker = coroutineStarter.StartCoroutine(BackgroundTextureCheck());
            isProcessing = false;
        }

        private static void ResetInternalState()
        {
            ClearTextureCache();
            ClearRankSprites();
            processedObjectIds.Clear();
            currentReplacements = null;
            currentLevel = null;
            isProcessing = false;
        }

        private static void ClearTextureCache()
        {
            foreach (var tex in textureCache.Values)
                Object.Destroy(tex);
            textureCache.Clear();
        }

        private static void ClearRankSprites()
        {
            foreach (var sprite in rankSprites.Values)
                Object.Destroy(sprite);
            rankSprites.Clear();
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
            foreach (var entry in textureMap)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                    yield break;

                string key = entry.Key;
                string filename = entry.Value.filename;
                string type = entry.Value.type;

                if (currentReplacements.ContainsKey(key) || rankSprites.ContainsKey(key))
                    continue;

                Texture2D loaded = null;
                yield return coroutineStarter.StartCoroutine(LoadTexture(filename, tex => loaded = tex));

                if (loaded == null)
                {
                    Logging.Warn($"[TexturePatcher] Failed to load texture: {filename}");
                    continue;
                }

                loaded.filterMode = FilterMode.Point;

                if (type == "sprite")
                {
                    float ppu = loaded.height;
                    var sprite = Sprite.Create(
                        loaded,
                        new Rect(0, 0, loaded.width, loaded.height),
                        new Vector2(0.5f, 0.5f),
                        ppu
                    );

                    rankSprites[key] = sprite;
                    Logging.Message($"[TexturePatcher] Loaded sprite '{filename}' as '{key}'");
                }
                else
                {
                    currentReplacements[key] = loaded;
                    Logging.Message($"[TexturePatcher] Loaded texture '{filename}' as '{key}'");
                }
            }
        }


        private static void StartBackgroundCheck()
        {
            if (backgroundChecker != null)
                coroutineStarter.StopCoroutine(backgroundChecker);

            backgroundChecker = coroutineStarter.StartCoroutine(BackgroundTextureCheck());
        }
        private static IEnumerator BackgroundTextureCheck()
        {
            while (!cancellationTokenSource.IsCancellationRequested && !string.IsNullOrEmpty(currentLevel))
            {
                float waitTime = GetSceneCheckDelay(currentLevel);
                yield return new WaitForSeconds(waitTime);

                if ((currentReplacements != null && currentReplacements.Count > 0) || rankSprites.Count > 0)
                {
                    Logging.Debug($"[TexturePatcher] Background check running (delay {waitTime}s)");
                    yield return ReplaceTexturesInScene(false);
                    yield return UpdateStyleHUD();
                }
            }

            Logging.Debug("[TexturePatcher] Background texture check ended.");
        }

        private static float GetSceneCheckDelay(string sceneName)
        {
            if (sceneName.IndexOf("4-S", StringComparison.OrdinalIgnoreCase) >= 0)
                return 3f;

            return 0.25f;
        }


        private static bool ShouldCancel() =>
            cancellationTokenSource?.IsCancellationRequested ?? false;
        private static readonly string[] TextureProps = { "_MainTex", "_BaseMap", "_DetailAlbedoMap", "_Texture", "_MainTexture", "_EmissiveTex" };

        private static IEnumerator ReplaceTexturesInScene(bool isInitialPass)
        {
            if (currentReplacements == null || cancellationTokenSource.IsCancellationRequested)
                yield break;

            Camera mainCam = Camera.main;
            int processed = 0;
            const int maxPerFrame = 5;

            foreach (var rend in Object.FindObjectsOfType<Renderer>())
            {
                if (!IsValidRenderer(rend, mainCam))
                    continue;

                int id = rend.GetInstanceID();
                if (!processedObjectIds.Add(id))
                    continue;

                bool modified = false;
                var materials = rend.materials;

                foreach (var mat in materials)
                {
                    if (mat == null) continue;

                    foreach (var prop in TextureProps)
                    {
                        if (!mat.HasProperty(prop)) continue;

                        Texture2D curTex = mat.GetTexture(prop) as Texture2D;
                        if (curTex == null) continue;

                        if (TryGetReplacement(curTex.name, out var replacement))
                        {
                            mat.SetTexture(prop, replacement);
                            modified = true;
                            processed++;
                        }

                        if (processed >= maxPerFrame)
                        {
                            processed = 0;
                            yield return null;
                        }
                    }
                }

                if (modified)
                    rend.materials = materials;
            }

            foreach (var raw in Object.FindObjectsOfType<RawImage>())
            {
                if (!IsValidRawImage(raw)) continue;

                int id = raw.GetInstanceID();
                if (!processedObjectIds.Add(id)) continue;

                Texture2D curTex = raw.texture as Texture2D;
                if (curTex == null) continue;

                if (TryGetReplacement(curTex.name, out var replacement))
                {
                    raw.texture = replacement;
                    processed++;

                    if (processed >= maxPerFrame)
                    {
                        processed = 0;
                        yield return null;
                    }
                }
            }
        }

        private static bool TryGetReplacement(string textureName, out Texture2D replacement)
        {
            return currentReplacements.TryGetValue(textureName, out replacement)
                || currentReplacements.TryGetValue(textureName.ToLower(), out replacement);
        }

        private static bool IsValidRenderer(Renderer rend, Camera mainCam)
        {
            if (rend == null || rend.gameObject == null || !rend.gameObject.activeInHierarchy)
                return false;

            if (mainCam && (rend.gameObject == mainCam.gameObject || rend.GetComponentInParent<Camera>(true) != null))
                return false;

            return true;
        }

        private static bool IsValidRawImage(RawImage raw)
        {
            return raw != null && raw.gameObject.activeInHierarchy;
        }

        private static IEnumerator UpdateStyleHUD()
        {
            var hud = MonoSingleton<StyleHUD>.Instance;
            if (hud == null || rankSprites.Count == 0)
                yield break;

            int max = Math.Min(hud.ranks.Count, 8);

            for (int i = 0; i < max; i++)
            {
                string rankName = GetRankNameByIndex(i);
                if (rankSprites.TryGetValue(rankName, out var sprite))
                {
                    hud.ranks[i].sprite = sprite;
                }
                else
                {
                    Logging.Warn($"[TexturePatcher] Missing sprite for rank: {rankName}");
                }
            }

            string currentRank = GetRankNameByIndex(hud.rankIndex);
            if (rankSprites.TryGetValue(currentRank, out var currentSprite))
            {
                hud.rankImage.sprite = currentSprite;
            }
            else
            {
                Logging.Warn($"[TexturePatcher] Missing sprite for current rank: {currentRank}");
            }

            yield return null;
        }


        private static readonly string[] RankNames = { "RankD", "RankC", "RankB", "RankA", "RankS", "RankSS", "RankSSS", "RankU" };

        private static string GetRankNameByIndex(int i)
        {
            if (i >= 0 && i < RankNames.Length)
                return RankNames[i];

            return "RankD";
        }

        [HarmonyPatch(typeof(StyleHUD), "Start")]
        private static class StyleHUD_Start_Patch
        {
            private static void Postfix(StyleHUD __instance)
            {
                if (__instance == null || rankSprites.Count == 0)
                    return;

                string rankName = GetRankNameByIndex(__instance.rankIndex);
                if (rankSprites.TryGetValue(rankName, out var sprite))
                {
                    __instance.rankImage.sprite = sprite;
                }
                else
                {
                    Logging.Warn($"[TexturePatcher] Missing sprite for rank at start: {rankName}");
                }
            }
        }


        private static IEnumerator LoadTexture(string filename, Action<Texture2D> callback)
        {
            if (cancellationTokenSource.IsCancellationRequested)
            {
                callback(null);
                yield break;
            }

            if (textureCache.TryGetValue(filename, out var cached))
            {
                callback(cached);
                yield break;
            }

            string fullPath = FindTextureFile(filename);
            if (string.IsNullOrEmpty(fullPath))
            {
                Logging.Warn($"[TexturePatcher] File not found: {filename}");
                callback(null);
                yield break;
            }

            byte[] fileData = null;
            Exception error = null;
            bool isDone = false;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    if (!cancellationTokenSource.IsCancellationRequested)
                        fileData = File.ReadAllBytes(fullPath);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
                finally
                {
                    isDone = true;
                }
            });

            while (!isDone && !cancellationTokenSource.IsCancellationRequested)
                yield return null;

            if (cancellationTokenSource.IsCancellationRequested)
            {
                callback(null);
                yield break;
            }

            if (error != null)
            {
                Logging.Warn($"[TexturePatcher] Error loading '{filename}': {error.Message}");
                callback(null);
                yield break;
            }

            if (fileData == null || fileData.Length == 0)
            {
                Logging.Warn($"[TexturePatcher] Empty or unreadable file: {filename}");
                callback(null);
                yield break;
            }

            var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false)
            {
                name = Path.GetFileNameWithoutExtension(fullPath),
                filterMode = FilterMode.Point,
                anisoLevel = 0
            };

            if (!tex.LoadImage(fileData))
            {
                Logging.Warn($"[TexturePatcher] Failed to decode image: {filename}");
                callback(null);
                yield break;
            }

            textureCache[filename] = tex;
            callback(tex);
        }

        private static string FindTextureFile(string filename)
        {
            string[] extensions = { ".png", ".jpg", ".jpeg", ".tga" };

            foreach (var ext in extensions)
            {
                string full = Path.Combine(texturesFolder, filename + ext);
                if (File.Exists(full))
                    return full;
            }

            string raw = Path.Combine(texturesFolder, filename);
            return File.Exists(raw) ? raw : null;
        }

        private static void ReplaceUISprites()
        {
            var images = GameObject.FindObjectsOfType<Image>(true);

            foreach (var img in images)
            {
                if (img == null || img.sprite == null)
                    continue;

                string spriteName = img.sprite.name;

                if (rankSprites.TryGetValue(spriteName, out var replacement))
                {
                    img.sprite = replacement;
                    Logging.Message($"[TexturePatcher] Sprite replaced: '{spriteName}' → in object '{img.gameObject.name}'");
                }
                else
                {
                    Logging.Debug($"[TexturePatcher] No replacement found for sprite: '{spriteName}'");
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
                textureCache.Clear();
                Logging.Message("[TexturePatcher] Coroutine destroyed and cache cleared");
            }
        }
    }
}