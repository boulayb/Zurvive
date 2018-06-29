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
            float force = collision.gameObject.GetComponent<CrowbarGrab>().CollisionForce();
            if (collision.gameObject.tag == Tags.crowbar && force >= forceToKill)
                Die(-(collision.contacts[0].point - parentZombie.transform.position).normalized, force, collision.rigidbody);
            else if (collision.gameObject.tag == Tags.crowbar && force >= forceToPush)
                zombieHit.zombieIsHit = true;
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

    public void Die(Vector3 dir, float force, Rigidbody rb)
    {
        zombieAI.isDead = true;
        Destroy(parentZombie.GetComponent<Animator>());
        Destroy(parentZombie.GetComponent<NavMeshAgent>());
        Destroy(parentZombie.GetComponent<SphereCollider>());
        Destroy(parentZombie.GetComponent<CapsuleCollider>());
        Destroy(parentZombie.GetComponent<ZombieAI>());
        Destroy(parentZombie.GetComponent<ZombieAnimation>());
        Destroy(parentZombie.GetComponent<ZombieAttack>());
        Destroy(parentZombie.GetComponent<ZombieHit>());
        Destroy(parentZombie.GetComponent<ZombieSight>());
        Destroy(parentZombie.GetComponent<Rigidbody>());
        SetKinematic(false);
        SetLayer("Dead");
        rb.AddForce(dir * (force * 2));
    }
}
