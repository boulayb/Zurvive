using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    private static int energy, maxEnergy, daysSurvived;
    private static bool playerDead;

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
}
