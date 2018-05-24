using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashID : MonoBehaviour
{
    public static HashID instance = null;

    public int speedFloat;
    public int attackFloat;
    public int AngularSpeedFloat;
    public int playerInSightBool;
    public int playerInRangeBool;
    public int zombieIsHit;
    public int locomotionState;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        speedFloat = Animator.StringToHash("Speed");
        attackFloat = Animator.StringToHash("Attack");
        AngularSpeedFloat = Animator.StringToHash("AngularSpeed");
        playerInSightBool = Animator.StringToHash("PlayerInSight");
        playerInRangeBool = Animator.StringToHash("PlayerInRange");
        zombieIsHit = Animator.StringToHash("ZombieIsHit");
        locomotionState = Animator.StringToHash("Base Layer.Locomotion");
    }
}
