using UnityEngine;
using System.Collections;

public class PullInto : MonoBehaviour
{
    public float forceMax = float.PositiveInfinity;
    public float force;
    public float sideForce;
    private void OnTriggerStay(Collider other)
    {
        if (!other.attachedRigidbody || other.isTrigger || other.gameObject.layer == LayerMask.NameToLayer("Debris"))
            return;

        Vector3 into = transform.position - other.attachedRigidbody.position;

        float f = Mathf.Clamp(into.magnitude, 0, forceMax);
        other.attachedRigidbody.AddForce(into.normalized * f * force + new Vector3(-into.z, into.y, into.x).normalized* f * sideForce );


    }
}