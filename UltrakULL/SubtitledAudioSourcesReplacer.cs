using System.Collections.Generic;
using System.Threading.Tasks;
using BepInEx;
using UltrakULL.json;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UltrakULL.ReflectionUtils;
using static UltrakULL.CommonFunctions;
using static UltrakULL.audio.AudioSwapper;
using static System.IO.Path;

namespace UltrakULL
{
    public static class SubtitledAudioSourcesReplacer
    {
        public static string SpeechFolder = Combine(Paths.ConfigPath,"ultrakull", "audio", LanguageManager.CurrentLanguage.metadata.langName);
        
        public static SubtitledSourcesConfig Config;

        public static async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            await Task.Delay(250);
            ReplaceSubsAndAudio();
        }

        public static void ReplaceSubsAndAudio()
        {
            if (!TryLoadMetadata(out var objectReferences)) 
                return;

            foreach (var objectReference in objectReferences)
            {
                foreach (var gameObject in objectReference.Objects)
                {
                    var obj = GetObject(gameObject);
                    if (obj == null)
                    {
                        Logging.Warn($"[UAK] GetObject('{gameObject}') return NULL");
                        continue;
                    }

                    var subtitledAudioSource = obj.GetComponent<SubtitledAudioSource>();
                    var audioSource = obj.GetComponentInChildren<AudioSource>();

                    if (ActiveDubbingEnabled())
                    {
                        if (audioSource != null)
                        {
                            var src = audioSource;
                            SwapClipWithFileAsync(src.clip, Combine(SpeechFolder, objectReference.AudioPath), (newClip) => { try { src.clip = newClip; } catch { } });
                        }
                        else
                        {
                            Logging.Warn($"[UAK] AudioSource not founded in '{obj.name}'");
                        }
                    }

                    if (subtitledAudioSource != null)
                    {
                        SetPrivate(subtitledAudioSource, typeof(SubtitledAudioSource), "subtitles", objectReference.ToSubtitleData());
                    }
                    else
                    {
                        Logging.Warn($"[UAK] SubtitledAudioSource not founded in '{obj.name}'");
                    }
                }
            }
        }

        private static bool ActiveDubbingEnabled()
        {
            return LanguageManager.configFile.Bind("General", "activeDubbing", "False").Value != "False";
        }

        private static bool TryLoadMetadata(out List<SubtitledObjectReference> references)
        {
            if (Config != null && Config.Scenes.TryGetValue(GetCurrentSceneName(), out references))
                return true;

            references = default;
            return false;
        }
    }
}