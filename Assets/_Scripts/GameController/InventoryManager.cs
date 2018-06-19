using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadInventory();
    }

    private void saveGun(ZurviveGun gun)
    {
        if (gun != null)
        {
            PlayerStats.Gun = true;
            if (gun.HasMag == true)
                PlayerStats.BulletsGun = gun.Bullets;
            else
                PlayerStats.BulletsGun = -1;
        }
    }

    public void ResetInventory()
    {
        PlayerStats.Bullets1 = -1;
        PlayerStats.Bullets2 = -1;
        PlayerStats.Bullets3 = -1;
        PlayerStats.BulletsGun = -1;
        PlayerStats.Gun = false;
        PlayerStats.Melee = false;
    }

    public void LoadInventory()
    {
        if (PlayerStats.Gun == true)
            HipStorage.instance.AttachGun(PlayerStats.BulletsGun);
        if (PlayerStats.Melee == true)
            HipStorage.instance.AttachMelee();
        if (PlayerStats.Bullets1 > -1)
            HipStorage.instance.AttachAmmo1(PlayerStats.Bullets1);
        if (PlayerStats.Bullets2 > -1)
            HipStorage.instance.AttachAmmo2(PlayerStats.Bullets2);
        if (PlayerStats.Bullets3 > -1)
            HipStorage.instance.AttachAmmo3(PlayerStats.Bullets3);
        ResetInventory();
    }

    public void SaveInventory()
    {
        if (HipStorage.instance.GetGun() != null)
            saveGun(HipStorage.instance.GetGun().GetComponent<ZurviveGun>());
        if (HipStorage.instance.GetMelee() != null)
            PlayerStats.Melee = true;
        if (HipStorage.instance.GetAmmo1() != null)
            PlayerStats.Bullets1 = HipStorage.instance.GetAmmo1().GetComponent<GunAmmo>().Bullets;
        if (HipStorage.instance.GetAmmo2() != null)
            PlayerStats.Bullets2 = HipStorage.instance.GetAmmo2().GetComponent<GunAmmo>().Bullets;
        if (HipStorage.instance.GetAmmo3() != null)
            PlayerStats.Bullets3 = HipStorage.instance.GetAmmo3().GetComponent<GunAmmo>().Bullets;
        GameObject leftObject = GameObject.FindWithTag(Tags.leftController).GetComponent<VRTK_InteractGrab>().GetGrabbedObject();
        GameObject rightObject = GameObject.FindWithTag(Tags.rightController).GetComponent<VRTK_InteractGrab>().GetGrabbedObject();
        if (leftObject != null)
            switch (leftObject.tag)
            {
                case Tags.gun:
                    saveGun(leftObject.GetComponent<ZurviveGun>());
                    break;
                case Tags.crowbar:
                    PlayerStats.Melee = true;
                    break;
                case Tags.magazine:
                    if (PlayerStats.Bullets1 == -1)
                        PlayerStats.Bullets1 = leftObject.GetComponent<GunAmmo>().Bullets;
                    else if (PlayerStats.Bullets2 == -1)
                        PlayerStats.Bullets2 = leftObject.GetComponent<GunAmmo>().Bullets;
                    else if (PlayerStats.Bullets3 == -1)
                        PlayerStats.Bullets3 = leftObject.GetComponent<GunAmmo>().Bullets;
                    break;
                case Tags.food:
                    EnergyManager.instance.AddEnergy(EnergyManager.instance.energyGainedFood);
                    break;
            }
        if (rightObject != null)
            switch (rightObject.tag)
            {
                case Tags.gun:
                    saveGun(rightObject.GetComponent<ZurviveGun>());
                    break;
                case Tags.crowbar:
                    PlayerStats.Melee = true;
                    break;
                case Tags.magazine:
                    if (PlayerStats.Bullets1 == -1)
                        PlayerStats.Bullets1 = rightObject.GetComponent<GunAmmo>().Bullets;
                    else if (PlayerStats.Bullets2 == -1)
                        PlayerStats.Bullets2 = rightObject.GetComponent<GunAmmo>().Bullets;
                    else if (PlayerStats.Bullets3 == -1)
                        PlayerStats.Bullets3 = rightObject.GetComponent<GunAmmo>().Bullets;
                    break;
                case Tags.food:
                    EnergyManager.instance.AddEnergy(EnergyManager.instance.energyGainedFood);
                    break;
            }
    }
}
