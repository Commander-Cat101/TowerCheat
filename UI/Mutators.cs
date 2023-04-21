using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppSystem.IO;

namespace TowerCheat.UI
{
    [HarmonyPatch(typeof(RangeSupport.MutatorTower), nameof(RangeSupport.MutatorTower.Mutate))]
    internal static class RangeSupport_MutatorTower_Mutate
    {
        [HarmonyPrefix]
        private static bool Prefix(RangeSupport.MutatorTower __instance, Model model)
        {
            if (__instance.id == MIB.GetMutator().id)
            {
                foreach(var damage in model.GetDescendants<DamageModel>().ToArray())
                {
                    damage.immuneBloonProperties = 0;
                }
            }
            return true;
        }
    }
    public static class MIB
    {
        public static BehaviorMutator GetMutator()
        {
            BehaviorMutator mutator = new RangeSupport.MutatorTower(true, "VisionMutator", 0, 0, null);
            return mutator;
        }
    }
}
