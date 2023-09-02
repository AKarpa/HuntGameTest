using System;
using UnityEngine;

namespace Hunt
{
    public class HuntCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        private Transform _transform;
        private Transform _followTarget;
        private Transform _lookTarget;
        private const float Damping = 10f;


        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        public void SetFollowTarget(Transform followTarget)
        {
            _followTarget = followTarget;
        }

        public void SetLookTarget(Transform lookTarget)
        {
            _lookTarget = lookTarget;
        }

        private void LateUpdate()
        {
            if (_followTarget != null)
            {
                _transform.position = Vector3.Lerp(_transform.position,
                    _followTarget.position + _followTarget.rotation * offset, Time.deltaTime * Damping);
            }

            if (_lookTarget != null)
            {
                Quaternion lookRotation = Quaternion.LookRotation(_lookTarget.position - _transform.position);
                _transform.rotation = Quaternion.Lerp(_transform.rotation, lookRotation, Time.deltaTime * Damping);
            }
        }
    }
}