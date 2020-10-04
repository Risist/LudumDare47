using UnityEngine;
using System.Collections;


[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class LineRendererConnector : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public Material material;
    LineRenderer _lineRenderer;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        _lineRenderer.material = material;
        _lineRenderer.SetPosition(0, start.transform.position);
        _lineRenderer.SetPosition(1, end.transform.position);
    }
}
