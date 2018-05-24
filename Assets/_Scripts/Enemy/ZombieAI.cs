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

    private ZombieSight zombieSight;
    private ZombieAttack zombieAttack;
    private NavMeshAgent nav;
    private float chaseTimer;
    private float patrolTimer;
    private int wayPointIndex;

    private void Awake()
    {
        zombieSight = GetComponent<ZombieSight>();
        zombieAttack = GetComponent<ZombieAttack>();
        nav = GetComponent<NavMeshAgent>();

        wayPointIndex = 0;
        colID = 0;
    }

    private void Start()
    {
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (isDead == true)
            GetComponentInChildren<ZombieDeath>().Die();
    }

    private void Update()
    {
        if (zombieAttack.playerInRange)
            Attacking();
        else if (zombieSight.personalLastSighting != zombieSight.resetPosition)
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

    private void SetRotationOnly(bool rotationOnly)
    {
        if (rotationOnly)
        {
            nav.updatePosition = false;
            nav.angularSpeed = 999f;
            nav.speed = 15f;
            nav.acceleration = 20f;
        }
        else
        {
            nav.angularSpeed = 400f;
            nav.speed = 3.5f;
            nav.acceleration = 8f;
            nav.Warp(transform.position);
            nav.updatePosition = true;
        }
    }

    private void Attacking()
    {
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

        nav.destination = patrolWayPoints[wayPointIndex].position;
    }
}
