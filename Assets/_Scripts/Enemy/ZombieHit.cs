using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHit : MonoBehaviour
{
    public bool zombieIsHit;

    private Animator anim;
    private HashID hash;
    private ZombieAI zombieAI;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        zombieAI = GetComponent<ZombieAI>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashID>();

        zombieIsHit = false;
    }

    private void Update()
    {
        if (zombieAI.isDead == false)
        {
            anim.SetBool(hash.zombieIsHit, zombieIsHit);
            if (anim.GetCurrentAnimatorStateInfo(2).IsName("Zombie Reaction Hit"))
                zombieIsHit = false;
        }
    }
}
