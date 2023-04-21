using MelonLoader;
using BTD_Mod_Helper;
using TowerCheat;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using UnityEngine;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Extensions;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Simulation.Towers;
using System;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using System.Linq;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppNinjaKiwi.Common;
using TowerCheat.TowerMethod;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using UnityEngine.Events;
using Il2CppAssets.Scripts.Unity.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using BTD_Mod_Helper.Api;
using TowerCheat.UI;
using TowerCheat.UIPanels;

[assembly: MelonInfo(typeof(TowerCheat.TowerCheat), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace TowerCheat;

public class TowerCheat : BloonsTD6Mod
{
    static bool IsMenuOpen = false;
    static GameObject upgrades;
    static GameObject tower;
    static ModHelperScrollPanel panel;
    public override void OnApplicationStart()
    {

    }
    public override void OnTowerSelected(Tower tower)
    {
        base.OnTowerSelected(tower);
        CloseMenu();
    }

    public override void OnMatchStart()
    {
        base.OnMatchStart();
        tower = GameObject.Find("TowerElements");
        upgrades = GameObject.Find("SelectedTowerOptions");
        panel = tower.AddModHelperScrollPanel(new Info("CheatPanel", 0, -333, 900, 1100), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanel, 50, 100);
        panel.ScrollRect.verticalScrollbarSpacing = 100;
        

        panel.SetActive(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyUp(KeyCode.F1))
        {
            if (IsMenuOpen)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }
    public void CloseMenu()
    {
        IsMenuOpen = false;
        upgrades.active = true;
        panel.SetActive(false);
    }
    public void OpenMenu()
    {
        if (InGame.instance.inputManager.SelectedTower != null)
        {
            IsMenuOpen = true;
            upgrades.active = false;
            panel.SetActive(true);

            LoadScrollContent();
        }
    }
    
    public static void LoadScrollContent(bool DestroyOldContent = true)
    {
        if (DestroyOldContent) { panel.ScrollContent.transform.DestroyAllChildren(); }
        panel.AddScrollContent(Panels.Tower(InGame.instance.inputManager.SelectedTower.tower));
        panel.AddScrollContent(Panels.Divider());
        foreach (var attack in InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().GetAttackModels())
        {
            panel.AddScrollContent(Panels.Weapon(attack));
            if (Panels.EffectsPanel(attack) != null)
            {
                panel.AddScrollContent(Panels.EffectsPanel(attack));
            }
            if (attack.weapons[0].projectile.HasBehavior<CreateProjectileOnContactModel>()) { panel.AddScrollContent(Panels.SplitshotPanel(attack)); }
            if (attack.weapons[0].projectile.HasBehavior<WindModel>()) { panel.AddScrollContent(Panels.WindPanel(attack)); }
            if (attack.weapons[0].emission.Is<ArcEmissionModel>()) { panel.AddScrollContent(Panels.MultishotPanel(attack)); }
            panel.AddScrollContent(Panels.Divider());
        }
        foreach (var abil in InGame.instance.inputManager.SelectedTower.tower.rootModel.Cast<TowerModel>().GetAbilities())
        {
            panel.AddScrollContent(Panels.AbilityPanel(abil));
            panel.AddScrollContent(Panels.Divider());
        }
    }
}