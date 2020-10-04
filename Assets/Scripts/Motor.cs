using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    public float force;
    public float initialForce;
    [Range(0, 1)] public float forceFallof = 1.0f;
    [NonSerialized] public float _currentForce;
    Rigidbody rb;

    private void OnEnable()
    {
        _currentForce = force;
    }

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        rb.AddForce(transform.forward * initialForce, ForceMode.Force);
    }
    private void FixedUpdate()
    {
        _currentForce *= forceFallof;
        rb.AddForce(transform.forward * _currentForce, ForceMode.Force);
    }
}
