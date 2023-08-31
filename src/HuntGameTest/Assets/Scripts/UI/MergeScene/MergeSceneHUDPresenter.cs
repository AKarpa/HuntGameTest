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
        private GridDataProxy _gridDataProxy;
        private BuyAnimalBalance _buyAnimalBalance;

        [Inject]
        public void Construct(GoldDataProxy goldDataProxy, GridDataProxy gridDataProxy, BuyAnimalBalance buyAnimalBalance)
        {
            _buyAnimalBalance = buyAnimalBalance;
            _gridDataProxy = gridDataProxy;
            _goldDataProxy = goldDataProxy;
        }
        
        private void Awake()
        {
            _view = GetComponent<MergeSceneHUDView>();
            _view.ClickedBuyButton += delegate
            {
                
            };
            _view.ClickedPlayButton += delegate
            {
                
            };

            _goldDataProxy.Gold.Subscribe(delegate(int gold)
            {
                _view.SetGold(gold);
            }).AddTo(this);
        }
    }
}