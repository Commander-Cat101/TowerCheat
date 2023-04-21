using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using TaskScheduler = BTD_Mod_Helper.Api.TaskScheduler;
using Il2CppNinjaKiwi.Common;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using TowerCheat.TowerMethod;
using TowerCheat.UI;
using Il2CppAssets.Scripts.Simulation.Towers;
using Action = System.Action;
using Math = Il2CppSystem.Math;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;

namespace TowerCheat.UIPanels
{
    public static class Panels
    {
        public static ModHelperPanel Tower(Tower tower)
        {
            ModHelperPanel panel = ModHelperPanel.Create(new Info("TowerPanel", 0, 0, 800, 600), VanillaSprites.MainBgPanelHematite);
            panel.AddText(new Info("Title", 0, 125, 750, 300), "Tower", 70, Il2CppTMPro.TextAlignmentOptions.Top);

            panel.AddText(new Info("CamoDetection", -100, 100, 400, 200), "Camo", 130);
            ModHelperButton CamoButton = null;
            CamoButton = panel.AddButton(new Info("CamoButton", 200, 100, 150, 150), "", new Action(() => { TowerMethods.ToggleCamo(tower, CamoButton); }));
            if (InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().HasBehavior<OverrideCamoDetectionModel>())
            {
                if (InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().GetBehavior<OverrideCamoDetectionModel>().detectCamo)
                {
                    CamoButton.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOnGreen"));

                }
                else
                {
                    CamoButton.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOffRed"));
                }
            }
            else
            {
                CamoButton.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOffRed"));
            }

            panel.AddText(new Info("MIB", -100, -100, 400, 200), "M.I.B", 130);
            ModHelperButton MIBButton = null;
            MIBButton = panel.AddButton(new Info("MIBButton", 200, -100, 150, 150), "", new Action(() => { TowerMethods.ToggleMIB(tower, MIBButton); }));
            if (InGame.instance.inputManager.SelectedTower.tower.GetMutatorById(MIB.GetMutator().id) != null)
            {
                MIBButton.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOnGreen"));
            }
            else
            {
                MIBButton.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOffRed"));
            }
            return panel;
        }
        public static ModHelperPanel Weapon(AttackModel attack)
        {
            var panel = ModHelperPanel.Create(new Info("WeaponPanel", 0, 0, 800, 700), VanillaSprites.MainBgPanelHematite);
            panel.AddText(new Info("AttackName", 0, 175, 750, 300), attack.name, 50, Il2CppTMPro.TextAlignmentOptions.Top);


            var RangeText = panel.AddText(new Info("Range", 0, 150, 400, 200), "Range: " + attack.range, 70);
            panel.AddButton(new Info("TakeRange", -275, 150, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { AttackMethods.ChangeRange(RangeText, attack.name, -1); }));
            panel.AddButton(new Info("AddRange", 275, 150, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { AttackMethods.ChangeRange(RangeText, attack.name, 1); }));
            AttackMethods.ChangeRange(RangeText, attack.name, 0);

            var RateText = panel.AddText(new Info("Speed", 0, 50, 400, 200), "Rate: " + Math.Round(attack.weapons[0].rate, 2), 70);
            panel.AddButton(new Info("TakeRange", -275, 50, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { AttackMethods.ChangeRate(RateText, attack.name, -0.05f); }));
            panel.AddButton(new Info("AddRange", 275, 50, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { AttackMethods.ChangeRate(RateText, attack.name, 0.05f); }));

            var PierceText = panel.AddText(new Info("Pierce", 0, -50, 400, 200), "Pierce: " + Math.Round(attack.weapons[0].projectile.pierce, 2), 70);
            panel.AddButton(new Info("TakePierce", -275, -50, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { AttackMethods.ChangePierce(PierceText, attack.name, -1); }));
            panel.AddButton(new Info("AddPierce", 275, -50, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { AttackMethods.ChangePierce(PierceText, attack.name, 1); }));

            if ((attack.weapons[0].projectile.GetDamageModel()) != null)
            {
                var DamageText = panel.AddText(new Info("Damage", 0, -150, 400, 200), "Damage: " + attack.weapons[0].projectile.GetDamageModel().damage, 60);
                panel.AddButton(new Info("TakeDamage", -275, -150, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { AttackMethods.ChangeDamage(DamageText, attack.name, -1); }));
                panel.AddButton(new Info("AddDamge", 275, -150, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { AttackMethods.ChangeDamage(DamageText, attack.name, 1); }));
            }


            return panel;
        }
        public static ModHelperPanel Divider()
        {
            return ModHelperPanel.Create(new Info("Divider", 0, 0, 2000, 100), VanillaSprites.MainBGPanelGrey);
        }
        public static ModHelperPanel? EffectsPanel(AttackModel attack)
        {
            bool render = false;
            var panel = ModHelperPanel.Create(new Info("WeaponPanel", 0, 0, 800, 1100), VanillaSprites.MainBgPanelHematite);
            panel.AddText(new Info("AttackName", 0, 275, 750, 500), attack.name + "\nAdd Effects", 55, Il2CppTMPro.TextAlignmentOptions.Top);
            if (attack.GetDescendant<WindModel>() == null)
            {
                ModHelperButton? addwind = null;
                addwind = panel.AddButton(new Info("AddWind", 0, 200, 500, 200), VanillaSprites.YellowBtnLong, new Action(() =>
                {
                    WindMethods.AddWind(addwind, attack.name);
                }));
                addwind.AddText(new Info("AddWindText", 0, 0, 500, 200), "Blowback", 50);
                render = true;
            }
            if (!attack.weapons[0].emission.IsType<ArcEmissionModel>())
            {
                ModHelperButton? addmultishot = null;
                addmultishot = panel.AddButton(new Info("AddMultishot", 0, -50, 500, 200), VanillaSprites.YellowBtnLong, new Action(() =>
                {
                    MultishotMethods.AddMultishot(addmultishot, attack.name);
                }));
                addmultishot.AddText(new Info("AddMultishotText", 0, 0, 500, 200), "Multishot", 50);
                render = true;
            }
            if (!attack.weapons[0].projectile.HasBehavior<CreateProjectileOnContactModel>())
            {
                ModHelperButton? addsplitshot = null;
                addsplitshot = panel.AddButton(new Info("AddMultishot", 0, -300, 500, 200), VanillaSprites.YellowBtnLong, new Action(() =>
                {
                    SplitshotMethods.AddSplitshot(addsplitshot, attack.name);
                }));
                addsplitshot.AddText(new Info("AddMultishotText", 0, 0, 500, 200), "Splitshot", 50);
                render = true;
            }
            if (render == true)
            {
                return panel;
            }
            else
            {
                return null;
            }
        }
        public static ModHelperPanel WindPanel(AttackModel attack)
        {
            WindModel wind = attack.weapons[0].projectile.GetBehavior<WindModel>();

            var panel = ModHelperPanel.Create(new Info("WeaponPanel", 0, 0, 800, 1000), VanillaSprites.MainBgPanelHematite);
            panel.AddText(new Info("AttackName", 0, 225, 750, 500), attack.name + "\nWind Model", 60, Il2CppTMPro.TextAlignmentOptions.Top);

            var ChanceText = panel.AddText(new Info("ChanceText", 0, 200, 450, 300), "Chance: " + (int)(wind.chance * 100) + "%", 55);
            panel.AddButton(new Info("AddChance", 250, 200, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { WindMethods.ChangeWindChance(ChanceText, attack.name, 0.01f); }));
            panel.AddButton(new Info("RemoveChance", -250, 200, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { WindMethods.ChangeWindChance(ChanceText, attack.name, -0.01f); }));

            var MinText = panel.AddText(new Info("MinDistance", 0, 50, 500, 300), "Min Blow Distance: " + wind.distanceMin, 55);
            panel.AddButton(new Info("AddMin", 300, 50, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { WindMethods.ChangeWindMin(MinText, attack.name, 5); }));
            panel.AddButton(new Info("RemoveMin", -300, 50, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { WindMethods.ChangeWindMin(MinText, attack.name, -5); }));

            var MaxText = panel.AddText(new Info("MaxDistance", 0, -100, 500, 300), "Min Blow Distance: " + wind.distanceMax, 55);
            panel.AddButton(new Info("AddMax", 300, -100, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { WindMethods.ChangeWindMax(MaxText, attack.name, 5); }));
            panel.AddButton(new Info("RemoveMax", -300, -100, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { WindMethods.ChangeWindMax(MaxText, attack.name, -5); }));

            ModHelperButton Toggle = null;
            Toggle = panel.AddButton(new Info("ToggleMoab", 200, -250, 150, 150), "", new Action(() => { WindMethods.ChangeWindMoab(Toggle, attack.name); }));
            panel.AddText(new Info("ToggleMoabText", -100, -250, 400, 200), "Affect Moabs", 55);
            Toggle.Image.SetSprite(ModContent.GetSprite<TowerCheat>("MkOffRed"));

            return panel;
        }
        public static ModHelperPanel MultishotPanel(AttackModel attack)
        {
            ArcEmissionModel arc = attack.weapons[0].emission.TryCast<ArcEmissionModel>();

            var panel = ModHelperPanel.Create(new Info("WeaponPanel", 0, 0, 800, 800), VanillaSprites.MainBgPanelHematite);
            panel.AddText(new Info("AttackName", 0, 225, 750, 300), attack.name + "\nMultishot Model", 60, Il2CppTMPro.TextAlignmentOptions.Top);

            var projectiles = panel.AddText(new Info("ProjectilesText", 0, 100, 450, 300), "Projectiles: " + arc.count, 55);
            panel.AddButton(new Info("AddProjectile", 250, 100, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { MultishotMethods.ChangeMultishotProj(projectiles, attack.name, 1); }));
            panel.AddButton(new Info("RemoveProjectiles", -250, 100, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { MultishotMethods.ChangeMultishotProj(projectiles, attack.name, -1); }));

            var rangeangle = panel.AddText(new Info("AimConeText", 0, -100, 450, 300), "Aim Cone: " + arc.angle, 55);
            panel.AddButton(new Info("AddProjectile", 250, -100, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { MultishotMethods.ChangeMultishotAimCone(rangeangle, attack.name, 5); }));
            panel.AddButton(new Info("RemoveProjectiles", -250, -100, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { MultishotMethods.ChangeMultishotAimCone(rangeangle, attack.name, -5); }));

            return panel;
        }
        public static ModHelperPanel AbilityPanel(AbilityModel ability)
        {
            var panel = ModHelperPanel.Create(new Info("AbilityPanel", 0, 0, 800, 600), VanillaSprites.MainBgPanelHematite);
            panel.AddText(new Info("AbilityName", 0, 125, 750, 300), ability.name, 60, Il2CppTMPro.TextAlignmentOptions.Top);

            var cooldown = panel.AddText(new Info("CooldownText", 0, 0, 450, 300), "Cooldown: " + ability.cooldown, 55);
            panel.AddButton(new Info("AddCooldown", 250, 0, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { AbilityMethods.ChangeAbilityCooldown(cooldown, ability.name, 2f); }));
            panel.AddButton(new Info("RemoveCooldown", -250, 0, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { AbilityMethods.ChangeAbilityCooldown(cooldown, ability.name, -2f); }));
            return panel;
        }
        public static ModHelperPanel SplitshotPanel(AttackModel attack)
        {
            ModHelperPanel panel = ModHelperPanel.Create(new Info("SplitShotPanel", 0, 0, 800, 800), VanillaSprites.MainBgPanelHematite); ;
            panel.AddText(new Info("AttackName", 0, 225, 750, 300), attack.name + "\nSplitshot Model", 60, Il2CppTMPro.TextAlignmentOptions.Top);

            var splitshot = attack.GetDescendant<CreateProjectileOnContactModel>();
            var projectiles = panel.AddText(new Info("ProjectilesText", 0, 100, 450, 300), "Projectiles: " + splitshot.emission.TryCast<ArcEmissionModel>().count, 50);
            panel.AddButton(new Info("AddProjectile", 250, 100, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { SplitshotMethods.ChangeSplitshotProj(projectiles, attack.name, 1); }));
            panel.AddButton(new Info("RemoveProjectiles", -250, 100, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { SplitshotMethods.ChangeSplitshotProj(projectiles, attack.name, -1); }));

            var damage = panel.AddText(new Info("DamageText", 0, -50, 450, 300), "Damage: " + splitshot.projectile.GetDamageModel().damage, 55);
            panel.AddButton(new Info("AddDamage", 250, -50, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { SplitshotMethods.ChangeSplitshotDamage(damage, attack.name, 1); }));
            panel.AddButton(new Info("RemoveDamage", -250, -50, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { SplitshotMethods.ChangeSplitshotDamage(damage, attack.name, -1); }));

            var aimcone = panel.AddText(new Info("AimConeText", 0, -200, 450, 300), "Aim Cone: " + splitshot.emission.TryCast<ArcEmissionModel>().angle, 55);
            panel.AddButton(new Info("AddDamage", 250, -200, 100, 100), VanillaSprites.AddMoreBtn, new Action(() => { SplitshotMethods.ChangeSplitshotAimCone(aimcone, attack.name, 5); }));
            panel.AddButton(new Info("RemoveDamage", -250, -200, 100, 100), VanillaSprites.AddRemoveBtn, new Action(() => { SplitshotMethods.ChangeSplitshotAimCone(aimcone, attack.name, -5); }));
            return panel;
        }
    }

}
