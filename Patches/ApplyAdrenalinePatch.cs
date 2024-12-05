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
        static float cooldownTime = 0f;
        static float cooldown = ConfigManager.Cooldown.Value;
        static float duration = ConfigManager.Duration.Value;
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

            // Check if damage recieved should apply adrenaline
            if (acceptedDamageTypes.Contains(type)) 
            {
                // Check not on cooldown
                if (Time.time >= cooldownTime)
                {
                    // Check no painkiller effect on head
                    if (RE.CheckEffectMethod.Invoke(healthController, new object[] { EBodyPart.Head }) == null)
                    {
                        // Reset cooldown
                        cooldownTime = Time.time + duration + cooldown;

                        // Apply Painkiller and TunnelVision
                        RE.PainKillerMethod.Invoke(healthController, new object[] { EBodyPart.Head, 0f, duration, 2f, 1f, null });
                        if (ConfigManager.Downsides.Value)
                        {
                            RE.TunnelVisionMethod.Invoke(healthController, new object[] { EBodyPart.Head, duration, duration / 2f, 2f, 1f, null });
                        }
                        
                    }
                }

            }
        }
 
    }
}
