using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class TongueBullet : MonoBehaviour
{
    public float pullForceScale;
    public Transform tongueEnd;
    public Rigidbody parentRigidbody;
    LineRenderer _lineRenderer;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        _lineRenderer.SetPosition(0, transform.position);
        if (tongueEnd)
            _lineRenderer.SetPosition(1, tongueEnd.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 force = collision.collider.transform.position - tongueEnd.position;
        parentRigidbody.AddForceAtPosition(force * pullForceScale, tongueEnd.position);
    }
}
