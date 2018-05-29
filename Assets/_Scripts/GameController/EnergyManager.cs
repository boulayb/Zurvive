using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager instance = null;

    public int MaxEnergy = 1000;
    public float sprintingThreshold = 8.0f;
    public float runningThreshold = 4.5f;
    public float walkingThreshold = 2f;
    public int energyLostSprinting = 3;
    public int energyLostRunning = 2;
    public int energyLostWalking = 1;
    public int energyLostHitting = 25;
    public int energyLostKilling = 50;
    public int energyLostNextDay = 100;

    public enum EnergyEventName
    {
        HITTING,
        KILLING,
        NEXTDAY
    }

    private bool waiting = false;
    private int energy;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        if (PlayerStats.Energy <= 0)
            energy = MaxEnergy;
        PlayerStats.MaxEnergy = MaxEnergy;
    }

    private void Update()
    {
        if (!waiting && PlayerController.instance != null)
        {
            waiting = true;
            float speed = PlayerController.instance.getSpeed();
            if (speed > sprintingThreshold)
                energy -= energyLostSprinting;
            else if (speed > runningThreshold)
                energy -= energyLostRunning;
            else if (speed > walkingThreshold)
                energy -= energyLostWalking;
            StartCoroutine(WaitTime(1));
        }
    }

    private IEnumerator WaitTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        waiting = false;
    }

    public int GetEnergy()
    {
        return energy;
    }

    public void LooseEnergy(EnergyEventName eventName)
    {
        if (eventName == EnergyEventName.HITTING)
            energy -= energyLostHitting;
        else if (eventName == EnergyEventName.KILLING)
            energy -= energyLostKilling;
        else if (eventName == EnergyEventName.NEXTDAY)
            energy -= energyLostNextDay;
    }
}