using System;
using System.Collections;
using PathCreation;
using UnityEngine;

namespace Hunt.Prey
{
    public class PreyView : MonoBehaviour
    {
        [SerializeField] private PathCreator pathCreator;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform[] followTransforms;
        private Transform _transform;
        private static readonly int Move = Animator.StringToHash("Move");
        private const float RunningSpeed = 2f;
        private const float Damping = 10f;
        
        public Transform[] FollowTransforms => followTransforms;

        public void StartRunning()
        {
            StartCoroutine(Running());
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private IEnumerator Running()
        {
            animator.SetBool(Move, true);
            float distanceTraveled = 0f;
            while (true)
            {
                distanceTraveled += RunningSpeed * Time.deltaTime;
                Vector3 newPosition = pathCreator.path.GetPointAtDistance(distanceTraveled);
                Quaternion lookRotation = Quaternion.LookRotation(newPosition - _transform.position);
                _transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * Damping);
                _transform.position = newPosition;
                yield return null;
            }
        }
    }
}