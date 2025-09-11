using System;
using System.IO;
using BepInEx;
using UltrakULL.json;
using UnityEngine;
using UnityEngine.Networking;

using static UltrakULL.CommonFunctions;

namespace UltrakULL.audio
{
    public static class AudioSwapper
    {
        public static string SpeechFolder = Path.Combine(Paths.ConfigPath, "ultrakull", "audio", LanguageManager.CurrentLanguage.metadata
        .langName) + Path.DirectorySeparatorChar;
        
        public static AudioClip SwapClipWithFile(AudioClip sourceClip, string audioFilePath)
        {
            if(isUsingEnglish())
            {
                return sourceClip;
            }
            
            string file = "file://" + audioFilePath + ".ogg";
            Logging.Message("Swapping: " + file);

            UnityWebRequest fileRequest = null;
            try
            {
                fileRequest = UnityWebRequestMultimedia.GetAudioClip(file, AudioType.OGGVORBIS);
                fileRequest.SendWebRequest();

                // Using a safer waiting method
                while (!fileRequest.isDone) 
                {
                    // Add a small delay to prevent blocking
                    if (Time.frameCount % 30 == 0) // Checking every 30 frames
                    {
                        System.Threading.Thread.Sleep(1);
                    }
                }
 
                if (fileRequest.result != UnityWebRequest.Result.Success)
                {
                    Logging.Warn(fileRequest.error + "\n Expected path: " + audioFilePath + ".ogg");
                }
                else
                {
                    var newClip = DownloadHandlerAudioClip.GetContent(fileRequest);
                    if (newClip != null)
                    {
                        sourceClip = newClip;
                    }
                }
            }
            catch (Exception err)
            {
                Logging.Warn("Failed to swap " + audioFilePath);
                Logging.Warn($"{err.Message}, {err.StackTrace}");
            }
            finally
            {
                // Releasing resources correctly
                if (fileRequest != null)
                {
                    fileRequest.Dispose();
                }
            }
            
            return sourceClip;
        }
    }
}
