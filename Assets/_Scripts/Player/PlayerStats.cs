using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    private static int energy, maxEnergy, daysSurvived, bullets1, bullets2, bullets3, bulletsGun;
    private static bool isDemo, playerDead, melee, gun;

    public static int Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = value;
        }
    }

    public static int MaxEnergy
    {
        get
        {
            return maxEnergy;
        }
        set
        {
            maxEnergy = value;
        }
    }

    public static int DaysSurvived
    {
        get
        {
            return daysSurvived;
        }
        set
        {
            daysSurvived = value;
        }
    }

    public static bool IsDemo
    {
        get
        {
            return isDemo;
        }
        set
        {
            isDemo = value;
        }
    }

    public static bool PlayerDead
    {
        get
        {
            return playerDead;
        }
        set
        {
            playerDead = value;
        }
    }

    public static bool Melee
    {
        get
        {
            return melee;
        }
        set
        {
            melee = value;
        }
    }

    public static bool Gun
    {
        get
        {
            return gun;
        }
        set
        {
            gun = value;
        }
    }

    public static int Bullets1
    {
        get
        {
            return bullets1;
        }
        set
        {
            bullets1 = value;
        }
    }

    public static int Bullets2
    {
        get
        {
            return bullets2;
        }
        set
        {
            bullets2 = value;
        }
    }

    public static int Bullets3
    {
        get
        {
            return bullets3;
        }
        set
        {
            bullets3 = value;
        }
    }

    public static int BulletsGun
    {
        get
        {
            return bulletsGun;
        }
        set
        {
            bulletsGun = value;
        }
    }
}
