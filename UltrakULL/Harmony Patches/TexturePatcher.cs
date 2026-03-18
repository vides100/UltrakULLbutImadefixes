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
using Object = UnityEngine.Object;

namespace UltrakULL.Harmony_Patches
{
	[HarmonyPatch]
	public static class TexturePatcher
	{
		[HarmonyPatch(typeof(StyleHUD), "Start")]
		private static class StyleHUD_Start_Patch
		{
			private static void Postfix(StyleHUD __instance)
			{
				if (!((Object)(object)__instance == (Object)null) && rankSprites.Count != 0)
				{
					string rankNameByIndex = GetRankNameByIndex(__instance.rankIndex);
					if (rankSprites.TryGetValue(rankNameByIndex, out var value))
					{
						__instance.rankImage.sprite = value;
					}
					else
					{
						Logging.Warn("[TexturePatcher] Missing sprite for rank at start: " + rankNameByIndex);
					}
				}
			}
		}

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

		private static readonly List<string> IgnoredPathPatterns = new List<string> { "Leaderboard/Container/Entry Template" };

		private static readonly Dictionary<string, (string filename, string type)> globalTextureReplacements = new Dictionary<string, (string, string)>
		{
			{
				"checkpoint",
				("Checkpoint", "texture")
			},
			{
				"T_ShopTerminal",
				("T_ShopTerminal", "texture")
			},
			{
				"T_ShopTerminal_Emission",
				("T_ShopTerminal_Emission", "texture")
			},
			{
				"RankD",
				("RankD", "sprite")
			},
			{
				"RankC",
				("RankC", "sprite")
			},
			{
				"RankB",
				("RankB", "sprite")
			},
			{
				"RankA",
				("RankA", "sprite")
			},
			{
				"RankS",
				("RankS", "sprite")
			},
			{
				"RankSS",
				("RankSS", "sprite")
			},
			{
				"RankSSS",
				("RankSSS", "sprite")
			},
			{
				"RankU",
				("RankU", "sprite")
			}
		};

		private static readonly HashSet<string> ignoredScenes = new HashSet<string> { "Bootstrap", "Intro", "Loading" };

		private static readonly string[] TextureProps = new string[6] { "_MainTex", "_BaseMap", "_DetailAlbedoMap", "_Texture", "_MainTexture", "_EmissiveTex" };

		private static readonly int[] TexturePropIDs = ((IEnumerable<string>)TextureProps).Select((Func<string, int>)Shader.PropertyToID).ToArray();

		private static readonly string[] RankNames = new string[8] { "RankD", "RankC", "RankB", "RankA", "RankS", "RankSS", "RankSSS", "RankU" };

		private static string texturesFolder => Path.Combine(Paths.ConfigPath, "ultrakull", "textures", LanguageManager.CurrentLanguage.metadata.langName) + Path.DirectorySeparatorChar;

		private static void EnsureTexturesFolderExists()
		{
			if (!Directory.Exists(texturesFolder))
			{
				Directory.CreateDirectory(texturesFolder);
				Logging.Message("[TexturePatcher] Created texture folder: " + texturesFolder);
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
			if (!initialized)
			{
				levelTextureMappings = new Dictionary<string, Dictionary<string, (string, string)>>
				{
					{
						"Main Menu",
						new Dictionary<string, (string, string)>
						{
							{
								"TextmodeLogo",
								("TextmodeLogo", "sprite")
							},
							{
								"TextmodeCircuit",
								("TextmodeCircuit", "texture")
							}
						}
					},
					{
						"Tutorial",
						new Dictionary<string, (string, string)> { 
						{
							"",
							("Batch Tutorial", "texture")
						} }
					},
					{
						"Level 0-1",
						new Dictionary<string, (string, string)>
						{
							{
								"logowideborderless",
								("logowideborderless", "sprite")
							},
							{
								"SignSecurityInstructions",
								("SignSecurityInstructions", "texture")
							},
							{
								"SignWarning",
								("SignWarning", "texture")
							},
							{
								"SignCoolingChamber",
								("SignCoolingChamber", "texture")
							},
							{
								"SignSecurityLockdown",
								("SignSecurityLockdown", "texture")
							},
							{
								"SignSecurityCheckpoint",
								("SignSecurityCheckpoint", "texture")
							}
						}
					},
					{
						"Level 0-2",
						new Dictionary<string, (string, string)>
						{
							{
								"SignSecurityInstructions",
								("SignSecurityInstructions", "texture")
							},
							{
								"SignWarning",
								("SignWarning", "texture")
							},
							{
								"SignCoolingChamber",
								("SignCoolingChamber", "texture")
							},
							{
								"SignSecurityLockdown",
								("SignSecurityLockdown", "texture")
							},
							{
								"SignSecurityCheckpoint",
								("SignSecurityCheckpoint", "texture")
							}
						}
					},
					{
						"Level 0-3",
						new Dictionary<string, (string, string)>
						{
							{
								"SignSecurityInstructions",
								("SignSecurityInstructions", "texture")
							},
							{
								"SignWarning",
								("SignWarning", "texture")
							},
							{
								"SignCoolingChamber",
								("SignCoolingChamber", "texture")
							},
							{
								"SignSecurityLockdown",
								("SignSecurityLockdown", "texture")
							},
							{
								"SignSecurityCheckpoint",
								("SignSecurityCheckpoint", "texture")
							}
						}
					},
					{
						"Level 0-4",
						new Dictionary<string, (string, string)>
						{
							{
								"SignSecurityInstructions",
								("SignSecurityInstructions", "texture")
							},
							{
								"SignWarning",
								("SignWarning", "texture")
							},
							{
								"SignCoolingChamber",
								("SignCoolingChamber", "texture")
							},
							{
								"SignSecurityLockdown",
								("SignSecurityLockdown", "texture")
							},
							{
								"SignSecurityCheckpoint",
								("SignSecurityCheckpoint", "texture")
							}
						}
					},
					{
						"Level 0-5",
						new Dictionary<string, (string, string)>
						{
							{
								"abandonhope2",
								("abandonhope2", "texture")
							},
							{
								"SignSecurityInstructions",
								("SignSecurityInstructions", "texture")
							},
							{
								"SignWarning",
								("SignWarning", "texture")
							},
							{
								"SignCoolingChamber",
								("SignCoolingChamber", "texture")
							},
							{
								"SignSecurityLockdown",
								("SignSecurityLockdown", "texture")
							},
							{
								"SignSecurityCheckpoint",
								("SignSecurityCheckpoint", "texture")
							}
						}
					},
					{
						"Level 1-4",
						new Dictionary<string, (string, string)> { 
						{
							"forgiveme",
							("forgiveme", "texture")
						} }
					},
					{
						"Level 2-2",
						new Dictionary<string, (string, string)>
						{
							{
								"electricitybox",
								("electricitybox", "texture")
							},
							{
								"",
								("Batch 2-2", "texture")
							}
						}
					},
					{
						"Level 2-3",
						new Dictionary<string, (string, string)>
						{
							{
								"",
								("Batch 2-3", "texture")
							},
							{
								"watercontrol1",
								("watercontrol1", "texture")
							},
							{
								"watercontrol2",
								("watercontrol2", "texture")
							}
						}
					},
					{
						"Level 4-3",
						new Dictionary<string, (string, string)>
						{
							{
								"traitor",
								("traitor", "texture")
							},
							{
								"",
								("Batch 4-3", "texture")
							}
						}
					},
					{
						"Level 5-1",
						new Dictionary<string, (string, string)>
						{
							{
								"WaterProcessingAttention",
								("WaterProcessingAttention", "texture")
							},
							{
								"",
								("Batch 5-1", "texture")
							}
						}
					},
					{
						"Level 5-S",
						new Dictionary<string, (string, string)> { 
						{
							"",
							("Batch 5-S", "texture")
						} }
					},
					{
						"Level 7-2",
						new Dictionary<string, (string, string)>
						{
							{
								"",
								("Batch 7-2", "texture")
							},
							{
								"exit",
								("exit", "texture")
							}
						}
					},
					{
						"Level 7-3",
						new Dictionary<string, (string, string)> { 
						{
							"marble_inverted 3",
							("marble_inverted 3", "texture")
						} }
					},
					{
						"Level 7-4",
						new Dictionary<string, (string, string)>
						{
							{
								"HotPipeSign",
								("HotPipeSign", "texture")
							},
							{
								"T_Cent_PlantRoom",
								("T_Cent_PlantRoom", "texture")
							}
						}
					},
					{
						"Level 7-S",
						new Dictionary<string, (string, string)> { 
						{
							"T_Placard",
							("T_Placard", "texture")
						} }
					},
					{
						"Level 0-E",
						new Dictionary<string, (string, string)>
						{
							{
								"exit",
								("exit", "texture")
							},
							{
								"abandonhope2",
								("abandonhope2", "texture")
							},
							{
								"SignSecurityInstructions",
								("SignSecurityInstructions", "texture")
							},
							{
								"SignWarning",
								("SignWarning", "texture")
							},
							{
								"SignCoolingChamber",
								("SignCoolingChamber", "texture")
							},
							{
								"SignSecurityLockdown",
								("SignSecurityLockdown", "texture")
							},
							{
								"SignSecurityCheckpoint",
								("SignSecurityCheckpoint", "texture")
							}
						}
					},
					{
						"uk_construct",
						new Dictionary<string, (string, string)> { 
						{
							"garry",
							("garry", "sprite")
						} }
					},
					{
						"CreditsMuseum2",
						new Dictionary<string, (string, string)>
						{
							{
								"sign_map_Texture_2",
								("sign_map_Texture_К2", "texture")
							},
							{
								"poster",
								("poster", "texture")
							},
							{
								"Staff only sign_texture",
								("Staff only sign_texture", "texture")
							}
						}
					}
				};
				initialized = true;
				Logging.Message($"[TexturePatcher] Loaded {globalTextureReplacements.Count} global and {levelTextureMappings.Count} level-specific mappings");
			}
		}

		[HarmonyPatch(typeof(SceneHelper), "OnSceneLoaded")]
		[HarmonyPostfix]
		private static void OnSceneLoaded()
		{

			cancellationTokenSource?.Cancel();
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = new CancellationTokenSource();
			if ((Object)(object)coroutineStarter == (Object)null)
			{
				coroutineStarter = (MonoBehaviour)(object)new GameObject("TexturePatcher_CoroutineStarter").AddComponent<DummyMonoBehaviour>();
			}
			coroutineStarter.StartCoroutine(ProcessSceneChange());
		}

		private static IEnumerator ProcessSceneChange()
		{
			if (isProcessing)
			{
				yield break;
			}
			isProcessing = true;
			string sceneName = CommonFunctions.GetCurrentSceneName();
			if (string.IsNullOrEmpty(sceneName) || cancellationTokenSource.IsCancellationRequested)
			{
				Logging.Message("[TexturePatcher] Scene loading aborted");
				ResetInternalState();
				yield break;
			}
			if (ShouldIgnoreScene(sceneName))
			{
				Logging.Message("[TexturePatcher] Ignoring scene: " + sceneName);
				ResetInternalState();
				yield break;
			}
			if (!initialized)
			{
				Logging.Warn("[TexturePatcher] Not initialized");
				ResetInternalState();
				yield break;
			}
			Logging.Message("[TexturePatcher] Processing scene: " + sceneName);
			yield return null;
			if (currentLevel != sceneName)
			{
				ResetInternalState();
				currentLevel = sceneName;
			}
			currentReplacements = new Dictionary<string, Texture2D>();
			yield return LoadTextures(globalTextureReplacements);
			Dictionary<string, (string, string)> levelSpecificTextures = GetLevelSpecificTextures(sceneName);
			if (levelSpecificTextures != null && levelSpecificTextures.Count > 0)
			{
				yield return LoadTextures(levelSpecificTextures);
			}
			if (currentReplacements.Count == 0 && rankSprites.Count == 0)
			{
				Logging.Warn("[TexturePatcher] No textures or rank sprites were loaded, skipping patching");
				isProcessing = false;
				yield break;
			}
			yield return ReplaceTexturesInScene(isInitialPass: true);
			yield return UpdateStyleHUD();
			ReplaceUISprites();
			if (backgroundChecker != null)
			{
				coroutineStarter.StopCoroutine(backgroundChecker);
			}
			backgroundChecker = coroutineStarter.StartCoroutine(BackgroundTextureCheck());
			isProcessing = false;
		}

		private static void ResetInternalState()
		{
			ClearTextureCache();
			ClearRankSprites();
			processedObjectIds.Clear();
			processedRawImages.Clear();
			currentReplacements = null;
			currentLevel = null;
			isProcessing = false;
		}

		private static void ClearTextureCache()
		{
			foreach (Texture2D value in textureCache.Values)
			{
				Object.Destroy((Object)(object)value);
			}
			textureCache.Clear();
		}

		private static void ClearRankSprites()
		{
			foreach (Sprite value in rankSprites.Values)
			{
				Object.Destroy((Object)(object)value);
			}
			rankSprites.Clear();
		}

		private static Dictionary<string, (string filename, string type)> GetLevelSpecificTextures(string sceneName)
		{
			foreach (KeyValuePair<string, Dictionary<string, (string, string)>> levelTextureMapping in levelTextureMappings)
			{
				if (sceneName.Contains(levelTextureMapping.Key))
				{
					return levelTextureMapping.Value;
				}
			}
			return null;
		}

		private static IEnumerator LoadTextures(Dictionary<string, (string filename, string type)> textureMap)
		{
			foreach (KeyValuePair<string, (string, string)> item in textureMap)
			{
				if (cancellationTokenSource.IsCancellationRequested)
				{
					yield break;
				}
				string key = item.Key;
				string filename = item.Value.Item1;
				string type = item.Value.Item2;
				if (currentReplacements.ContainsKey(key) || rankSprites.ContainsKey(key))
				{
					continue;
				}
				Texture2D loaded = null;
				yield return coroutineStarter.StartCoroutine(LoadTexture(filename, delegate(Texture2D tex)
				{
					loaded = tex;
				}));
				if ((Object)(object)loaded == (Object)null)
				{
					Logging.Warn("[TexturePatcher] Failed to load texture: " + filename);
					continue;
				}
				((Texture)loaded).filterMode = (FilterMode)0;
				if (type == "sprite")
				{
					float num = ((Texture)loaded).height;
					Sprite value = Sprite.Create(loaded, new Rect(0f, 0f, (float)((Texture)loaded).width, (float)((Texture)loaded).height), new Vector2(0.5f, 0.5f), num);
					rankSprites[key] = value;
					Logging.Message("[TexturePatcher] Loaded sprite '" + filename + "' as '" + key + "'");
				}
				else
				{
					currentReplacements[key] = loaded;
					Logging.Message("[TexturePatcher] Loaded texture '" + filename + "' as '" + key + "'");
				}
			}
		}

		private static void StartBackgroundCheck()
		{
			if (backgroundChecker != null)
			{
				coroutineStarter.StopCoroutine(backgroundChecker);
			}
			backgroundChecker = coroutineStarter.StartCoroutine(BackgroundTextureCheck());
		}

		private static IEnumerator BackgroundTextureCheck()
		{
			while (!cancellationTokenSource.IsCancellationRequested && !string.IsNullOrEmpty(currentLevel))
			{
				float sceneCheckDelay = GetSceneCheckDelay(currentLevel);
				yield return (object)new WaitForSeconds(sceneCheckDelay);
				if ((currentReplacements != null && currentReplacements.Count > 0) || rankSprites.Count > 0)
				{
					yield return ReplaceTexturesInScene(isInitialPass: false);
					yield return UpdateStyleHUD();
				}
			}
			Logging.Debug("[TexturePatcher] Background texture check ended.");
		}

		private static float GetSceneCheckDelay(string sceneName)
		{
			if (sceneName.IndexOf("4-S", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return 3f;
			}
			return 0.5f;
		}

		private static bool ShouldCancel()
		{
			return cancellationTokenSource?.IsCancellationRequested ?? false;
		}

		private static IEnumerator ReplaceTexturesInScene(bool isInitialPass)
		{
			if (currentReplacements == null || cancellationTokenSource.IsCancellationRequested)
			{
				yield break;
			}
			Camera mainCam = Camera.main;
			int processedChanges = 0;
			int scannedRenderers = 0;
			int scannedRawImages = 0;
			Renderer[] array = Object.FindObjectsOfType<Renderer>();
			Renderer[] array2 = array;
			foreach (Renderer rend in array2)
			{
				if (!IsValidRenderer(rend, mainCam) || IsInIgnoredPath(((Component)rend).gameObject))
				{
					continue;
				}
				int instanceID = ((Object)rend).GetInstanceID();
				if (!processedObjectIds.Add(instanceID))
				{
					continue;
				}
				Material[] sharedMaterials = rend.sharedMaterials;
				MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
				for (int m = 0; m < sharedMaterials.Length; m++)
				{
					Material mat = sharedMaterials[m];
					if ((Object)(object)mat == (Object)null)
					{
						continue;
					}
					bool modified = false;
					propertyBlock.Clear();
					rend.GetPropertyBlock(propertyBlock, m);
					for (int p = 0; p < TexturePropIDs.Length; p++)
					{
						int num = TexturePropIDs[p];
						if (!mat.HasProperty(num))
						{
							continue;
						}
						Texture texture = mat.GetTexture(num);
						Texture2D val = (Texture2D)(object)((texture is Texture2D) ? texture : null);
						if (!((Object)(object)val == (Object)null))
						{
							if (TryGetReplacement(((Object)val).name, out var replacement))
							{
								propertyBlock.SetTexture(num, (Texture)(object)replacement);
								modified = true;
								processedChanges++;
							}
							if (processedChanges >= 8)
							{
								processedChanges = 0;
								yield return null;
							}
						}
					}
					if (modified)
					{
						rend.SetPropertyBlock(propertyBlock, m);
					}
				}
				scannedRenderers++;
				if (scannedRenderers % 60 == 0)
				{
					yield return null;
				}
			}
			RawImage[] array3 = Object.FindObjectsOfType<RawImage>();
			foreach (RawImage val2 in array3)
			{
				if (!IsValidRawImage(val2) || IsInIgnoredPath(((Component)val2).gameObject))
				{
					continue;
				}
				int instanceID2 = ((Object)val2).GetInstanceID();
				if (!processedRawImages.Add(instanceID2))
				{
					continue;
				}
				Texture texture2 = val2.texture;
				Texture2D val3 = (Texture2D)(object)((texture2 is Texture2D) ? texture2 : null);
				if (!((Object)(object)val3 == (Object)null))
				{
					if (TryGetReplacement(((Object)val3).name, out var replacement2))
					{
						val2.texture = (Texture)(object)replacement2;
						processedChanges++;
					}
					scannedRawImages++;
					if (processedChanges >= 8 || scannedRawImages % 60 == 0)
					{
						processedChanges = 0;
						yield return null;
					}
				}
			}
		}

		private static bool TryGetReplacement(string textureName, out Texture2D replacement)
		{
			if (!currentReplacements.TryGetValue(textureName, out replacement))
			{
				return currentReplacements.TryGetValue(textureName.ToLower(), out replacement);
			}
			return true;
		}

		private static bool IsValidRenderer(Renderer rend, Camera mainCam)
		{
			if ((Object)(object)rend == (Object)null || (Object)(object)((Component)rend).gameObject == (Object)null || !((Component)rend).gameObject.activeInHierarchy)
			{
				return false;
			}
			if (((Object)(object)mainCam) != null && ((Object)(object)((Component)rend).gameObject == (Object)(object)((Component)mainCam).gameObject || (Object)(object)((Component)rend).GetComponentInParent<Camera>(true) != (Object)null))
			{
				return false;
			}
			return true;
		}

		private static bool IsValidRawImage(RawImage raw)
		{
			if ((Object)(object)raw != (Object)null)
			{
				return ((Component)raw).gameObject.activeInHierarchy;
			}
			return false;
		}

		private static IEnumerator UpdateStyleHUD()
		{
			StyleHUD instance = MonoSingleton<StyleHUD>.Instance;
			if ((Object)(object)instance == (Object)null || rankSprites.Count == 0)
			{
				yield break;
			}
			int num = Math.Min(instance.ranks.Count, 8);
			for (int i = 0; i < num; i++)
			{
				string rankNameByIndex = GetRankNameByIndex(i);
				if (rankSprites.TryGetValue(rankNameByIndex, out var value))
				{
					instance.ranks[i].sprite = value;
				}
				else
				{
					Logging.Warn("[TexturePatcher] Missing sprite for rank: " + rankNameByIndex);
				}
			}
			string rankNameByIndex2 = GetRankNameByIndex(instance.rankIndex);
			if (rankSprites.TryGetValue(rankNameByIndex2, out var value2))
			{
				instance.rankImage.sprite = value2;
			}
			else
			{
				Logging.Warn("[TexturePatcher] Missing sprite for current rank: " + rankNameByIndex2);
			}
			yield return null;
		}

		private static string GetRankNameByIndex(int i)
		{
			if (i >= 0 && i < RankNames.Length)
			{
				return RankNames[i];
			}
			return "RankD";
		}

		private static IEnumerator LoadTexture(string filename, Action<Texture2D> callback)
		{
			if (cancellationTokenSource.IsCancellationRequested)
			{
				callback(null);
				yield break;
			}
			if (textureCache.TryGetValue(filename, out var value))
			{
				callback(value);
				yield break;
			}
			string fullPath = FindTextureFile(filename);
			if (string.IsNullOrEmpty(fullPath))
			{
				Logging.Warn("[TexturePatcher] File not found: " + filename);
				callback(null);
				yield break;
			}
			byte[] fileData = null;
			Exception error = null;
			bool isDone = false;
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					if (!cancellationTokenSource.IsCancellationRequested)
					{
						fileData = File.ReadAllBytes(fullPath);
					}
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
			{
				yield return null;
			}
			if (cancellationTokenSource.IsCancellationRequested)
			{
				callback(null);
				yield break;
			}
			if (error != null)
			{
				Logging.Warn("[TexturePatcher] Error loading '" + filename + "': " + error.Message);
				callback(null);
				yield break;
			}
			if (fileData == null || fileData.Length == 0)
			{
				Logging.Warn("[TexturePatcher] Empty or unreadable file: " + filename);
				callback(null);
				yield break;
			}
			Texture2D val = new Texture2D(2, 2, (TextureFormat)4, false)
			{
				name = Path.GetFileNameWithoutExtension(fullPath),
				filterMode = (FilterMode)0,
				anisoLevel = 0
			};
			if (!ImageConversion.LoadImage(val, fileData))
			{
				Logging.Warn("[TexturePatcher] Failed to decode image: " + filename);
				callback(null);
			}
			else
			{
				textureCache[filename] = val;
				callback(val);
			}
		}

		private static string FindTextureFile(string filename)
		{
			string[] array = new string[4] { ".png", ".jpg", ".jpeg", ".tga" };
			foreach (string text in array)
			{
				string text2 = Path.Combine(texturesFolder, filename + text);
				if (File.Exists(text2))
				{
					return text2;
				}
			}
			string text3 = Path.Combine(texturesFolder, filename);
			if (!File.Exists(text3))
			{
				return null;
			}
			return text3;
		}

		private static void ReplaceUISprites()
		{
			Image[] array = Object.FindObjectsOfType<Image>(true);
			int num = 0;
			Image[] array2 = array;
			foreach (Image val in array2)
			{
				if (!((Object)(object)val == (Object)null) && !((Object)(object)val.sprite == (Object)null))
				{
					string name = ((Object)val.sprite).name;
					if (rankSprites.TryGetValue(name, out var value))
					{
						val.sprite = value;
						num++;
					}
				}
			}
			if (num > 0)
			{
				Logging.Message($"[TexturePatcher] Replaced {num} UI sprites");
			}
		}

		private static string GetHierarchyPath(Transform transform)
		{
			List<string> list = new List<string>();
			while ((Object)(object)transform != (Object)null)
			{
				list.Insert(0, ((Object)transform).name);
				transform = transform.parent;
			}
			return string.Join("/", list);
		}

		private static bool IsInIgnoredPath(GameObject obj)
		{
			string hierarchyPath = GetHierarchyPath(obj.transform);
			foreach (string ignoredPathPattern in IgnoredPathPatterns)
			{
				if (hierarchyPath.Contains(ignoredPathPattern))
				{
					return true;
				}
			}
			return false;
		}

		private static bool ShouldIgnoreScene(string sceneName)
		{
			return ignoredScenes.Any((string i) => sceneName.Equals(i, StringComparison.OrdinalIgnoreCase));
		}
	}
}
