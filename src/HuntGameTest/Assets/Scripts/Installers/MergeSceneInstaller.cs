using System;
using MergeGrid.GridAnimals;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MergeSceneInstaller : MonoInstaller
    {
        [SerializeField] private GridAnimal gridAnimalPrefab;
        
        public override void InstallBindings()
        {
            Container.BindFactory<int, GridAnimal, GridAnimalFactory>()
                .FromPoolableMemoryPool<int, GridAnimal, GridAnimalPool>(x =>
                    x.WithInitialSize(5).FromComponentInNewPrefab(gridAnimalPrefab));
        }
        
        private class GridAnimalPool : PoolableMemoryPool<int, IMemoryPool, GridAnimal>
        {
            
        }
    }
}