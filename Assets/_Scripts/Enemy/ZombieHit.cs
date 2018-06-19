using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHit : MonoBehaviour
{
    public bool zombieIsHit = false;

    private Animator anim;
    private ZombieAI zombieAI;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        zombieAI = GetComponent<ZombieAI>();
    }

    private void Update()
    {
        if (zombieAI)
        {
            anim.SetBool(HashID.instance.zombieIsHit, zombieIsHit);
            if (anim.GetCurrentAnimatorStateInfo(2).IsName("Zombie Reaction Hit"))
                zombieIsHit = false;
        }
    }
}
