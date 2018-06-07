using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    public GameObject[] Zombies;
    public int TurnsBeforeIncrease = 1;
    public int NbToIncrease = 2;
    public int ZombiesAtStart = 2;
    public int MaximumZombiesPresent = 12;
    public bool CanSpawnInSamePlace = false;

    private GameObject[] spawns;

    private void Start()
    {
        spawns = GameObject.FindGameObjectsWithTag(Tags.zombieSpawn);
        int spawnsOffset = 1;

        int zombiesToSpawn = ZombiesAtStart + ((PlayerStats.DaysSurvived / TurnsBeforeIncrease) * NbToIncrease);
        if (zombiesToSpawn > MaximumZombiesPresent)
            zombiesToSpawn = MaximumZombiesPresent;

        for (int i = 0; i < zombiesToSpawn; i++)
        {
            int spawnPoint = Random.Range(0, spawns.Length - spawnsOffset);
            ZombieAI zombie = Instantiate(Zombies[Random.Range(0, Zombies.Length - 1)], spawns[spawnPoint].transform.position, spawns[spawnPoint].transform.rotation).GetComponent<ZombieAI>();
            zombie.patrolWayPoints = new Transform[spawns[spawnPoint].transform.childCount];
            int patrolIndex = 0;
            foreach (Transform child in spawns[spawnPoint].transform)
            {
                if (child.gameObject.tag == Tags.patrolPoint)
                {
                    zombie.patrolWayPoints[patrolIndex] = child;
                    patrolIndex++;
                }
            }
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
