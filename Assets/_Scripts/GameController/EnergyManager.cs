using System.Collections;
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
    public int energyLostNextDay = 100;
    public int energyGainedFood = 100;

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
        else
            energy = PlayerStats.Energy;
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
            if (energy <= 0)
            {
                PlayerController.instance.Die(false);
            }
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

    public void LooseEnergy(int energyLost)
    {
        energy -= energyLost;
    }

    public void AddEnergy(int energyAdded)
    {
        energy += energyAdded;
        if (energy > MaxEnergy)
            energy = MaxEnergy;
    }
}