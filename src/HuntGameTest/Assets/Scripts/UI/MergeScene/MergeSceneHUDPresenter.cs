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
        private SceneLoader _sceneLoader;
        private HuntDataProxy _huntDataProxy;

        [Inject]
        public void Construct(GoldDataProxy goldDataProxy, BuyAnimalBalance buyAnimalBalance,
            Grid.MergeGrid mergeGrid, SceneLoader sceneLoader, HuntDataProxy huntDataProxy)
        {
            _huntDataProxy = huntDataProxy;
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
                if (_mergeGrid.IsFull) return;
                _goldDataProxy.SubtractGold(buyPrice);
                _mergeGrid.SpawnNewGridAnimal();
            };
            _view.ClickedPlayButton += delegate
            {
                _sceneLoader.LoadScene(SceneName.HuntScene);
            };

            _goldDataProxy.Gold.Subscribe(delegate(int gold) { _view.SetGold(gold); }).AddTo(this);
            _huntDataProxy.Level.Subscribe(delegate(int level) { _view.SetLevel(level); }).AddTo(this);
            _view.SetBuyPrice(_buyAnimalBalance.BuyPrice);
        }
    }
}