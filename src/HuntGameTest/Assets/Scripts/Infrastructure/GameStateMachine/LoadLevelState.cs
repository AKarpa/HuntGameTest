using Scenes;
using StateMachines;

namespace Infrastructure.GameStateMachine
{
    public class LoadLevelState : IPayloadedState<SceneName>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(SceneName levelName)
        {
            _sceneLoader.LoadScene(levelName, delegate { _gameStateMachine.Enter<GameLoopState>(); });
        }

        public void Exit()
        {
        }
    }
}