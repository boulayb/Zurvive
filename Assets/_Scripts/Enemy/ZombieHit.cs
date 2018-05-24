using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHit : MonoBehaviour
{
    public bool zombieIsHit;

    private Animator anim;
    private ZombieAI zombieAI;

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
                EnergyManager.instance.LooseEnergy(EnergyManager.EnergyEventName.HITTING);
                zombieIsHit = false;
            }
        }
    }
}
