using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class BlackHoleRayController : MonoBehaviour
    {
        [SerializeField] private GameObject _blackHole;
        [SerializeField] private GameObject _fish;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private ParticleSystemRenderer _particleRenderer;
        [SerializeField, Range(0,100)] private float _zm;
        [SerializeField] private Material _particleMaterial;

        void Awake()
        {
            _particleMaterial = _particleRenderer.material;
        }

        void Update()
        {
            var startPos = _fish.transform.position;
            transform.position = startPos;
            var delta = (_blackHole.transform.position -startPos);
            transform.rotation = Quaternion.LookRotation( delta.normalized);

            var vof = _particle.velocityOverLifetime;
            vof.zMultiplier = _zm*delta.magnitude;

            var mm = _particle.main;
            var ffactor = Mathf.InverseLerp(20, 0, delta.magnitude) + 0.2f;
            mm.simulationSpeed = ffactor;

            _particleMaterial.SetFloat("_InvFade", Mathf.Clamp01(ffactor));
        }

    }
}
