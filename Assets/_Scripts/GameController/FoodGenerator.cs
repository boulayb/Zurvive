using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodGenerator : MonoBehaviour
{
    public GameObject[] FoodItems;
    public int TurnsBeforeDecrease = 1;
    public int NbToDecrease = 1;
    public int FoodAtStart = 6;
    public int MinimumFoodPresent = 2;
    public bool CanSpawnInSamePlace = false;

    private GameObject[] spawns;

    private void Start()
    {
        spawns = GameObject.FindGameObjectsWithTag(Tags.foodSpawn);
        int spawnsOffset = 1;

        int foodToSpawn = FoodAtStart - ((PlayerStats.DaysSurvived / TurnsBeforeDecrease) * NbToDecrease);
        if (foodToSpawn < MinimumFoodPresent)
            foodToSpawn = MinimumFoodPresent;

        for (int i = 0; i < foodToSpawn; i++)
        {
            int spawnPoint = Random.Range(0, spawns.Length - spawnsOffset);
            Instantiate(FoodItems[Random.Range(0, FoodItems.Length - 1)], spawns[spawnPoint].transform.position, spawns[spawnPoint].transform.rotation);
            if (CanSpawnInSamePlace == false)
            {
                GameObject tmp = spawns[spawns.Length - spawnsOffset];
                spawns[spawns.Length - spawnsOffset] = spawns[spawnPoint];
                spawns[spawnPoint] = tmp;
                spawnsOffset++;
                if (spawnsOffset > spawns.Length)
                    break;
            }
        }
    }
}