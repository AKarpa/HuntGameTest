using Data;
using Scenes;
using StateMachines;

namespace Infrastructure.GameStateMachine
{
    public class GameStateMachine : StateMachine
    {
        public GameStateMachine(SceneLoader sceneLoader, DataController dataController)
        {
            AddNewState(new BootstrapState(this, sceneLoader, dataController));
            AddNewState(new LoadLevelState(this, sceneLoader));
            AddNewState(new GameLoopState());

            Enter<BootstrapState>();
        }
    }
}