using Balances;
using Data.DataProxies;
using UniRx;
using UnityEngine;
using Zenject;
using Grid = MergeGrid.Grid;

namespace UI.MergeScene
{
    [RequireComponent(typeof(MergeSceneHUDView))]
    public class MergeSceneHUDPresenter : MonoBehaviour
    {
        private MergeSceneHUDView _view;
        private GoldDataProxy _goldDataProxy;
        private BuyAnimalBalance _buyAnimalBalance;
        private Grid _grid;

        [Inject]
        public void Construct(GoldDataProxy goldDataProxy, BuyAnimalBalance buyAnimalBalance, Grid grid)
        {
            _grid = grid;
            _buyAnimalBalance = buyAnimalBalance;
            _goldDataProxy = goldDataProxy;
        }

        private void Awake()
        {
            _view = GetComponent<MergeSceneHUDView>();
            _view.ClickedBuyButton += delegate
            {
                int buyPrice = _buyAnimalBalance.BuyPrice;
                if (_goldDataProxy.Gold.Value < buyPrice) return;
                _goldDataProxy.SubtractGold(buyPrice);
                _grid.SpawnNewGridAnimal();
            };
            _view.ClickedPlayButton += delegate { };

            _goldDataProxy.Gold.Subscribe(delegate(int gold) { _view.SetGold(gold); }).AddTo(this);
        }
    }
}