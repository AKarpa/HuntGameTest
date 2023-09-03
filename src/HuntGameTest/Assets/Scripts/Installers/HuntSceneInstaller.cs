using Hunt;
using Hunt.HuntAnimal;
using Hunt.Prey;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class HuntSceneInstaller : MonoInstaller
    {
        [SerializeField] private HuntAnimalPresenter huntAnimalPrefab;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<HuntingPack>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PreyPresenter>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<HuntCamera>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<HuntInput>().FromComponentInHierarchy().AsSingle();
            Container.BindFactory<HuntAnimalSpawnInfo, HuntAnimalPresenter, HuntAnimalFactory>()
                .FromPoolableMemoryPool<HuntAnimalSpawnInfo, HuntAnimalPresenter, HuntAnimalPool>(x =>
                    x.WithInitialSize(5).FromComponentInNewPrefab(huntAnimalPrefab));
        }
        
        private class HuntAnimalPool : MonoPoolableMemoryPool<HuntAnimalSpawnInfo, IMemoryPool, HuntAnimalPresenter>
        {
            
        }
    }
}