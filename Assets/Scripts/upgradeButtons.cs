using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class upgradeButtons : MonoBehaviour
{
    public int pistolROFUpgrade;
    public int pistolDamageUpgrade;
    public int pistolReloadSpeedUpgrade;

    public int shotgunROFUpgrade;
    public int shotgunDamageUpgrade;
    public int shotgunReloadSpeedUpgrade;

    public int rifleROFUpgrade;
    public int rifleDamageUpgrade;
    public int rifleReloadSpeedUpgrade;

    public int machinegunROFUpgrade;
    public int machinegunDamageUpgrade;
    public int machinegunReloadSpeedUpgrade;

    [SerializeField]public List<Button> upgradebuttons;

    GameObject playerWeapons;
    playerController player;
    
    void Start()
    {
        for (int i = 0; i < upgradebuttons.Count; i++)
        {
            upgradebuttons[i].interactable = false;
        }
        playerWeapons = GameObject.Find("Player");
        player = playerWeapons.GetComponent<playerController>();
    }

    void Update()
    {
        if (player.skillPoints >= pistolROFUpgrade)
        {
            // Upgrade becomes available 
            upgradebuttons[0].interactable = true;
        }

        if (player.skillPoints >= pistolDamageUpgrade)
        {
            upgradebuttons[1].interactable = true;
        }

        if (player.skillPoints >= pistolReloadSpeedUpgrade)
        {
            upgradebuttons[2].interactable = true;
        }

        if (player.skillPoints >= shotgunROFUpgrade)
        {
            upgradebuttons[3].interactable = true;
        }

        if (player.skillPoints >= shotgunDamageUpgrade)
        {
            upgradebuttons[4].interactable = true;
        }

        if (player.skillPoints >= shotgunReloadSpeedUpgrade)
        {
            upgradebuttons[5].interactable = true;
        }

        if (player.skillPoints >= rifleROFUpgrade)
        {
            upgradebuttons[6].interactable = true;
        }

        if (player.skillPoints >= rifleDamageUpgrade)
        {
            upgradebuttons[7].interactable = true;
        }

        if (player.skillPoints >= rifleReloadSpeedUpgrade)
        {
            upgradebuttons[8].interactable = true;
        }

        if (player.skillPoints >= machinegunROFUpgrade)
        {
            upgradebuttons[9].interactable = true;
        }

        if (player.skillPoints >= machinegunDamageUpgrade)
        {
            upgradebuttons[10].interactable = true;
        }

        if (player.skillPoints >= machinegunReloadSpeedUpgrade)
        {
            upgradebuttons[11].interactable = true;
        }
    }

    public void upgradeWeapon()
    {
        string upgrade = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(upgrade);

        // Checks to see which upgrade the player has selected
        if (upgrade == "pistolROF")
        {
            // Adds value on to current value
            player.pistol_RateOfFire *= 0.9f;
            // Takes away upgrade cost from score
            player.skillPoints -= pistolROFUpgrade;
            // Adds value on to upgrade cost
            pistolROFUpgrade += 1;
            upgradebuttons[0].interactable = false;
        }

        if (upgrade == "pistolDamage")
        {
            player.pistol_Dammage += 1;
            player.skillPoints -= pistolDamageUpgrade;
            pistolDamageUpgrade += 1;
            upgradebuttons[1].interactable = false;
        }

        if (upgrade == "pistolReloadSpeed")
        {
            player.pistol_ReloadSpeed *= 0.9f;
            player.skillPoints -= pistolReloadSpeedUpgrade;
            pistolReloadSpeedUpgrade += 1;
            upgradebuttons[2].interactable = false;
        }

        if (upgrade == "shotgunROF")
        {
            player.shotgun_RateOfFire *= 0.9f;
            player.skillPoints -= shotgunROFUpgrade;
            shotgunROFUpgrade += 1;
            upgradebuttons[3].interactable = false;
        }

        if (upgrade == "shotgunDamage")
        {
            player.shotgun_Dammage += 1;
            player.skillPoints -= shotgunDamageUpgrade;
            shotgunDamageUpgrade += 1;
            upgradebuttons[4].interactable = false;
        }

        if (upgrade == "shotgunReloadSpeed")
        {
            player.shotgun_ReloadSpeed *= 0.9f;
            player.skillPoints -= shotgunReloadSpeedUpgrade;
            shotgunReloadSpeedUpgrade += 1;
            upgradebuttons[5].interactable = false;
        }

        if (upgrade == "rifleROF")
        {
            player.rifle_RateOfFire *= 0.9f;
            player.skillPoints -= rifleROFUpgrade;
            rifleROFUpgrade += 1;
            upgradebuttons[6].interactable = false;
        }

        if (upgrade == "rifleDamage")
        {
            player.rifle_Dammage += 1;
            player.skillPoints -= rifleDamageUpgrade;
            rifleDamageUpgrade += 1;
            upgradebuttons[7].interactable = false;
        }

        if (upgrade == "rifleReloadSpeed")
        {
            player.rifle_ReloadSpeed *= 0.9f;
            player.skillPoints -= rifleReloadSpeedUpgrade;
            rifleReloadSpeedUpgrade += 1;
            upgradebuttons[8].interactable = false;
        }

        if (upgrade == "machinegunROF")
        {
            player.machineGun_RateOfFire *= 0.9f;
            player.skillPoints -= machinegunROFUpgrade;
            machinegunROFUpgrade += 1;
            upgradebuttons[9].interactable = false;
        }

        if (upgrade == "machinegunDamage")
        {
            player.machineGun_Dammage += 1;
            player.skillPoints -= machinegunDamageUpgrade;
            machinegunDamageUpgrade += 1;
            upgradebuttons[10].interactable = false;
        }

        if (upgrade == "machinegunReloadSpeed")
        {
            player.machineGun_ReloadSpeed *= 0.9f;
            player.skillPoints -= machinegunReloadSpeedUpgrade;
            machinegunReloadSpeedUpgrade += 1;
            upgradebuttons[11].interactable = false;
        }
    }

    
}
