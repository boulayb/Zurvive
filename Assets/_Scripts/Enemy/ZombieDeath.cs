using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieDeath : MonoBehaviour
{
    public float forceToKill = 850f;
    public GameObject parentZombie;
    public ZombieAI zombieAI;

    private void Awake()
    {
        zombieAI = parentZombie.GetComponent<ZombieAI>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (zombieAI.isDead == false)
        {
            if (collision.gameObject.tag == Tags.weapons &&
                collision.contacts[0].thisCollider.tag == Tags.head &&
                collision.gameObject.GetComponent<CrowbarGrab>().CollisionForce() >= forceToKill)
            {
                Die();
            }
        }
    }

    private void SetKinematic(bool newValue)
    {
        Destroy(parentZombie.GetComponent<Rigidbody>());
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
        SetKinematic(false);
        parentZombie.GetComponent<Animator>().enabled = false;
        parentZombie.GetComponent<NavMeshAgent>().enabled = false;
        SetLayer("Dead");
    }
}
