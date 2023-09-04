using System;
using Data.Models;
using UniRx;

namespace Data.DataProxies
{
    public class GoldDataProxy : IDataProxy
    {
        private readonly ReactiveProperty<int> _gold = new();

        public IReadOnlyReactiveProperty<int> Gold => _gold;

        public void SubtractGold(int amount)
        {
            if (amount > _gold.Value)
            {
                throw new InvalidOperationException();
            }

            _gold.Value -= amount;
        }

        public void AddGold(int amount)
        {
            _gold.Value += amount;
        }

        void IDataProxy.SetGameState(GameStateModel gameStateModel)
        {
            _gold.Value = gameStateModel.gold;

            _gold.Subscribe(delegate(int gold) { gameStateModel.gold = gold; });
        }
    }
}