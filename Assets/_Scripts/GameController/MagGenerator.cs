using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MagGenerator : MonoBehaviour
{
    public GameObject[] MagItems;
    public int TurnsBeforeDecrease = 1;
    public int NbToDecrease = 1;
    public int MagAtStart = 6;
    public int MinimumMagPresent = 1;
    public bool CanSpawnInSamePlace = false;
    public int MaxBulletInMag = 10;

    private GameObject[] spawns;

    private void Start()
    {
        spawns = GameObject.FindGameObjectsWithTag(Tags.magSpawn);
        int spawnsOffset = 1;

        int magToSpawn = MagAtStart - ((PlayerStats.DaysSurvived / TurnsBeforeDecrease) * NbToDecrease);
        if (magToSpawn < MinimumMagPresent)
            magToSpawn = MinimumMagPresent;

        for (int i = 0; i < magToSpawn; i++)
        {
            int spawnPoint = Random.Range(0, spawns.Length - spawnsOffset);
            Instantiate(MagItems[Random.Range(0, MagItems.Length - 1)], spawns[spawnPoint].transform.position, spawns[spawnPoint].transform.rotation).GetComponent<GunAmmo>().SetBullets(Random.Range(0, MaxBulletInMag));
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
