using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAnimation : MonoBehaviour
{
    public float deadZone = 5f;

    private GameObject player;
    private ZombieAttack zombieAttack;
    private NavMeshAgent nav;
    private Animator anim;
    private HashID hash;
    private AnimatorSetup animSetup;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        zombieAttack = GetComponent<ZombieAttack>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashID>();

        nav.updateRotation = true;
        animSetup = new AnimatorSetup(anim, hash);

        anim.SetLayerWeight(1, 1f);

        deadZone *= Mathf.Deg2Rad;
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
        NavAnimSetup();
    }

    private void OnAnimatorMove()
    {
        nav.velocity = anim.deltaPosition / Time.deltaTime;
        transform.rotation = anim.rootRotation;
    }

    private void NavAnimSetup()
    {
        float speed;
        float angle;

        if (zombieAttack.playerInRange)
        {
            speed = 0f;
            angle = FindAngle(transform.forward, player.transform.position - transform.position, transform.up);
        }
        else
        {
            speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
            angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);

            if (Mathf.Abs(angle) < deadZone)
            {
                transform.LookAt(transform.position + nav.desiredVelocity);
                angle = 0f;
            }
        }

        animSetup.Setup(speed, angle);
    }

    private float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        if (toVector == Vector3.zero)
            return 0f;

        float angle = Vector3.Angle(fromVector, toVector);
        Vector3 normal = Vector3.Cross(fromVector, toVector);
        angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
        angle *= Mathf.Deg2Rad;

        return angle;
    }
}
