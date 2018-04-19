using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 lastPosition;
    private float speed;

    private void Awake()
    {
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        speed = Vector3.Distance(lastPosition, transform.position) / Time.deltaTime;
        lastPosition = transform.position;
    }

    public float getSpeed()
    {
        return speed;
    }
}
