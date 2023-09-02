using System.Collections.Generic;
using System.Linq;
using Data.Models;
using UniRx;

namespace Data.DataProxies
{
    public class HuntDataProxy : IDataProxy
    {
        private HuntModel _huntModel;
        private readonly ReactiveProperty<int> _level = new();

        public IReadOnlyReactiveProperty<int> Level => _level;

        public void IncreaseLevel()
        {
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