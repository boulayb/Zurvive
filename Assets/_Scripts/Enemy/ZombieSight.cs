using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSight : MonoBehaviour
{
    public float fieldOfViewAngle = 110f;
    public bool playerInSight;
    public Vector3 personalLastSighting;
    public Vector3 resetPosition = new Vector3(5000f, 5000f, 5000f);

    private NavMeshAgent nav;
    private SphereCollider col;
    private Animator anim;
    private ZombieAI zombieAI;
    private Vector3 viewOffset = new Vector3(0, 1.6f, 0.5f);

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        zombieAI = GetComponent<ZombieAI>();

        playerInSight = false;
        personalLastSighting = resetPosition;
    }

    private void Update()
    {
        anim.SetBool(HashID.instance.playerInSightBool, playerInSight);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.sound && playerInSight == false)
            personalLastSighting = PlayerController.instance.gameObject.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (zombieAI.colID >= 1 && other.gameObject == PlayerController.instance.gameObject)
        {
            playerInSight = false;

            CapsuleCollider playerCol = PlayerController.instance.getCollider();
            Transform playerBox = playerCol.transform.parent;
            Quaternion playerQuat = Quaternion.identity;
            Quaternion selfQuat = Quaternion.identity;
            playerQuat.SetLookRotation(playerBox.forward, Vector3.Cross(playerBox.forward, playerBox.right));
            selfQuat.SetLookRotation(transform.forward, Vector3.Cross(transform.forward, transform.right));
            Vector3 direction = ((other.transform.position + (playerQuat * playerCol.center)) - (transform.position + (selfQuat * new Vector3(0f, 0f, viewOffset.z)))) - (other.transform.up * (2f - playerCol.center.y));
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                //Debug.DrawRay(transform.position + (quat2 * viewOffset), direction, Color.red);
                if (Physics.Raycast(transform.position + (selfQuat * viewOffset), direction.normalized, out hit, col.radius))
                {
                    //Debug.Log("obj = " + hit.collider.gameObject);
                    if (hit.collider.gameObject == PlayerController.instance.gameObject || hit.collider.gameObject.tag == Tags.player)
                    {
                        //Debug.Log("PLAYER");
                        playerInSight = true;
                        personalLastSighting = PlayerController.instance.gameObject.transform.position;
                    }
                }

            }

            if (playerInSight == false && PlayerController.instance.getSpeed() > PlayerController.instance.MaxSpeedNoise)
               if (CalculatePathLength(PlayerController.instance.gameObject.transform.position) <= col.radius)
                    personalLastSighting = PlayerController.instance.gameObject.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (zombieAI.colID >= 1 && other.gameObject == PlayerController.instance.gameObject)
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