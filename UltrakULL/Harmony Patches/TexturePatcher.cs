using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UltrakULL.Harmony_Patches
{
    /// <summary>
    /// Patches textures and sprites for language-dependent replacements.
    /// </summary>
    [HarmonyPatch]
    public static class TexturePatcher
    {
        // ======================== Constants ========================
        private const int MaxPropertyChangesBeforeYield = 8;
        private const int RenderersScanBatchSize = 60;
        private const float DefaultBackgroundCheckDelay = 0.5f;
        private const float Level4SBackgroundCheckDelay = 3f;
        private const int MinRankCount = 8;

        // ======================== Static fields ========================
        private static bool _initialized;
        private static Dictionary<string, Dictionary<string, (string filename, string type)>> _levelTextureMappings;
        private static readonly Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();
        private static CoroutineRunner _coroutineRunner;
        private static Dictionary<string, Texture2D> _currentReplacements;
        private static string _currentLevel = string.Empty;
        private static Coroutine _backgroundCheckCoroutine;
        private static bool _isProcessing;
        private static CancellationTokenSource _currentCts;

        private static readonly HashSet<int> _processedRendererIds = new HashSet<int>();
        private static readonly HashSet<int> _processedRawImageIds = new HashSet<int>();
        private static readonly Dictionary<string, Sprite> _rankSprites = new Dictionary<string, Sprite>();

        private static readonly List<string> IgnoredPathPatterns = new List<string> { "Leaderboard/Container/Entry Template" };

        // Global replacements (scene-independent)
        private static readonly Dictionary<string, (string filename, string type)> GlobalTextureReplacements =
            new Dictionary<string, (string, string)>
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

        private static readonly HashSet<string> IgnoredScenes = new HashSet<string> { "Bootstrap", "Intro", "Loading" };

        private static readonly string[] TextureProps =
        {
            "_MainTex", "_BaseMap", "_DetailAlbedoMap", "_Texture", "_MainTexture", "_EmissiveTex"
        };

        private static readonly int[] TexturePropIDs = TextureProps.Select(Shader.PropertyToID).ToArray();

        private static readonly string[] RankNames = { "RankD", "RankC", "RankB", "RankA", "RankS", "RankSS", "RankSSS", "RankU" };

        private static string TexturesFolder => Path.Combine(Paths.ConfigPath, "ultrakull", "textures", UltrakULL.json.LanguageManager.CurrentLanguage.metadata.langName) + Path.DirectorySeparatorChar;

        // ======================== Initialization ========================
        [HarmonyPrepare]
        private static void Prepare()
        {
            EnsureTexturesFolderExists();
            InitializeTextureMappings();
            Logging.Message("[TexturePatcher] Module initialized");
        }

        private static void EnsureTexturesFolderExists()
        {
            if (!Directory.Exists(TexturesFolder))
            {
                Directory.CreateDirectory(TexturesFolder);
                Logging.Message("[TexturePatcher] Created texture folder: " + TexturesFolder);
            }
        }

        private static void InitializeTextureMappings()
        {
            if (_initialized) return;

            _levelTextureMappings = new Dictionary<string, Dictionary<string, (string, string)>>
            {
                {
                    "Main Menu",
                    new Dictionary<string, (string, string)>
                    {
                        { "TextmodeLogo", ("TextmodeLogo", "sprite") },
                        { "TextmodeCircuit", ("TextmodeCircuit", "texture") }
                    }
                },
                {
                    "Tutorial",
                    new Dictionary<string, (string, string)> { { "", ("Batch Tutorial", "texture") } }
                },
                {
                    "Level 0-1",
                    new Dictionary<string, (string, string)>
                    {
                        { "logowideborderless", ("logowideborderless", "sprite") },
                        { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") },
                        { "SignWarning", ("SignWarning", "texture") },
                        { "SignCoolingChamber", ("SignCoolingChamber", "texture") },
                        { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") },
                        { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") }
                    }
                },
                {
                    "Level 0-2",
                    new Dictionary<string, (string, string)>
                    {
                        { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") },
                        { "SignWarning", ("SignWarning", "texture") },
                        { "SignCoolingChamber", ("SignCoolingChamber", "texture") },
                        { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") },
                        { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") }
                    }
                },
                {
                    "Level 0-3",
                    new Dictionary<string, (string, string)>
                    {
                        { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") },
                        { "SignWarning", ("SignWarning", "texture") },
                        { "SignCoolingChamber", ("SignCoolingChamber", "texture") },
                        { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") },
                        { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") }
                    }
                },
                {
                    "Level 0-4",
                    new Dictionary<string, (string, string)>
                    {
                        { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") },
                        { "SignWarning", ("SignWarning", "texture") },
                        { "SignCoolingChamber", ("SignCoolingChamber", "texture") },
                        { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") },
                        { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") }
                    }
                },
                {
                    "Level 0-5",
                    new Dictionary<string, (string, string)>
                    {
                        { "abandonhope2", ("abandonhope2", "texture") },
                        { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") },
                        { "SignWarning", ("SignWarning", "texture") },
                        { "SignCoolingChamber", ("SignCoolingChamber", "texture") },
                        { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") },
                        { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") }
                    }
                },
                {
                    "Level 1-4",
                    new Dictionary<string, (string, string)> { { "forgiveme", ("forgiveme", "texture") } }
                },
                {
                    "Level 2-2",
                    new Dictionary<string, (string, string)>
                    {
                        { "electricitybox", ("electricitybox", "texture") },
                        { "", ("Batch 2-2", "texture") }
                    }
                },
                {
                    "Level 2-3",
                    new Dictionary<string, (string, string)>
                    {
                        { "", ("Batch 2-3", "texture") },
                        { "watercontrol1", ("watercontrol1", "texture") },
                        { "watercontrol2", ("watercontrol2", "texture") }
                    }
                },
                {
                    "Level 4-3",
                    new Dictionary<string, (string, string)>
                    {
                        { "traitor", ("traitor", "texture") },
                        { "", ("Batch 4-3", "texture") }
                    }
                },
                {
                    "Level 5-1",
                    new Dictionary<string, (string, string)>
                    {
                        { "WaterProcessingAttention", ("WaterProcessingAttention", "texture") },
                        { "", ("Batch 5-1", "texture") }
                    }
                },
                {
                    "Level 5-S",
                    new Dictionary<string, (string, string)> { { "", ("Batch 5-S", "texture") } }
                },
                {
                    "Level 7-2",
                    new Dictionary<string, (string, string)>
                    {
                        { "", ("Batch 7-2", "texture") },
                        { "exit", ("exit", "texture") }
                    }
                },
                {
                    "Level 7-3",
                    new Dictionary<string, (string, string)> { { "marble_inverted 3", ("marble_inverted 3", "texture") } }
                },
                {
                    "Level 7-4",
                    new Dictionary<string, (string, string)>
                    {
                        { "HotPipeSign", ("HotPipeSign", "texture") },
                        { "T_Cent_PlantRoom", ("T_Cent_PlantRoom", "texture") }
                    }
                },
                {
                    "Level 7-S",
                    new Dictionary<string, (string, string)> { { "T_Placard", ("T_Placard", "texture") } }
                },
				{
                    "Level 8-1",
                    new Dictionary<string, (string, string)> { 
						{ "", ("Batch 8-1", "texture") },
						{ "ArchangelNamePlateRaphael", ("ArchangelNamePlateRaphael", "texture") },
						{ "ArchangelNamePlatePhanuel", ("ArchangelNamePlatePhanuel", "texture") },
						{ "ArchangelNamePlateMichael", ("ArchangelNamePlateMichael", "texture") },
						{ "ArchangelNamePlateGabriel", ("ArchangelNamePlateGabriel", "texture") },
						{ "T_LionPlaque", ("T_LionPlaque", "texture") },
						{ "wecamein", ("wecamein", "texture") },
						{ "wecamein2", ("wecamein2", "texture") }
						}
                },
				{
                    "Level 8-2",
                    new Dictionary<string, (string, string)> { 
						{ "", ("Batch 8-2", "texture") },
						{ "ad_fox 1", ("ad_fox 1", "texture") },
						{ "big_hakita", ("big_hakita", "texture") },
						{ "inthemirror", ("inthemirror", "texture") },
						{ "OfficeMaintenance", ("OfficeMaintenance", "texture") },
						{ "presentation2", ("presentation2", "texture") },
						{ "VendingMachine", ("VendingMachine", "texture") },
						{ "StatsBoard", ("StatsBoard", "texture") },
						{ "OfficeArchive", ("OfficeArchive", "texture") }
						}
                },
                {
                    "Level 8-3",
                    new Dictionary<string, (string, string)> { 
						{ "", ("Batch 8-3", "texture") },
                        { "SignWarning", ("SignWarning", "texture") },
                        //Additional paths, just in case
						{ "ad_fox 1", ("ad_fox 1", "texture") },
						{ "big_hakita", ("big_hakita", "texture") },
						{ "VendingMachine", ("VendingMachine", "texture") },
						{ "StatsBoard", ("StatsBoard", "texture") },
						{ "OfficeArchive", ("OfficeArchive", "texture") }
						}
                },
                {
                    "Level 8-4",
                    new Dictionary<string, (string, string)> { 
						{ "", ("Batch 8-4", "texture") },
                        { "SignWarning", ("SignWarning", "texture") },
                        { "CityoftheDeadSunPoster", ("CityoftheDeadSunPoster", "texture") }
						}
                },
                {
                    "Level 0-E",
                    new Dictionary<string, (string, string)>
                    {
                        { "exit", ("exit", "texture") },
                        { "abandonhope2", ("abandonhope2", "texture") },
                        { "SignSecurityInstructions", ("SignSecurityInstructions", "texture") },
                        { "SignWarning", ("SignWarning", "texture") },
                        { "SignCoolingChamber", ("SignCoolingChamber", "texture") },
                        { "SignSecurityLockdown", ("SignSecurityLockdown", "texture") },
                        { "SignSecurityCheckpoint", ("SignSecurityCheckpoint", "texture") }
                    }
                },
                {
                    "uk_construct",
                    new Dictionary<string, (string, string)> { { "garry", ("garry", "sprite") } }
                },
                {
                    "CreditsMuseum2",
                    new Dictionary<string, (string, string)>
                    {
                        { "sign_map_Texture_2", ("sign_map_Texture_К2", "texture") },
                        { "poster", ("poster", "texture") },
                        { "Staff only sign_texture", ("Staff only sign_texture", "texture") }
                    }
                }
            };

            _initialized = true;
            Logging.Message($"[TexturePatcher] Loaded {GlobalTextureReplacements.Count} global and {_levelTextureMappings.Count} level-specific mappings");
        }

        // ======================== Harmony patches ========================
        [HarmonyPatch(typeof(StyleHUD), "Start")]
        private static class StyleHUD_Start_Patch
        {
            private static void Postfix(StyleHUD __instance)
            {
                if (__instance == null || _rankSprites.Count == 0) return;

                string rankName = GetRankNameByIndex(__instance.rankIndex);
                if (_rankSprites.TryGetValue(rankName, out var sprite))
                    __instance.rankImage.sprite = sprite;
                else
                    Logging.Warn($"[TexturePatcher] Missing sprite for rank at start: {rankName}");
            }
        }

        [HarmonyPatch(typeof(SceneHelper), "OnSceneLoaded")]
        [HarmonyPostfix]
        private static void OnSceneLoaded()
        {
            // Safely cancel previous operation
            _currentCts?.Cancel();
            _currentCts?.Dispose();
            _currentCts = new CancellationTokenSource();

            if (_coroutineRunner == null)
                _coroutineRunner = CoroutineRunner.Instance;

            _coroutineRunner.StartCoroutine(ProcessSceneChange(_currentCts.Token));
        }

        // ======================== Core logic ========================
        private static IEnumerator ProcessSceneChange(CancellationToken token)
        {
            if (_isProcessing)
            {
                Logging.Debug("[TexturePatcher] Already processing a scene change, skipping.");
                yield break;
            }

            _isProcessing = true;
            try
            {
                string sceneName = CommonFunctions.GetCurrentSceneName();
                if (string.IsNullOrEmpty(sceneName) || token.IsCancellationRequested)
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

                if (!_initialized)
                {
                    Logging.Warn("[TexturePatcher] Not initialized");
                    ResetInternalState();
                    yield break;
                }

                Logging.Message($"[TexturePatcher] Processing scene: {sceneName}");

                yield return null; // Wait one frame for stability

                if (_currentLevel != sceneName)
                {
                    ResetInternalState();
                    _currentLevel = sceneName;
                }

                _currentReplacements = new Dictionary<string, Texture2D>(StringComparer.OrdinalIgnoreCase);
                yield return LoadTextures(GlobalTextureReplacements, token);

                var levelSpecific = GetLevelSpecificTextures(sceneName);
                if (levelSpecific != null && levelSpecific.Count > 0)
                    yield return LoadTextures(levelSpecific, token);

                if (_currentReplacements.Count == 0 && _rankSprites.Count == 0)
                {
                    Logging.Warn("[TexturePatcher] No textures or rank sprites were loaded, skipping patching");
                    yield break;
                }

                yield return ReplaceTexturesInScene(isInitialPass: true, token);
                yield return UpdateStyleHUD();
                ReplaceUISprites();

                // Start background checker if not already running
                if (_backgroundCheckCoroutine != null)
                    _coroutineRunner.StopCoroutine(_backgroundCheckCoroutine);
                _backgroundCheckCoroutine = _coroutineRunner.StartCoroutine(BackgroundTextureCheck(token));
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private static IEnumerator BackgroundTextureCheck(CancellationToken token)
        {
            while (!token.IsCancellationRequested && !string.IsNullOrEmpty(_currentLevel))
            {
                float delay = _currentLevel.IndexOf("4-S", StringComparison.OrdinalIgnoreCase) >= 0
                    ? Level4SBackgroundCheckDelay
                    : DefaultBackgroundCheckDelay;
                yield return new WaitForSeconds(delay);

                if ((_currentReplacements != null && _currentReplacements.Count > 0) || _rankSprites.Count > 0)
                {
                    yield return ReplaceTexturesInScene(isInitialPass: false, token);
                    yield return UpdateStyleHUD();
                }
            }
            Logging.Debug("[TexturePatcher] Background texture check ended.");
        }

        private static IEnumerator LoadTextures(Dictionary<string, (string filename, string type)> textureMap, CancellationToken token)
        {
            foreach (var pair in textureMap)
            {
                if (token.IsCancellationRequested) yield break;

                string key = pair.Key;
                string filename = pair.Value.filename;
                string type = pair.Value.type;

                if (_currentReplacements.ContainsKey(key) || _rankSprites.ContainsKey(key))
                    continue;

                Texture2D loaded = null;
                yield return LoadTextureAsync(filename, tex => loaded = tex, token);

                if (loaded == null)
                {
                    Logging.Warn($"[TexturePatcher] Failed to load texture: {filename}");
                    continue;
                }

                loaded.filterMode = FilterMode.Point;
                loaded.name = Path.GetFileNameWithoutExtension(filename);

                if (type == "sprite")
                {
                    float pixelsPerUnit = loaded.height;
                    var sprite = Sprite.Create(loaded, new Rect(0, 0, loaded.width, loaded.height),
                        new Vector2(0.5f, 0.5f), pixelsPerUnit);
                    _rankSprites[key] = sprite;
                    Logging.Message($"[TexturePatcher] Loaded sprite '{filename}' as '{key}'");
                }
                else
                {
                    _currentReplacements[key] = loaded;
                    Logging.Message($"[TexturePatcher] Loaded texture '{filename}' as '{key}'");
                }
            }
        }

        private static IEnumerator LoadTextureAsync(string filename, Action<Texture2D> callback, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                callback(null);
                yield break;
            }

            if (_textureCache.TryGetValue(filename, out var cached))
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
                    if (!token.IsCancellationRequested)
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

            while (!isDone && !token.IsCancellationRequested)
                yield return null;

            if (token.IsCancellationRequested)
            {
                callback(null);
                yield break;
            }

            if (error != null)
            {
                Logging.Error($"[TexturePatcher] Error loading '{filename}': {error}");
                callback(null);
                yield break;
            }

            if (fileData == null || fileData.Length == 0)
            {
                Logging.Warn($"[TexturePatcher] Empty or unreadable file: {filename}");
                callback(null);
                yield break;
            }

            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            if (!ImageConversion.LoadImage(texture, fileData))
            {
                Logging.Warn($"[TexturePatcher] Failed to decode image: {filename}");
                Object.Destroy(texture);
                callback(null);
                yield break;
            }

            _textureCache[filename] = texture;
            callback(texture);
        }

        private static IEnumerator ReplaceTexturesInScene(bool isInitialPass, CancellationToken token)
        {
            if (_currentReplacements == null || token.IsCancellationRequested)
                yield break;

            Camera mainCam = Camera.main;
            int changesSinceYield = 0;
            int renderersScanned = 0;

            // Replace in Renderers
            foreach (var renderer in Object.FindObjectsOfType<Renderer>())
            {
                if (!IsValidRenderer(renderer, mainCam) || IsInIgnoredPath(renderer.gameObject))
                    continue;

                int id = renderer.GetInstanceID();
                if (!_processedRendererIds.Add(id))
                    continue;

                var materials = renderer.sharedMaterials;
                var propertyBlock = new MaterialPropertyBlock();

                for (int m = 0; m < materials.Length; m++)
                {
                    var mat = materials[m];
                    if (mat == null) continue;

                    bool modified = false;
                    propertyBlock.Clear();
                    renderer.GetPropertyBlock(propertyBlock, m);

                    for (int p = 0; p < TexturePropIDs.Length; p++)
                    {
                        int propId = TexturePropIDs[p];
                        if (!mat.HasProperty(propId)) continue;

                        var texture = mat.GetTexture(propId) as Texture2D;
                        if (texture == null) continue;

                        if (TryGetReplacement(texture.name, out var replacement))
                        {
                            propertyBlock.SetTexture(propId, replacement);
                            modified = true;
                            changesSinceYield++;
                        }
                    }

                    if (modified)
                        renderer.SetPropertyBlock(propertyBlock, m);

                    if (changesSinceYield >= MaxPropertyChangesBeforeYield)
                    {
                        changesSinceYield = 0;
                        yield return null;
                        if (token.IsCancellationRequested) yield break;
                    }
                }

                renderersScanned++;
                if (renderersScanned % RenderersScanBatchSize == 0)
                    yield return null;
            }

            // Replace in RawImages
            int rawImagesScanned = 0;
            foreach (var rawImage in Object.FindObjectsOfType<RawImage>())
            {
                if (!IsValidRawImage(rawImage) || IsInIgnoredPath(rawImage.gameObject))
                    continue;

                int id = rawImage.GetInstanceID();
                if (!_processedRawImageIds.Add(id))
                    continue;

                var texture = rawImage.texture as Texture2D;
                if (texture != null && TryGetReplacement(texture.name, out var replacement))
                {
                    rawImage.texture = replacement;
                    changesSinceYield++;
                }

                rawImagesScanned++;
                if (changesSinceYield >= MaxPropertyChangesBeforeYield || rawImagesScanned % RenderersScanBatchSize == 0)
                {
                    changesSinceYield = 0;
                    yield return null;
                    if (token.IsCancellationRequested) yield break;
                }
            }
        }

        private static IEnumerator UpdateStyleHUD()
        {
            var instance = MonoSingleton<StyleHUD>.Instance;
            if (instance == null || _rankSprites.Count == 0)
                yield break;

            int count = Math.Min(instance.ranks.Count, MinRankCount);
            for (int i = 0; i < count; i++)
            {
                string rankName = GetRankNameByIndex(i);
                if (_rankSprites.TryGetValue(rankName, out var sprite))
                    instance.ranks[i].sprite = sprite;
                else
                    Logging.Warn($"[TexturePatcher] Missing sprite for rank: {rankName}");
            }

            string currentRank = GetRankNameByIndex(instance.rankIndex);
            if (_rankSprites.TryGetValue(currentRank, out var currentSprite))
                instance.rankImage.sprite = currentSprite;
            else
                Logging.Warn($"[TexturePatcher] Missing sprite for current rank: {currentRank}");

            yield return null;
        }

        private static void ReplaceUISprites()
        {
            int replaced = 0;
            foreach (var image in Object.FindObjectsOfType<Image>(true))
            {
                if (image?.sprite == null) continue;
                string spriteName = image.sprite.name;
                if (_rankSprites.TryGetValue(spriteName, out var newSprite))
                {
                    image.sprite = newSprite;
                    replaced++;
                }
            }
            if (replaced > 0)
                Logging.Message($"[TexturePatcher] Replaced {replaced} UI sprites");
        }

        // ======================== Helper methods ========================
        private static bool TryGetReplacement(string textureName, out Texture2D replacement)
        {
            if (_currentReplacements != null && _currentReplacements.TryGetValue(textureName, out replacement))
                return true;
            replacement = null;
            return false;
        }

        private static string GetRankNameByIndex(int index)
        {
            return (index >= 0 && index < RankNames.Length) ? RankNames[index] : "RankD";
        }

        private static Dictionary<string, (string filename, string type)> GetLevelSpecificTextures(string sceneName)
        {
            foreach (var kvp in _levelTextureMappings)
            {
                if (sceneName.Contains(kvp.Key))
                    return kvp.Value;
            }
            return null;
        }

        private static string FindTextureFile(string filename)
        {
            string[] extensions = { ".png", ".jpg", ".jpeg", ".tga" };
            foreach (string ext in extensions)
            {
                string path = Path.Combine(TexturesFolder, filename + ext);
                if (File.Exists(path)) return path;
            }
            string noExtPath = Path.Combine(TexturesFolder, filename);
            return File.Exists(noExtPath) ? noExtPath : null;
        }

        private static bool IsValidRenderer(Renderer rend, Camera mainCam)
        {
            if (rend == null || rend.gameObject == null || !rend.gameObject.activeInHierarchy)
                return false;
            if (mainCam != null && (rend.gameObject == mainCam.gameObject || rend.GetComponentInParent<Camera>(true) != null))
                return false;
            return true;
        }

        private static bool IsValidRawImage(RawImage raw)
        {
            return raw != null && raw.gameObject.activeInHierarchy;
        }

        private static bool IsInIgnoredPath(GameObject obj)
        {
            string path = GetHierarchyPath(obj.transform);
            return IgnoredPathPatterns.Any(pattern => path.Contains(pattern));
        }

        private static string GetHierarchyPath(Transform t)
        {
            var parts = new List<string>();
            while (t != null)
            {
                parts.Add(t.name);
                t = t.parent;
            }
            parts.Reverse();
            return string.Join("/", parts);
        }

        private static bool ShouldIgnoreScene(string sceneName)
        {
            return IgnoredScenes.Contains(sceneName);
        }

        private static void ResetInternalState()
        {
            ClearTextureCache();
            ClearRankSprites();
            _processedRendererIds.Clear();
            _processedRawImageIds.Clear();
            _currentReplacements = null;
            _currentLevel = null;
            _isProcessing = false;
        }

        private static void ClearTextureCache()
        {
            foreach (var tex in _textureCache.Values)
                Object.Destroy(tex);
            _textureCache.Clear();
        }

        private static void ClearRankSprites()
        {
            foreach (var spr in _rankSprites.Values)
                Object.Destroy(spr);
            _rankSprites.Clear();
        }
    }

    /// <summary>
    /// Helper MonoBehaviour to run coroutines without scene dependency.
    /// </summary>
    internal class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;
        public static CoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("TexturePatcher_CoroutineRunner");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<CoroutineRunner>();
                }
                return _instance;
            }
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}