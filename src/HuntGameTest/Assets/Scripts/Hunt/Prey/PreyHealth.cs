using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Hunt.Prey
{
    public class PreyHealth : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI text;
        private Transform _huntCameraTransform;
        private Transform _transform;

        [Inject]
        public void Construct(HuntCamera huntCamera)
        {
            _huntCameraTransform = huntCamera.transform;
        }

        public void SetHealth(int health, int maxHealth)
        {
            slider.value = (float)health / maxHealth;
            text.text = $"{health}/{maxHealth}";
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            _transform.rotation = Quaternion.LookRotation(_transform.position - _huntCameraTransform.position);
        }
    }
}
