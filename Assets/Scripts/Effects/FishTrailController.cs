using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class FishTrailController : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _renderer;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _timeMultiplier;
        [SerializeField] private float _timeOffset;
        [SerializeField] private float _timeAccomodationSpeed;

        void Update()
        {
            _renderer.time = Mathf.Lerp(_renderer.time, _rigidbody.velocity.magnitude * _timeMultiplier + _timeOffset, _timeAccomodationSpeed*Time.deltaTime);
        }
    }
}
