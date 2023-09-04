using System;
using System.Collections;
using DG.Tweening;
using Hunt.Prey;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Hunt.HuntAnimal
{
    public class HuntAnimalView : MonoBehaviour
    {
        [SerializeField] private Animator[] models;
        [SerializeField] private Transform jumpAim;
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Win = Animator.StringToHash("Win");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private Animator _modelAnimator;
        private Transform _transform;
        private Coroutine _followCoroutine;
        private Sequence _sequence;
        private IDisposable _jumpCollisionDisposable;
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
            _followCoroutine = StartCoroutine(Follow(followTransform));
        }

        public void SetJumpAimActive(bool active)
        {
            jumpAim.gameObject.SetActive(active);
        }

        public void SetJumpAimPosition(Vector3 jumpAimPosition)
        {
            jumpAim.localPosition = jumpAimPosition;
        }

        public void Jump(Vector3 jumpDirection, Action onJumpEnded, Action<PreyPresenter> onCollidedWithPrey,
            Action dispose)
        {
            StopCoroutine(_followCoroutine);
            _followCoroutine = null;
            jumpAim.SetParent(_transform.parent);
            _modelAnimator.SetBool(Move, false);
            _modelAnimator.SetBool(Win, true);
            _jumpCollisionDisposable = gameObject.OnCollisionEnterAsObservable().Subscribe(delegate(Collision collision)
            {
                if (!collision.gameObject.TryGetComponent(out PreyPresenter preyPresenter)) return;
                _jumpCollisionDisposable?.Dispose();
                onCollidedWithPrey?.Invoke(preyPresenter);
                Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(delegate(long l) { dispose?.Invoke(); });
            });
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_transform.DOJump(_transform.position + _transform.rotation * jumpDirection, 2f, 1, 1f))
                .AppendCallback(
                    delegate
                    {
                        _jumpCollisionDisposable?.Dispose();
                        jumpAim.SetParent(_transform);
                        SetJumpAimActive(false);
                        _modelAnimator.SetBool(Win, false);
                        _modelAnimator.SetBool(Idle, true);
                        onJumpEnded?.Invoke();
                    }).AppendInterval(1f).AppendCallback(delegate { dispose?.Invoke(); });
        }

        public void StopFollow()
        {
            if (_followCoroutine != null)
                StopCoroutine(_followCoroutine);
            _modelAnimator.SetBool(Move, false);
            _modelAnimator.SetBool(Idle, true);
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            SetJumpAimActive(false);
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }

        private void OnDisable()
        {
            _sequence.Kill();
        }

        private IEnumerator Follow(Transform followTransform)
        {
            _modelAnimator.SetBool(Move, true);
            while (true)
            {
                Vector3 followPosition = followTransform.position;
                Vector3 currentPosition = _transform.position;
                Vector3 direction = followPosition - currentPosition;
                if (direction.magnitude > 0.1f)
                {
                    Vector3 newPosition = currentPosition +
                                          Mathf.Min(RunningSpeed * Time.deltaTime, direction.magnitude) *
                                          direction.normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(newPosition - currentPosition);
                    _transform.rotation = Quaternion.Lerp(_transform.rotation, lookRotation, Time.deltaTime * Damping);
                    _transform.position = newPosition;
                }

                yield return null;
            }
        }
    }
}