using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MaterialFader : MonoBehaviour
{
    public float fadeRatio;

    new MeshRenderer renderer;
    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        Debug.Assert(renderer);
        a = renderer.material.color.a;
    }

    float a;

    private void FixedUpdate()
    {
        Color cl = renderer.material.color;
        a *= fadeRatio;
        cl.a = a;
        renderer.material.color = cl;
    }
}

public class DeathEventPhysicsDestruction : MonoBehaviour
{
    public float explosionForce;
    public float explosionRadius;
    public float bodyDrag;
    public float particleAliveTime = 0.5f;
    public float fadeRatio;
    public bool preSelectMaterials = true;

    MeshRenderer[] meshes;

    private void Awake()
    {
        if (preSelectMaterials)
            meshes = GetComponentsInChildren<MeshRenderer>();
    }

    void Start()
    {
        var health = GetComponentInParent<HealthController>();
        health.onDeathCallback += DeathEvent;
    }

    private void OnDestroy()
    {
        var health = GetComponentInParent<HealthController>();
        if (health)
            health.onDeathCallback -= DeathEvent;
    }


    void DeathEvent(DamageData data)
    {
        if (meshes == null)
            meshes = GetComponentsInChildren<MeshRenderer>();

        int n = meshes.Length;
        for (int i = 0; i < n; ++i)
        {
            var obj = meshes[i].transform;
            obj.parent = null;

            var fader = obj.gameObject.AddComponent<MaterialFader>();
            fader.fadeRatio = fadeRatio;

            var body = obj.gameObject.AddComponent<Rigidbody>();
            if (!body)
                body = obj.GetComponent<Rigidbody>();

            body.drag = bodyDrag;
            body.AddExplosionForce(explosionForce, data.position, explosionRadius);

            Destroy(obj.gameObject, particleAliveTime);
        }

        Destroy(gameObject);
    }

}
