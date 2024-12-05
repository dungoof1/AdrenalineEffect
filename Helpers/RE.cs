using System;
using System.Reflection;
using EFT.HealthSystem;
using HarmonyLib;

namespace AdrenalineEffect.Helpers
{
    public static class RE
    {
        public static Type PainKillerType;
        public static Type TunnelVisionType;
        public static MethodInfo PainKillerMethod;
        public static MethodInfo TunnelVisionMethod;
        public static MethodInfo CheckEffectMethod;

        static RE()
        {
            PainKillerType = AccessTools.Inner(typeof(ActiveHealthController), name: "PainKiller");
            TunnelVisionType = AccessTools.Inner(typeof(ActiveHealthController), name: "TunnelVision");

            PainKillerMethod = AccessTools
                .Method(
                    typeof(ActiveHealthController),
                    nameof(ActiveHealthController.AddEffect),
                    generics: [PainKillerType]);

            TunnelVisionMethod = AccessTools
                .Method(
                    typeof(ActiveHealthController),
                    nameof(ActiveHealthController.AddEffect),
                    generics: [TunnelVisionType]);

            CheckEffectMethod = AccessTools
                .Method(
                typeof(ActiveHealthController),
                nameof(ActiveHealthController.FindExistingEffect),
                generics: [PainKillerType]);
        }
    }
}
