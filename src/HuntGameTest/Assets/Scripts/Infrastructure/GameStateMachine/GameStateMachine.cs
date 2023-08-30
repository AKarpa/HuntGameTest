using Data;
using Scenes;
using StateMachines;

namespace Infrastructure.GameStateMachine
{
    public class GameStateMachine : StateMachine
    {
        public GameStateMachine(SceneLoader sceneLoader, DataController dataController, SaveSystem saveSystem)
        {
            AddNewState(new BootstrapState(this, sceneLoader, dataController, saveSystem));
            AddNewState(new LoadLevelState(this, sceneLoader));
            AddNewState(new GameLoopState());

            Enter<BootstrapState>();
        }
    }
}