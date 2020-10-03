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


    bool pull = false;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        _lineRenderer.SetPosition(0, transform.position);
        if (tongueEnd)
            _lineRenderer.SetPosition(1, tongueEnd.transform.position);

        if(pull)
        {
            Vector3 force = transform.position - parentRigidbody.position;
            parentRigidbody.AddForce(force * pullForceScale);
        }
    }

    Vector3 GetContactCenter(Collision collision)
    {
        Vector3 contactPosition = Vector3.zero;
        foreach (var it in collision.contacts)
            contactPosition += it.point;
        contactPosition /= collision.contactCount;
        return contactPosition;
    }
    private void OnCollisionEnter(Collision collision)
    {
        pull = true;
    }
}
