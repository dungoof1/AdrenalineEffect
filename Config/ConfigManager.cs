

using System.Runtime.CompilerServices;
using BepInEx.Configuration;

namespace AdrenalineEffect.Config
{
    internal class ConfigManager
    {
        public static ConfigEntry<float> Cooldown;
        public static ConfigEntry<float> Duration;
        public static ConfigEntry<bool> Downsides;

        public static void RegisterConfig(ConfigFile config)
        {
            Cooldown = config.Bind(
                "General Settings",
                "Cooldown",
                30f);

            Duration = config.Bind(
                "General Settings",
                "Duration",
                10f);

            Downsides = config.Bind(
                "General Settings",
                "Downsides",
                true,
                new ConfigDescription(
                    "Enable tunnelvision after painkiller"));
        }
    }
}
