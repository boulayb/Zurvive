using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHit : MonoBehaviour
{
    public bool zombieIsHit;

    private Animator anim;
    private ZombieAI zombieAI;
    private bool energyLost = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        zombieAI = GetComponent<ZombieAI>();

        zombieIsHit = false;
    }

    private void Update()
    {
        if (zombieAI)
        {
            anim.SetBool(HashID.instance.zombieIsHit, zombieIsHit);
            if (anim.GetCurrentAnimatorStateInfo(2).IsName("Zombie Reaction Hit"))
            {
                if (energyLost == false)
                    EnergyManager.instance.LooseEnergy(EnergyManager.EnergyEventName.HITTING);
                energyLost = true;
                zombieIsHit = false;
            }
            if (anim.IsInTransition(2) && anim.GetNextAnimatorStateInfo(2).IsName("New State"))
            {
                energyLost = false;
            }
        }
    }
}
