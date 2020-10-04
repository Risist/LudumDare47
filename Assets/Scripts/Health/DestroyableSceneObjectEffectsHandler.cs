using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Health
{
    [RequireComponent(typeof(HealthController))]
    public class DestroyableSceneObjectEffectsHandler : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _collisionParticlesPrefab;
        [SerializeField] private ParticleSystem _destructionParticlesPrefab;
        private ParticleSystem _damageSystem;

        void Start()
        {
            var healthController = this.GetComponentNotNull<HealthController>();
            healthController.onDamageCallback += (damageData) =>
            {
                CreateDamageEffect(damageData.position, damageData.direction);
            };

            healthController.onDeathCallback += (damageData) => { CreateDeathEffect(damageData.position); };
        }

        private void CreateDeathEffect(Vector3 damagePosition)
        {
            var destroySystem = GameObject.Instantiate(_destructionParticlesPrefab, damagePosition, transform.rotation);
            destroySystem.transform.localScale *= 0.66f;
        }

        private void CreateDamageEffect(Vector3 damagePosition, Vector3 damageDirection)
        {
            if (_damageSystem == null)
            {
                _damageSystem = GameObject.Instantiate(_collisionParticlesPrefab, damagePosition,
                    Quaternion.LookRotation(damageDirection));

                _damageSystem.transform.localScale = new Vector3(
                    _damageSystem.transform.localScale.x * transform.localScale.x,
                    _damageSystem.transform.localScale.y * transform.localScale.y,
                    _damageSystem.transform.localScale.z * transform.localScale.z);
            }
        }
    }
}
