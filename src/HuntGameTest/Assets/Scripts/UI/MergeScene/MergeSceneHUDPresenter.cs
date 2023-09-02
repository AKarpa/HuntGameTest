using Balances;
using Data.DataProxies;
using Scenes;
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
        private Grid.MergeGrid _mergeGrid;
        private HuntDataProxy _huntDataProxy;
        private SceneLoader _sceneLoader;

        [Inject]
        public void Construct(GoldDataProxy goldDataProxy, BuyAnimalBalance buyAnimalBalance,
            Grid.MergeGrid mergeGrid, SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
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
            _view.ClickedPlayButton += delegate
            {
                _sceneLoader.LoadScene(SceneName.HuntScene);
            };

            _goldDataProxy.Gold.Subscribe(delegate(int gold) { _view.SetGold(gold); }).AddTo(this);
            _view.SetBuyPrice(_buyAnimalBalance.BuyPrice);
        }
    }
}