using Balances;
using Data.Models;
using UniRx;

namespace Data.DataProxies
{
    public class HuntDataProxy : IDataProxy
    {
        private readonly LevelBalances _levelBalances;
        private HuntModel _huntModel;
        private readonly ReactiveProperty<int> _level = new();

        public HuntDataProxy(LevelBalances levelBalances)
        {
            _levelBalances = levelBalances;
        }

        public IReadOnlyReactiveProperty<int> Level => _level;

        public void IncreaseLevel()
        {
            if (_level.Value == _levelBalances.MaxLevel)
            {
                _level.Value = 1;
                return;
            }

            _level.Value++;
        }

        void IDataProxy.SetGameState(GameStateModel gameStateModel)
        {
            _huntModel = gameStateModel.hunt;
            _level.Value = _huntModel.level;
            _level.Subscribe(delegate(int level) { _huntModel.level = level; });
        }
    }
}