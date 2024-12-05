using BepInEx.Logging;
using BepInEx;
using AdrenalineEffect.Patches;
using AdrenalineEffect.Config;

namespace AdrenalineEffect
{
    [BepInPlugin("dev.dungoof.AdrenalineEffect", "AdrenalineEffect", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public void Awake()
        {
            Plugin.LogSource = base.Logger;
            Plugin.LogSource.LogInfo("AdrenalineEffect enabled.");
            ConfigManager.RegisterConfig(Config);
            new ApplyAdrenalinePatch().Enable();
            
        }
        public static ManualLogSource LogSource;
    }
}
