using UnityEngine;
using System.Collections;


public class SlowingMotor : MonoBehaviour
{
    public float force;
    [Range(0,1)] public float forceFallof = 1.0f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * force, ForceMode.Force);
        force *= forceFallof;
    }
}