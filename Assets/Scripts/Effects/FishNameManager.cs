using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    class FishNameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private string _fishName;

        void Start()
        {
            _fishName = $"fish{RandomGeneratorSingleton.GetRandom().Next(99999):00000}";
            _text.text = _fishName;
        }

        void Update()
        {
            transform.rotation = Quaternion.LookRotation((transform.position - Camera.main.transform.position).normalized);
        }

        public string FishName => _fishName;
    }
}
