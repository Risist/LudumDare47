using UnityEngine;
using System.Collections;

public class PullInto : MonoBehaviour
{
    public float forceMax = float.PositiveInfinity;
    public float forceMin = 0;
    public float force;
    public float sideForce;

    [Space]
    public float forceIncreaseRate;
    public float forceSideIncreaseRate;
    public bool increaseForce;

    private void OnTriggerStay(Collider other)
    {
        if (!other.attachedRigidbody || other.isTrigger || other.gameObject.layer == LayerMask.NameToLayer("Debris"))
            return;

        Vector3 into = transform.position - other.attachedRigidbody.position;

        float f = Mathf.Clamp(into.magnitude, forceMin, forceMax);
        other.attachedRigidbody.AddForce(into.normalized * f * force + new Vector3(-into.z, into.y, into.x).normalized* f * sideForce );


    }
    private void FixedUpdate()
    {
        if (increaseForce)
        {
            force += forceIncreaseRate;
            sideForce += forceSideIncreaseRate;
        }
    }
}