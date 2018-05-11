using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieAttack : MonoBehaviour
{
    public bool playerInRange;

    private Animator anim;
    private HashID hash;
    private GameObject player;
    private ZombieAI zombieAI;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        zombieAI = GetComponent<ZombieAI>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashID>();

        playerInRange = false;
    }

    private void Start()
    {
        StartCoroutine(LateStart(3));
    }

    private IEnumerator LateStart(float waitTime) // LATE INIT BECAUSE OF VRTK TAKING TIME TO LOAD
    {
        yield return new WaitForSeconds(waitTime);

        player = GameObject.FindGameObjectWithTag(Tags.player);
    }

    private void Update()
    {
        anim.SetBool(hash.playerInRangeBool, playerInRange);
        float attack = anim.GetFloat(hash.attackFloat);

        if (attack > 0.4 && playerInRange)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (zombieAI.colID == 2 && other.gameObject == player)
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (zombieAI.colID == 2 && other.gameObject == player)
            playerInRange = false;
    }
}
