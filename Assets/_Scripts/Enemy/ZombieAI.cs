using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public int colID;
    public bool isDead = false;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3f;
    public float chaseWaitTime = 5f;
    public float patrolWaitTime = 1f;
    public Transform[] patrolWayPoints;
    public AudioClip[] IdleSounds;

    private AudioSource sound;
    private ZombieSight zombieSight;
    private NavMeshAgent nav;
    private float chaseTimer;
    private float patrolTimer;
    private int wayPointIndex;
    private bool playAudio = false;

    private void Awake()
    {
        zombieSight = GetComponent<ZombieSight>();
        nav = GetComponent<NavMeshAgent>();
        sound = GetComponent<AudioSource>();

        wayPointIndex = 0;
        colID = 0;
    }

    private void Start()
    {
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (isDead == true)
            GetComponentInChildren<ZombieDeath>().Die(new Vector3(0, 0, 0), 0f, GetComponent<Rigidbody>());
    }

    private void Update()
    {
        playSoundIdle();
        if (zombieSight.personalLastSighting != zombieSight.resetPosition)
            Chassing();
        else
            Patrolling();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerController.instance != null && other.gameObject == PlayerController.instance.gameObject)
            colID++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlayerController.instance != null && other.gameObject == PlayerController.instance.gameObject)
            colID--;
    }

    private void playSoundIdle()
    {
        if (playAudio && !sound.isPlaying)
            sound.PlayOneShot(IdleSounds[Random.Range(0, IdleSounds.Length - 1)], 1.0f);
        playAudio = !playAudio;
    }

    private void Chassing()
    {
        nav.speed = chaseSpeed;

        Vector3 sightingDeltaPos = zombieSight.personalLastSighting - transform.position;

        if (sightingDeltaPos.sqrMagnitude > 0.2f)
        {
            if (zombieSight.playerInSight == false)
            {
                patrolTimer += Time.deltaTime;
                if (patrolTimer >= patrolWaitTime)
                {
                    patrolTimer = 0f;
                }
            }
            else
                patrolTimer = 0f;
            nav.destination = zombieSight.personalLastSighting;
        }

        if (nav.remainingDistance < nav.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;
            if (chaseTimer > chaseWaitTime)
            {
                zombieSight.personalLastSighting = zombieSight.resetPosition;
                chaseTimer = 0f;
            }
        }
        else
            chaseTimer = 0f;
    }

    private void Patrolling()
    {
        if (patrolWayPoints != null && patrolWayPoints.Length > 0)
        {
            nav.speed = patrolSpeed;

            if (nav.destination == zombieSight.resetPosition || nav.remainingDistance < nav.stoppingDistance)
            {
                patrolTimer += Time.deltaTime;

                if (patrolTimer >= patrolWaitTime)
                {
                    if (wayPointIndex == patrolWayPoints.Length - 1)
                       wayPointIndex = 0;
                    else
                       wayPointIndex++;
                    patrolTimer = 0f;
                }
            }
            else
                patrolTimer = 0f;

            if (patrolWayPoints[wayPointIndex])
                nav.destination = patrolWayPoints[wayPointIndex].position;
        }
    }
}
