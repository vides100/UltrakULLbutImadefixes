using BepInEx;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UltrakULL.json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
using static UltrakULL.CommonFunctions;
using static UnityEngine.UI.Image;

namespace UltrakULL.Harmony_Patches
{
    [HarmonyPatch(typeof(IntermissionController))]
    public static class LocalizeIntermission
    {
        private static int currentIndex = 1;
        private static bool firstPlayed = false;
        private static bool previousWaitingForInput = false;
        private static int lastPlayedIndex = 0;
        private static bool isPlaying = false;
        private static AudioSource currentAudioSource = null;
        private static Coroutine currentCoroutine = null;

        private const float FadeInDelay = 0.3f;
        private static float timeSinceTransition = 0f;
        private static bool transitioned = false;

        [HarmonyPrefix]
        [HarmonyPatch("Start")]
        public static bool Start_MyPatch(IntermissionController __instance, ref string ___preText, ref string ___fullString, ref Text ___txt)
        {
            ResetAudio();
            CoroutineHelper.Init();

            /*
            In the original, there is no ▼ (wait input) between act2_intermission_fourth8 and act2_intermission_fourth9.
            Therefore, I disable the update stop for English(to ensure that the behavior is the same between English and other languages.
            If this option is not suitable, remove the ▼ between act2_intermission_fourth8 and act2_intermission_fourth9)
            */

            //if (isUsingEnglish()) return true; 

            ___txt = __instance.GetComponent<Text>();
            ___txt.verticalOverflow = VerticalWrapMode.Overflow;
            ___fullString = ___txt.text;
            ___txt.text = "";

            IntermissionStrings intStrings = new IntermissionStrings();
            ___fullString = intStrings.GetIntermissionString(___fullString);
            ___txt.text = ___fullString;

            if (GetCurrentSceneName() == "Level 2-S")
            {
                string openingTag = "<color=grey>";
                string closingTag = "</color>";
                string mirageName = Regex.Replace(___preText, @"<[^>]*>", "");

                switch (mirageName)
                {
                    case "JUST SOMEONE:":
                        ___preText = $"{openingTag}{LanguageManager.CurrentLanguage.visualnovel.visualnovel_mirageName1}{closingTag}:";
                        break;
                    case "THE PRETTIEST GIRL IN TOWN:":
                        ___preText = $"{openingTag}{LanguageManager.CurrentLanguage.visualnovel.visualnovel_mirageName2}{closingTag}:";
                        break;
                    case "MIRAGE:":
                        ___preText = $"{openingTag}{LanguageManager.CurrentLanguage.visualnovel.visualnovel_mirageName3}{closingTag}:";
                        break;
                }
            }

            return true;
        }


        private static bool wasPausedLastFrame = false;

        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        public static void Update_MyPatch(IntermissionController __instance, ref bool ___waitingForInput, ref bool ___skipToInput)
        {
            if (!__instance.gameObject.activeInHierarchy)
                return;

            bool isPaused = MonoSingleton<OptionsManager>.Instance != null && MonoSingleton<OptionsManager>.Instance.paused;

            // ▶️ Переход из паузы в норму → UnPause
            if (wasPausedLastFrame && !isPaused)
            {
                if (currentAudioSource != null && currentAudioSource.clip != null && currentAudioSource.isPlaying == false)
                {
                    currentAudioSource.UnPause();
                }
            }
            // ⏸ Вход в паузу → Pause
            else if (!wasPausedLastFrame && isPaused)
            {
                if (currentAudioSource != null && currentAudioSource.isPlaying)
                {
                    currentAudioSource.Pause();
                }
            }

            wasPausedLastFrame = isPaused;

            // ❌ Не продолжаем обработку, если игра на паузе
            if (isPaused)
                return;

            bool waitingForInput = ___waitingForInput;
            bool skipToInput = ___skipToInput;

            if (!firstPlayed)
            {
                PlayAudioForIndex(currentIndex, GetSanitizedHierarchyPath(__instance.transform, 2));
                lastPlayedIndex = currentIndex;
                firstPlayed = true;
                previousWaitingForInput = waitingForInput;
                return;
            }

            if (!waitingForInput && skipToInput && isPlaying)
            {
                StopCurrentAudio();
                return;
            }

            if (previousWaitingForInput && !waitingForInput && !skipToInput)
            {
                StopCurrentAudio();
                currentIndex++;

                if (currentIndex != lastPlayedIndex)
                {
                    PlayAudioForIndex(currentIndex, GetSanitizedHierarchyPath(__instance.transform, 2));
                    lastPlayedIndex = currentIndex;
                }
            }

            previousWaitingForInput = waitingForInput;
        }


        private static IEnumerator WaitAndPlay(int index, string path, float delay)
        {
            yield return new WaitForSeconds(delay);
            PlayAudioForIndex(index, path);
        }

        private static string GetSanitizedHierarchyPath(Transform transform, int depth)
        {
            List<string> names = new List<string>();
            Transform current = transform;

            for (int i = 0; i < depth && current.parent != null; i++)
            {
                current = current.parent;
                names.Insert(0, SanitizeFileName(current.name));
            }

            names.Add(SanitizeFileName(transform.gameObject.name));

            return Path.Combine(names.ToArray());
        }

        private static void StopCurrentAudio()
        {
            if (currentAudioSource != null)
            {
                currentAudioSource.Stop();
                UnityEngine.Object.Destroy(currentAudioSource.gameObject);
                currentAudioSource = null;
                isPlaying = false;
                CoroutineHelper.StopCurrentCoroutine();
                Logging.Info("[LocalizeIntermission] Озвучка остановлена (скип).");
            }
        }

        private static void PlayAudioForIndex(int index, string gameObjectName)
        {
            isPlaying = true;
            string audioFileName = $"line{index}.ogg";
            string folderPath = Path.Combine(
                Paths.ConfigPath,
                "ultrakull",
                "audio",
                LanguageManager.CurrentLanguage.metadata.langName,
                GetCurrentSceneName(),
                gameObjectName
            );

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string audioPath = Path.Combine(folderPath, audioFileName);
            string audioUrl = "file:///" + audioPath.Replace("\\", "/");

            Logging.Info($"[LocalizeIntermission] Попытка воспроизвести: {audioFileName}");
            CoroutineHelper.StartSafeCoroutine(PlayAudioCoroutine(audioUrl, audioFileName, index));
        }

        private static string SanitizeFileName(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = new System.Text.StringBuilder();

            foreach (char c in name)
            {
                if (!char.IsWhiteSpace(c) && !invalidChars.Contains(c))
                {
                    sanitized.Append(c);
                }
            }

            return sanitized.ToString();
        }

        private static IEnumerator PlayAudioCoroutine(string audioUrl, string fileName, int expectedIndex)
        {
            isPlaying = true;

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioUrl, AudioType.OGGVORBIS))
            {
                yield return www.SendWebRequest();

                if (currentIndex != expectedIndex)
                {
                    Logging.Info($"[LocalizeIntermission] Индекс изменился (ожидался {expectedIndex}, сейчас {currentIndex}), отмена воспроизведения.");
                    yield break;
                }

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Logging.Warn($"[LocalizeIntermission] Ошибка загрузки аудиофайла '{fileName}': {www.error}");
                    Logging.Warn($"[LocalizeIntermission] Путь: '{audioUrl}'");
                    isPlaying = false;
                    yield break;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip == null || clip.length <= 0f)
                {
                    Logging.Warn($"[LocalizeIntermission] Файл '{fileName}' повреждён или пуст.");
                    isPlaying = false;
                    yield break;
                }

                GameObject audioObject = new GameObject($"Audio_{fileName}");
                AudioSource source = audioObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.playOnAwake = false;

                currentAudioSource = source;
                UnityEngine.Object.DontDestroyOnLoad(audioObject);

                Logging.Info($"[LocalizeIntermission] Воспроизведение: {fileName}");
                source.Play();

                float timer = 0f;
                while (timer < clip.length)
                {
                    if (currentAudioSource != source || source == null) break;
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (currentAudioSource == source && source != null)
                {
                    UnityEngine.Object.Destroy(source.gameObject);
                    currentAudioSource = null;
                }

                isPlaying = false;
            }
        }

        public static void ResetAudio()
        {
            currentIndex = 1;
            firstPlayed = false;
            previousWaitingForInput = false;
            isPlaying = false;
            transitioned = false;
            timeSinceTransition = 0f;

            StopCurrentAudio();
            Logging.Info("[LocalizeIntermission] Сброс состояния озвучки при запуске сцены.");
        }

        private class CoroutineHelper : MonoBehaviour
        {
            private static CoroutineHelper instance;
            private static Coroutine currentCoroutine;

            public static void Init()
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("CoroutineHelper");
                    UnityEngine.Object.DontDestroyOnLoad(go);
                    instance = go.AddComponent<CoroutineHelper>();
                }
            }

            public static void StartSafeCoroutine(IEnumerator coroutine)
            {
                Init();
                if (currentCoroutine != null)
                {
                    instance.StopCoroutine(currentCoroutine);
                    currentCoroutine = null;
                }
                currentCoroutine = instance.StartCoroutine(coroutine);
            }

            public static void StopCurrentCoroutine()
            {
                if (currentCoroutine != null)
                {
                    instance.StopCoroutine(currentCoroutine);
                    currentCoroutine = null;
                }
            }
        }
    }
}
