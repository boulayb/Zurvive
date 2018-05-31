using System.Collections;
using UnityEngine;
using VRTK;

public class CrowbarGrab : VRTK_InteractableObject
{
    public int EnergyLostDivider = 30;

    private float impactMagnifier = 120f;
    private float collisionForce = 0f;
    private float maxCollisionForce = 4000f;
    private VRTK_ControllerReference controllerReference;
    private bool waiting = false;

    public float CollisionForce()
    {
        return collisionForce;
    }

    public override void Grabbed(VRTK_InteractGrab grabbingObject)
    {
        base.Grabbed(grabbingObject);
        controllerReference = VRTK_ControllerReference.GetControllerReference(grabbingObject.controllerEvents.gameObject);
    }

    public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        controllerReference = null;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        controllerReference = null;
        interactableRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (VRTK_ControllerReference.IsValid(controllerReference) && IsGrabbed())
        {
            collisionForce = VRTK_DeviceFinder.GetControllerVelocity(controllerReference).magnitude * impactMagnifier;
            var hapticStrength = collisionForce / maxCollisionForce;
            VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference, hapticStrength, 0.5f, 0.01f);

            if (waiting == false && (collision.gameObject.tag == Tags.body || collision.gameObject.tag == Tags.head))
            {
                waiting = true;
                EnergyManager.instance.LooseEnergy((int)(collisionForce / EnergyLostDivider));
                StartCoroutine(WaitTime(1));
            }
        }
        else
        {
            collisionForce = collision.relativeVelocity.magnitude * impactMagnifier;
        }
    }

    private IEnumerator WaitTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        waiting = false;
    }
}