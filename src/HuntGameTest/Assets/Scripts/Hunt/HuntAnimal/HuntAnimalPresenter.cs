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