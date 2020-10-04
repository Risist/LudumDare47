using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeColorOnDamage : MonoBehaviour
{
    public MeshRenderer[] renderers;
    [Range(0, 1)] public float animationSpeed;
    [Range(0, 1)] public float desiredStateDamping;
    Color[] defaultColors;

    Color DesiredColor = Color.red;

    float damageState;
    float desiredState;

    void Start()
    {
        defaultColors = new Color[renderers.Length];
        for(int i = 0; i < renderers.Length; ++i)
        {
            defaultColors[i] = renderers[i].material.color;
        }

        var healthController = GetComponentInParent<HealthController>();
        Debug.Assert(healthController);

        healthController.onDamageCallback += (DamageData data) =>
        {
            desiredState = 1.0f;
        };
    }

    private void Update()
    {
        for (int i = 0; i < renderers.Length; ++i)
        {
            renderers[i].material.color = Color.Lerp(defaultColors[i], DesiredColor, damageState);
        }
    }
    private void FixedUpdate()
    {
        damageState = Mathf.Lerp(desiredState, damageState, animationSpeed);
        desiredState *= desiredStateDamping;
    }
}
