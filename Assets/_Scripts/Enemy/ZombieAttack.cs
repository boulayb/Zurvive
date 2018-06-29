using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public bool playerInRange;

    private Animator anim;
    private ZombieAI zombieAI;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        zombieAI = GetComponent<ZombieAI>();

        playerInRange = false;
    }

    private void Update()
    {
        anim.SetBool(HashID.instance.playerInRangeBool, playerInRange);
        float attack = anim.GetFloat(HashID.instance.attackFloat);

        if (attack > 0.1 && playerInRange)
        {
            PlayerController.instance.Die(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (zombieAI.colID == 2 && other.gameObject == PlayerController.instance.gameObject)
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (zombieAI.colID == 2 && other.gameObject == PlayerController.instance.gameObject)
            playerInRange = false;
    }
}
