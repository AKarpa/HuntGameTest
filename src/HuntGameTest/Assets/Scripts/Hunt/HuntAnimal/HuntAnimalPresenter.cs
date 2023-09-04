using System;
using Balances;
using Hunt.Prey;
using UnityEngine;
using Zenject;

namespace Hunt.HuntAnimal
{
    [RequireComponent(typeof(HuntAnimalView))]
    public class HuntAnimalPresenter : MonoBehaviour, IPoolable<HuntAnimalSpawnInfo, IMemoryPool>, IDisposable
    {
        private HuntAnimalView _view;
        private IMemoryPool _pool;
        private HuntAnimalSpawnInfo _spawnInfo;
        private AnimalBalances _animalBalances;
        private const float MaxJumpLength = 9f;
        
        [Inject]
        public void Construct(AnimalBalances animalBalances)
        {
            _animalBalances = animalBalances;
        }
        
        public void SetJumpDirection(Vector2 jumpScreenDirection)
        {
            _view.SetJumpAimActive(CanJump(jumpScreenDirection));
            if (CanJump(jumpScreenDirection))
            {
                _view.SetJumpAimPosition(GetJumpDirection(jumpScreenDirection));
            }
        }

        public static bool CanJump(Vector2 jumpScreenDirection)
        {
            return jumpScreenDirection.y <= -0.1f;
        }

        public void Jump(Vector2 jumpScreenDirection, Action onJumpEnded)
        {
            _view.Jump(GetJumpDirection(jumpScreenDirection), onJumpEnded, delegate(PreyPresenter presenter)
            {
                int damage = _animalBalances.GetLevelBalance(_spawnInfo.Level).Damage;
                presenter.ReceiveDamage(damage);
            }, Dispose);
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        void IPoolable<HuntAnimalSpawnInfo, IMemoryPool>.OnSpawned(HuntAnimalSpawnInfo spawnInfo, IMemoryPool pool)
        {
            _spawnInfo = spawnInfo;
            _pool = pool;
            _view.SetLevel(spawnInfo.Level);
            _view.StartFollowing(spawnInfo.FollowTransform);
        }

        void IPoolable<HuntAnimalSpawnInfo, IMemoryPool>.OnDespawned()
        {
            _pool = null;
        }

        private void Awake()
        {
            _view = GetComponent<HuntAnimalView>();
        }
        
        private static Vector3 GetJumpDirection(Vector2 jumpScreenDirection)
        {
            return -MaxJumpLength * new Vector3(jumpScreenDirection.x, 0f, jumpScreenDirection.y);
        }

        public void StopFollow()
        {
            _view.StopFollow();
        }
    }
}