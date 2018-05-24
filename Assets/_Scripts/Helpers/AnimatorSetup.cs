using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetup
{
    public float speedDampTime = 0.1f;
    public float angularSpeedDampTime = 0.7f;
    public float angleResponseTime = 0.6f;

    private Animator anim;

    public AnimatorSetup(Animator animator)
    {
        anim = animator;
    }

    public void Setup(float speed, float angle)
    {
        float angularSpeed = angle / angleResponseTime;

        anim.SetFloat(HashID.instance.speedFloat, speed, speedDampTime, Time.deltaTime);
        anim.SetFloat(HashID.instance.AngularSpeedFloat, angularSpeed, angularSpeedDampTime, Time.deltaTime);
    }
}

