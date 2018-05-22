using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHit : MonoBehaviour
{
    public bool zombieIsHit;

    private Animator anim;
    private HashID hash;
    private ZombieAI zombieAI;
    private EnergyManager energyManager;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        zombieAI = GetComponent<ZombieAI>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashID>();
        energyManager = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<EnergyManager>();

        zombieIsHit = false;
    }

    private void Update()
    {
        if (zombieAI)
        {
            anim.SetBool(hash.zombieIsHit, zombieIsHit);
            if (anim.GetCurrentAnimatorStateInfo(2).IsName("Zombie Reaction Hit"))
            {
                energyManager.LooseEnergy(EnergyManager.EnergyEventName.HITTING);
                zombieIsHit = false;
            }
        }
    }
}
