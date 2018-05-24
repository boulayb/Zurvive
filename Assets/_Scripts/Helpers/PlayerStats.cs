using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static int Energy
    {
        get
        {
            return Energy;
        }
        set
        {
            Energy = value;
        }
    }

    public static int MaxEnergy
    {
        get
        {
            return MaxEnergy;
        }
        set
        {
            MaxEnergy = value;
        }
    }

    public static int DaysSurvived
    {
        get
        {
            return DaysSurvived;
        }
        set
        {
            DaysSurvived = value;
        }
    }

    public static bool PlayerDead
    {
        get
        {
            return PlayerDead;
        }
        set
        {
            PlayerDead = value;
        }
    }
}
