using System.Collections.Generic;
using System.Reflection;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using UnityEngine;
using Comfort.Common;
using AdrenalineEffect.Helpers;
using AdrenalineEffect.Config;

namespace AdrenalineEffect.Patches
{

    internal class ApplyAdrenalinePatch : ModulePatch
    {
        // Constants
        static float cooldownTime = 0f;
        static float duration = ConfigManager.Duration.Value;
        static bool downsides = ConfigManager.Downsides.Value;
        static bool debug = ConfigManager.Debug.Value;

        // Damage type list
        static List<EDamageType> acceptedDamageTypes = new List<EDamageType>
            {
                EDamageType.Bullet,
                EDamageType.Explosion,
                EDamageType.Sniper,
                EDamageType.Landmine,
                EDamageType.GrenadeFragment,
                EDamageType.Blunt
            };


        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player), "ReceiveDamage");
        }

        [PatchPostfix]
        private static void Postfix(ref Player __instance, EDamageType type)
        {
            var healthController = Singleton<GameWorld>.Instance.MainPlayer.ActiveHealthController;

            // Check if damage is to the player
            if (__instance != Singleton<GameWorld>.Instance.MainPlayer) 
            {
                return; 
            }
            
            // Check if damage is appropiate type
            if (!acceptedDamageTypes.Contains(type)) 
            {
                return; 
            }

            if (debug)
            {
                Plugin.LogSource.LogInfo("Taken appropiate damage at" + Time.time);
            }

            // Check not on cooldown
            if(Time.time < cooldownTime)
            {
                return;
            }

            // Check no painkiller effect
            if(RE.CheckEffectMethod.Invoke(healthController, new object[] { EBodyPart.Head }) != null)
            {
                return;
            }

            // Reset cooldown
            cooldownTime = Time.time + duration + ConfigManager.Cooldown.Value;

            // Apply Painkiller and TunnelVision
            RE.PainKillerMethod.Invoke(healthController, new object[] { EBodyPart.Head, 0f, duration, 2f, 1f, null });
            if (downsides)
            {
                RE.TunnelVisionMethod.Invoke(healthController, new object[] { EBodyPart.Head, duration, duration / 2f, 2f, 1f, null });
            }
            if (debug)
            {
                Plugin.LogSource.LogInfo("Adrenaline activated at" + Time.time);
            }
        }
    }
}
