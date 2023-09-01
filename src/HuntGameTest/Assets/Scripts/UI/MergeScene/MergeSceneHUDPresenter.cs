using Balances;
using Data.DataProxies;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.MergeScene
{
    [RequireComponent(typeof(MergeSceneHUDView))]
    public class MergeSceneHUDPresenter : MonoBehaviour
    {
        private MergeSceneHUDView _view;
        private GoldDataProxy _goldDataProxy;
        private BuyAnimalBalance _buyAnimalBalance;
        private MergeGrid.MergeGrid _mergeGrid;

        [Inject]
        public void Construct(GoldDataProxy goldDataProxy, BuyAnimalBalance buyAnimalBalance, MergeGrid.MergeGrid mergeGrid)
        {
            _mergeGrid = mergeGrid;
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
                _mergeGrid.SpawnNewGridAnimal();
            };
            _view.ClickedPlayButton += delegate { };

            _goldDataProxy.Gold.Subscribe(delegate(int gold) { _view.SetGold(gold); }).AddTo(this);
            _view.SetBuyPrice(_buyAnimalBalance.BuyPrice);
        }
    }
}