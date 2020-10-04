using UnityEngine;
using System.Collections;

public class PullInto : MonoBehaviour
{
    public float force;
    public float sideForce;
    private void OnTriggerStay(Collider other)
    {
        if (!other.attachedRigidbody || other.isTrigger)
            return;

        Vector3 into = transform.position - other.attachedRigidbody.position;
        other.attachedRigidbody.AddForce(into * force + new Vector3(-into.z, into.y, into.x)*sideForce );
        }
}