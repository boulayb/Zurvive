using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieDeath : MonoBehaviour
{
    public float forceToKill = 850f;
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
            if (collision.gameObject.tag == Tags.weapons &&
                collision.gameObject.GetComponent<CrowbarGrab>().CollisionForce() >= forceToKill)
            {
                EnergyManager.instance.LooseEnergy(EnergyManager.EnergyEventName.KILLING);
                Die();
            }
            else if (collision.gameObject.tag == Tags.weapons &&
                collision.gameObject.GetComponent<CrowbarGrab>().CollisionForce() >= forceToPush)
            {
                zombieHit.zombieIsHit = true;
            }
        }
    }

    private void SetKinematic(bool newValue)
    {
        Rigidbody[] bodies = parentZombie.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = newValue;
        }
    }

    private void SetLayer(string layer)
    {
        parentZombie.layer = LayerMask.NameToLayer(layer);
        Transform[] allChildren = parentZombie.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }

    public void Die()
    {
        zombieAI.isDead = true;
        Destroy(parentZombie.GetComponent<Rigidbody>());
        Destroy(parentZombie.GetComponent<Animator>());
        Destroy(parentZombie.GetComponent<NavMeshAgent>());
        Destroy(parentZombie.GetComponent<SphereCollider>());
        Destroy(parentZombie.GetComponent<CapsuleCollider>());
        Destroy(parentZombie.GetComponent<ZombieAI>());
        Destroy(parentZombie.GetComponent<ZombieAnimation>());
        Destroy(parentZombie.GetComponent<ZombieAttack>());
        Destroy(parentZombie.GetComponent<ZombieHit>());
        Destroy(parentZombie.GetComponent<ZombieSight>());
        SetKinematic(false);
        SetLayer("Dead");
    }
}
