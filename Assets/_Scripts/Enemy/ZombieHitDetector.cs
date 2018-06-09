using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHitDetector : MonoBehaviour
{
    public float forceToPush = 150f;
    public GameObject parentZombie;

    private ZombieAI zombieAI;
    private ZombieHit zombieHit;

    private void Awake()
    {
        zombieAI = parentZombie.GetComponent<ZombieAI>();
        zombieHit = parentZombie.GetComponent<ZombieHit>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (zombieAI)
        {
            if (collision.gameObject.tag == Tags.crowbar &&
                collision.gameObject.GetComponent<CrowbarGrab>().CollisionForce() >= forceToPush)
            {
                Hit();
            }
        }
    }

    public void Hit()
    {
        zombieHit.zombieIsHit = true;
    }
}
