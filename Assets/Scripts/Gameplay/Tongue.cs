using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class Tongue : MonoBehaviour
{
    public GameObject tongueEnd;
    LineRenderer _lineRenderer;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        _lineRenderer.SetPosition(0, transform.position);
        if(tongueEnd)
            _lineRenderer.SetPosition(1, tongueEnd.transform.position);
    }
}