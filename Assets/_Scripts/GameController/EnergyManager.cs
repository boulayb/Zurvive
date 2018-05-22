using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public int MaxEnergy = 1000;
    public float sprintingThreshold = 8.0f;
    public float runningThreshold = 4.5f;
    public float walkingThreshold = 2f;
    public int energyLostSprinting = 3;
    public int energyLostRunning = 2;
    public int energyLostWalking = 1;
    public int energyLostHitting = 25;
    public int energyLostKilling = 50;

    public enum EnergyEventName
    {
        HITTING,
        KILLING
    }

    private bool waiting = false;
    private int energy;
    private PlayerController playerController;

    private void Start()
    {
        energy = MaxEnergy;
        StartCoroutine(LateStart(3));
    }

    private IEnumerator LateStart(float waitTime) // LATE INIT BECAUSE OF VRTK TAKING TIME TO LOAD
    {
        yield return new WaitForSeconds(waitTime);

        playerController = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!waiting)
        {
            waiting = true;
            float speed = playerController.getSpeed();
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
    }
}