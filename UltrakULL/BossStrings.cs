using UltrakULL.json;
using static UltrakULL.CommonFunctions;

namespace UltrakULL
{
    public static class BossStrings
    {
        public static string GetBossName(string originalBossName)
        {
            Logging.Warn(originalBossName);

            //Alter RADIANT names changer. But i go add new boss strings to the json file and EnemyBios.cs

            //if (originalBossName.Contains("RADIANT"))
            //{
            //    return (LanguageManager.CurrentLanguage.enemyNames.enemyname_boss_swordsmachineAgony + " " + EnemyBios.GetName(originalBossName.ToUpper())); ;
            //}

            return EnemyBios.GetName(originalBossName);
        }
    }
}
