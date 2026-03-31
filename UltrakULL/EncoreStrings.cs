using UltrakULL.json;
using static UltrakULL.CommonFunctions;

namespace UltrakULL
{
    // The same code architecture, like as Act*Strings
    public static class EncoreStrings
    {
        public static string GetLevelChallenge(string currentLevel)
        {
            switch (currentLevel)
            {
                default: { return "There are no Challenges for this level."; }
            }
        }

        public static string GetLevelName()
        {
            string currentLevel = GetCurrentSceneName();

            switch (currentLevel)
            {
                case "Level 0-E": { return "0-E - " + LanguageManager.CurrentLanguage.levelNames.levelName_encorePrelude; }
                case "Level 1-E": { return "1-E - " + LanguageManager.CurrentLanguage.levelNames.levelName_encoreLimbo; }

                default: 
                    {
                        Logging.Warn("Unknown level name: " + currentLevel);
                        return currentLevel; 
                    }
            }
        }

        private static string Level0E(string message, string message2)
        {
            string fullMessage = message + message2;
            if (fullMessage.Contains("RADIANT"))
            {
                return LanguageManager.CurrentLanguage.encore.encorePrelude_aboutRadiantEnemies;
            }
            return ("Unimplemented string");
        }

        public static string GetMessage(string message, string message2, string input)
        {
            string currentLevel = GetCurrentSceneName();
            string fullMessage = message + message2;

            switch (currentLevel)
            {
                case "Level 0-E":
                    {
                        return Level0E(message, message2);
                    }
                //case "Level 1-E":
                //    {
                //        return Level1E(message, message2);
                //    }
                //case "Level 2-E":
                //    {
                //        return Level2E(message, message2, input);
                //    }
                //case "Level 3-E":
                //    {
                //        return Level3E(message, message2);
                //    }
                //case "Level 4-E":
                //    {
                //        return Level4E(message, message2);
                //    }
                //case "Level 5-E":
                //    {
                //        return Level5E(message, message2);
                //    }
                //case "Level 6-E":
                //    {
                //        return Level6E(message, message2);
                //    }
                //case "Level 7-E":
                //    {
                //        return Level7E(message, message2);
                //    }
                //case "Level 8-E":
                //    {
                //        return Level8E(message, message2);
                //    }
                //case "Level 9-E":
                //    {
                //        return Level9E(message, message2);
                //    }
                default:
                    {
                        Logging.Warn("Unknown Encore HUD-message string in " + currentLevel + ": \n" + message + message2);
                        return ("Unimplemented string");
                    }
            }
        }
    }
}
