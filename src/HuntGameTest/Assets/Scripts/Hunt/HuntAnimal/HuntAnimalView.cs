using System.Collections;
using UnityEngine;

namespace Hunt.HuntAnimal
{
    public class HuntAnimalView : MonoBehaviour
    {
        [SerializeField] private Animator[] models;
        private Animator _modelAnimator;
        private Transform _transform;
        private static readonly int Move = Animator.StringToHash("Move");
        private const float RunningSpeed = 3f;
        private const float Damping = 10f;

        public void SetLevel(int level)
        {
            for (int i = 0; i < models.Length; i++)
            {
                bool isActive = i + 1 == level;
                models[i].gameObject.SetActive(isActive);
                if (isActive)
                {
                    _modelAnimator = models[i];
                }
            }
        }

        public void StartFollowing(Transform followTransform)
        {
            _transform.position = followTransform.position;
            _transform.rotation = followTransform.rotation;
            StartCoroutine(Follow(followTransform));
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private IEnumerator Follow(Transform followTransform)
        {
            _modelAnimator.SetBool(Move, true);
            while (true)
            {
                Vector3 followPosition = followTransform.position;
                Vector3 currentPosition = _transform.position;
                Vector3 direction = followPosition - currentPosition;
                Vector3 newPosition = currentPosition + Mathf.Min(RunningSpeed * Time.deltaTime, direction.magnitude) * direction.normalized;
                Quaternion lookRotation = Quaternion.LookRotation(newPosition - currentPosition);
                _transform.rotation = Quaternion.Lerp(_transform.rotation, lookRotation, Time.deltaTime * Damping);
                _transform.position = newPosition;
                yield return null;
            }
        }
    }
}