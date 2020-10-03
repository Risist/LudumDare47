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
    Rigidbody _rb;

    bool pull = false;

    Timer tPull = new Timer();

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _rb = GetComponent<Rigidbody>();
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
            //if(tPull.IsReady())
            {
                var collider = GetComponent<Collider>();
                collider.enabled = true;
                _rb.isKinematic = false;
                _rb.AddForce(-force * pullForceScale);
            }
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
        if(!pull)
            tPull.Restart();
        pull = true;
        var collider = GetComponent<Collider>();
        var motor = GetComponent<Motor>();
        motor.enabled = false;
        //_rb.isKinematic = true;
        //collider.enabled = false;

    }
}
