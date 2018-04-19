using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashID : MonoBehaviour
{
    public int speedFloat;
    public int attackFloat;
    public int AngularSpeedFloat;
    public int playerInSightBool;
    public int playerInRangeBool;
    public int locomotionState;

    private void Awake()
    {
        speedFloat = Animator.StringToHash("Speed");
        attackFloat = Animator.StringToHash("Attack");
        AngularSpeedFloat = Animator.StringToHash("AngularSpeed");
        playerInSightBool = Animator.StringToHash("PlayerInSight");
        playerInRangeBool = Animator.StringToHash("PlayerInRange");
        locomotionState = Animator.StringToHash("Base Layer.Locomotion");
    }
}
