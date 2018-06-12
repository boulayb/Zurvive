using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletGenerator : MonoBehaviour
{
    public GameObject[] BulletItems;
    public int TurnsBeforeDecrease = 1;
    public int NbToDecrease = 5;
    public int BulletAtStart = 30;
    public int MinimumBulletPresent = 5;
    public bool CanSpawnInSamePlace = true;

    private GameObject[] spawns;

    private void Start()
    {
        spawns = GameObject.FindGameObjectsWithTag(Tags.bulletSpawn);
        int spawnsOffset = 1;

        int bulletToSpawn = BulletAtStart - ((PlayerStats.DaysSurvived / TurnsBeforeDecrease) * NbToDecrease);
        if (bulletToSpawn < MinimumBulletPresent)
            bulletToSpawn = MinimumBulletPresent;

        for (int i = 0; i < bulletToSpawn; i++)
        {
            int spawnPoint = Random.Range(0, spawns.Length - spawnsOffset);
            Instantiate(BulletItems[Random.Range(0, BulletItems.Length - 1)], spawns[spawnPoint].transform.position, spawns[spawnPoint].transform.rotation);
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
