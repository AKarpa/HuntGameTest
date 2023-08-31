using Data;
using Data.DataProxies;
using Infrastructure.GameStateMachine;
using Scenes;
using Zenject;

namespace Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DataController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SaveSystem>().AsSingle().NonLazy();

            BindDataProxies();
        }

        private void BindDataProxies()
        {
            Container.BindInterfacesAndSelfTo<GoldDataProxy>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GridDataProxy>().AsSingle().NonLazy();
        }
    }
}