using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Harmony;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerCheat.UI;
using UnityEngine;
using UnityEngine.UIElements;
using static MelonLoader.MelonLogger;
using HarmonyPatch = HarmonyLib.HarmonyPatch;
using TaskScheduler = BTD_Mod_Helper.Api.TaskScheduler;

namespace TowerCheat.TowerMethod
{
    public static class AttackMethods
    {
        public static void ChangeRange(ModHelperText text, string name, float rangechange)
        {
            if (IsShiftPressed()) { rangechange *= 5; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).range += rangechange;
            InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
            string range;
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).range > 999) { range = "Inf"; } else { range = model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).range.ToString(); }
            text.SetText("Range: " + range);
        }
        public static void ChangeRate(ModHelperText text, string name, float ratechange)
        {
            if (IsShiftPressed()) { ratechange *= 5; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).weapons[0].rate += ratechange;
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).weapons[0].rate > 0)
            {
                InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
                text.SetText("Rate: " + Math.Round(model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).weapons[0].rate, 2));
                
            }
        }
        public static void ChangePierce(ModHelperText text, string name, float piercechange)
        {
            if (IsShiftPressed()) { piercechange *= 5; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).weapons[0].projectile.pierce += piercechange;
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).weapons[0].projectile.pierce > 0)
            {
                InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
                text.SetText("Pierce: " + Math.Round(model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).weapons[0].projectile.pierce, 2));
            }
        }
        public static void ChangeDamage(ModHelperText text, string name, float damagechange)
        {
            if (IsShiftPressed()) { damagechange *= 5; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).weapons[0].projectile.GetDamageModel().damage += damagechange;
            InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
            text.SetText("Damage: " + Math.Round(model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).weapons[0].projectile.GetDamageModel().damage, 2));
        }
        

        public static bool IsShiftPressed()
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
    }
    public static class WindMethods
    {

        public static void AddWind(ModHelperButton button, string name)
        {
            MelonLogger.Msg("AddWind");
            button.Destroy();
            var Wind = new WindModel("WindModelAddedByTowerCheat", 50, 50, .5f, false, "");
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == name).weapons[0].projectile.AddBehavior(Wind);
            InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);

            TowerCheat.LoadScrollContent();
        }

        public static void ChangeWindChance(ModHelperText text, string attackname, float change)
        {
            if (Input.GetKey(KeyCode.LeftShift)) { change *= 5; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().chance += change;
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().chance > 0 && model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().chance < 1.01)
            {
                InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
                text.SetText("Chance: " + (int)(model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().chance * 100) + "%");
            }
        }
        public static void ChangeWindMin(ModHelperText text, string attackname, float change)
        {
            if (Input.GetKey(KeyCode.LeftShift)) { change *= 5; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().distanceMin += change;
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().distanceMin > 0)
            {
                InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
                text.SetText("Min Blow Distance: " + model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().distanceMin);
            }
        }
        public static void ChangeWindMax(ModHelperText text, string attackname, float change)
        {
            if (Input.GetKey(KeyCode.LeftShift)) { change *= 5; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().distanceMax += change;
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().distanceMax > 0)
            {
                InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
                text.SetText("Max Blow Distance: " + model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().distanceMax);
            }
        }

        public static void ChangeWindMoab(ModHelperButton button, string attackname)
        {
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().affectMoab)
            {
                model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().affectMoab = false;
                button.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOffRed"));
            }
            else
            {
                model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<WindModel>().affectMoab = true;
                button.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOnGreen"));
            }
            InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
        }
    }
    public static class MultishotMethods
    {

        public static void AddMultishot(ModHelperButton button, string name)
        {
            var model = InGame.instance.inputManager.SelectedTower.tower.towerModel.Duplicate();

            var emission = Game.instance.model.GetTower(TowerType.TackShooter).GetAttackModel().weapons[0].emission.Duplicate();
            model.GetAttackModels().Find(a => a.name == name).weapons[0].emission = emission;
            InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
            TowerCheat.LoadScrollContent();
        }

        public static void ChangeMultishotProj(ModHelperText text, string attackname, int change)
        {
            if (Input.GetKey(KeyCode.LeftShift)) { change *= 3; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].emission.TryCast<ArcEmissionModel>().count += change;
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].emission.TryCast<ArcEmissionModel>().count > 0)
            {
                InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
                text.SetText("Projectiles: " + model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].emission.TryCast<ArcEmissionModel>().count);
            }
        }
        public static void ChangeMultishotAimCone(ModHelperText text, string attackname, int change)
        {
            if (Input.GetKey(KeyCode.LeftShift)) { change *= 10; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].emission.TryCast<ArcEmissionModel>().angle += change;
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].emission.TryCast<ArcEmissionModel>().angle > -1 && model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].emission.TryCast<ArcEmissionModel>().angle < 361)
            {
                InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
                text.SetText("Aim Cone: " + model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].emission.TryCast<ArcEmissionModel>().angle);
            }
        }
    }
    public static class TowerMethods
    {
        public static void ToggleCamo(Tower tower, ModHelperButton CamoButton)
        {
            var model = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (model.HasBehavior<OverrideCamoDetectionModel>())
            {
                if (model.GetBehavior<OverrideCamoDetectionModel>().detectCamo)
                {
                    model.GetBehavior<OverrideCamoDetectionModel>().detectCamo = false;
                    CamoButton.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOffRed"));
                }
                else
                {
                    model.GetBehavior<OverrideCamoDetectionModel>().detectCamo = true;
                    CamoButton.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOnGreen"));
                }  
            }
            else
            {
                model.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoTowerCheat", true));
                CamoButton.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOnGreen"));
            }
            tower.UpdateRootModel(model);
        }
        public static void ToggleMIB(Tower tower, ModHelperButton MIBButton)
        {
            var model = tower.rootModel.Duplicate().Cast<TowerModel>();
            if (InGame.instance.inputManager.SelectedTower.tower.GetMutatorById(MIB.GetMutator().id) == null)
            {
                InGame.instance.inputManager.SelectedTower.tower.AddMutator(MIB.GetMutator());
                MIBButton.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOnGreen"));
            }
            else
            {
                InGame.instance.inputManager.SelectedTower.tower.RemoveMutatorsById(MIB.GetMutator().id);
                MIBButton.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOffRed"));
            }
        }
    }
    public static class AbilityMethods
    {
        public static void ChangeAbilityCooldown(ModHelperText text,string abilityname, float change)
        {
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            var ability = model.GetAbilities().First(a => a.name == abilityname);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                change *= 5;
            }
            ability.cooldown += change;
            if (ability.cooldown > 0)
            {
                InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
                text.SetText("Cooldown: " + ability.cooldown);
            }
        }
    }
    public static class SplitshotMethods
    {
        public static void AddSplitshot(ModHelperButton button, string name)
        {
            var model = InGame.instance.inputManager.SelectedTower.tower.towerModel.Duplicate();
            var sub = Game.instance.model.GetTower(TowerType.MonkeySub, 0, 0, 2).GetDescendant<CreateProjectileOnExhaustFractionModel>().Duplicate();
            sub.emission.Cast<ArcEmissionModel>().angle = 40;
            var splitshot = new CreateProjectileOnContactModel("SplitshotByTowerCheat", sub.projectile, sub.emission, false, false, false);


            model.GetAttackModels().Find(a => a.name == name).weapons[0].projectile.AddBehavior(splitshot);
            InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
            TowerCheat.LoadScrollContent();
        }
        public static void ChangeSplitshotProj(ModHelperText text, string attackname, int change)
        {
            if (Input.GetKey(KeyCode.LeftShift)) { change *= 5; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().emission.TryCast<ArcEmissionModel>().count += change;
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().emission.TryCast<ArcEmissionModel>().count > 0)
            {
                InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
                text.SetText("Projectiles: " + model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().emission.TryCast<ArcEmissionModel>().count);
            }
        }
        public static void ChangeSplitshotDamage(ModHelperText text, string attackname, int change)
        {
            if (Input.GetKey(KeyCode.LeftShift)) { change *= 5; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += change;
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage > 0)
            {
                InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
                text.SetText("Damage: " + model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage);
            }
        }
        public static void ChangeSplitshotAimCone(ModHelperText text, string attackname, int change)
        {
            if (Input.GetKey(KeyCode.LeftShift)) { change *= 10; }
            var model = InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().Duplicate();
            model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().emission.TryCast<ArcEmissionModel>().angle += change;
            if (model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().emission.TryCast<ArcEmissionModel>().angle > -1 && model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().emission.TryCast<ArcEmissionModel>().angle < 361)
            {
                InGame.instance.inputManager.SelectedTower.tower.UpdateRootModel(model);
                text.SetText("Aim Cone: " + model.GetDescendants<AttackModel>().FirstOrDefault(a => a.name == attackname).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().emission.TryCast<ArcEmissionModel>().angle);
            }
        }
    }
}
