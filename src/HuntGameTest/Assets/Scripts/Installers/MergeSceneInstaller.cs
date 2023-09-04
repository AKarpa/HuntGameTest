using System;
using Grid.GridAnimals;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MergeSceneInstaller : MonoInstaller
    {
        [SerializeField] private GridAnimal gridAnimalPrefab;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Grid.MergeGrid>().FromComponentInHierarchy().AsSingle();
            Container.BindFactory<int, GridAnimal, GridAnimalFactory>()
                .FromPoolableMemoryPool<int, GridAnimal, GridAnimalPool>(x =>
                    x.WithInitialSize(5).FromComponentInNewPrefab(gridAnimalPrefab));
        }
        
        private class GridAnimalPool : MonoPoolableMemoryPool<int, IMemoryPool, GridAnimal>
        {
            
        }
    }
}