using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(LineRenderer))]
public class TongueBullet : MonoBehaviour
{
    public float pullForceScale;
    public float tonguePullForceScale;
    public Transform tongueEnd;
    public Transform tongueStart;
    public Rigidbody parentRigidbody;
    LineRenderer _lineRenderer;
    Rigidbody _rb;
    Motor _motor;

    bool pull = false;

    MinimalTimer tExecute = new MinimalTimer();
    const float backTime = 0.25f;
    const float destroyDistance = 1.25f;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _rb = GetComponent<Rigidbody>();
        _motor = GetComponent<Motor>();
        tExecute.Restart();
        _motor._currentForce += _rb.velocity.magnitude;
    }

    void Update()
    {
        if (tongueEnd)
            _lineRenderer.SetPosition(1, tongueEnd.transform.position);
        if(tongueStart)
            _lineRenderer.SetPosition(0, tongueStart.transform.position);

        Vector3 force = transform.position - parentRigidbody.position;
        if (tExecute.IsReady(backTime))
        {
            //var collider = GetComponent<Collider>();
            //collider.enabled = true;
            _motor.enabled = false;
            _rb.isKinematic = true;
            transform.parent = tongueEnd;

            transform.localPosition = Vector3.Lerp(tongueEnd.position, transform.position, 0.99f);
            if(force.magnitude < destroyDistance)
            {
                Destroy(gameObject);
            }
        }
    }
    void FixedUpdate()
    {
        Vector3 force = transform.position - parentRigidbody.position;
        if (pull)
        {
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
    private void OnTriggerStay(Collider collision)
    {
        if (collision.attachedRigidbody == _rb || collision.attachedRigidbody == parentRigidbody)
            return;
        pull = true;
        _motor.enabled = false;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
        //collider.enabled = false;

    }
}
