using System;
using UnityEngine;
using Zenject;

namespace MergeGrid.GridAnimals
{
    public class GridAnimal : MonoBehaviour, IPoolable<int, IMemoryPool>, IDisposable
    {
        [SerializeField] private GameObject[] levelModels;
        private IMemoryPool _pool;

        public void SetLevel(int level)
        {
            for (int i = 0; i < levelModels.Length; i++)
            {
                levelModels[i].SetActive(i + 1 == level);
            }
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        void IPoolable<int, IMemoryPool>.OnSpawned(int level, IMemoryPool pool)
        {
            _pool = pool;
            SetLevel(level);
        }

        void IPoolable<int, IMemoryPool>.OnDespawned()
        {
            _pool = null;
        }
    }
}