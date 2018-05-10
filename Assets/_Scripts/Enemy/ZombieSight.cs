using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSight : MonoBehaviour
{
    public float fieldOfViewAngle = 110f;
    public float maxSpeedNoise = 3f;
    public bool playerInSight;
    public Vector3 personalLastSighting;
    public Vector3 resetPosition = new Vector3(5000f, 5000f, 5000f);

    private NavMeshAgent nav;
    private SphereCollider col;
    private Animator anim;
    private GameObject player;
    private HashID hash;
    private ZombieAI zombieAI;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        zombieAI = GetComponent<ZombieAI>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashID>();

        playerInSight = false;
        personalLastSighting = resetPosition;
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
        if (zombieAI.isDead == false)
            anim.SetBool(hash.playerInSightBool, playerInSight);
    }

    private void OnTriggerStay(Collider other)
    {
        if (zombieAI.colID >= 1 && zombieAI.isDead == false && other.gameObject == player)
        {
            playerInSight = false;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                    if (hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        personalLastSighting = player.transform.position;
                    }
            }

            if (playerInSight == false && player.GetComponent<PlayerController>().getSpeed() > maxSpeedNoise)
                if (CalculatePathLength(player.transform.position) <= col.radius)
                    personalLastSighting = player.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (zombieAI.colID >= 1 && zombieAI.isDead == false && other.gameObject == player)
            playerInSight = false;
    }

    private float CalculatePathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if (nav.enabled)
            nav.CalculatePath(targetPosition, path);

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++)
            allWayPoints[i + 1] = path.corners[i];

        float pathLength = 0f;

        for (int i = 0; i < allWayPoints.Length - 1; i++)
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);

        return pathLength;
    }
}