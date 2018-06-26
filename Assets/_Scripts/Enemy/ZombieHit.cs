using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHit : MonoBehaviour
{
    public bool zombieIsHit = false;

    private Animator anim;
    private ZombieAI zombieAI;
    private ZombieSight zombieSight;
    private bool wait = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        zombieAI = GetComponent<ZombieAI>();
        zombieSight = GetComponent<ZombieSight>();
    }

    private void Update()
    {
        if (zombieAI)
        {
            anim.SetBool(HashID.instance.zombieIsHit, zombieIsHit);
            zombieIsHit = false;
        }
    }
}
