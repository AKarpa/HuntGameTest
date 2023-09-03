using System;
using UnityEngine;
using Zenject;

namespace Hunt.HuntAnimal
{
    [RequireComponent(typeof(HuntAnimalView))]
    public class HuntAnimalPresenter : MonoBehaviour, IPoolable<HuntAnimalSpawnInfo, IMemoryPool>, IDisposable
    {
        private HuntAnimalView _view;
        private IMemoryPool _pool;
        private const float MaxJumpLength = 3f;
        
        public void SetJumpDirection(Vector2 jumpDirection)
        {
            _view.SetJumpAimActive(CanJump(jumpDirection));
            if (CanJump(jumpDirection))
            {
                _view.SetJumpAimPosition(MaxJumpLength * new Vector3(jumpDirection.x, 0f, jumpDirection.y));
            }
        }

        public bool CanJump(Vector2 jumpDirection)
        {
            return jumpDirection.y <= 0.1f;
        }

        public void Jump(Vector2 deltaPositionValue)
        {
            
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        void IPoolable<HuntAnimalSpawnInfo, IMemoryPool>.OnSpawned(HuntAnimalSpawnInfo spawnInfo, IMemoryPool pool)
        {
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
    }
}