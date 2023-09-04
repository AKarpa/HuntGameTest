using System;
using Balances;
using Data.DataProxies;
using Scenes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Hunt.Prey
{
    [RequireComponent(typeof(PreyView))]
    public class PreyPresenter : MonoBehaviour
    {
        private PreyView _view;
        private readonly ReactiveProperty<int> _health = new();
        private LevelBalance _levelBalance;
        private GoldDataProxy _goldDataProxy;
        private SceneLoader _sceneLoader;
        private HuntDataProxy _huntDataProxy;
        public event Action Died;

        [Inject]
        public void Construct(LevelBalances levelBalances, HuntDataProxy huntDataProxy, GoldDataProxy goldDataProxy, SceneLoader sceneLoader)
        {
            _huntDataProxy = huntDataProxy;
            _sceneLoader = sceneLoader;
            _goldDataProxy = goldDataProxy;
            _levelBalance = levelBalances.GetLevelBalance(huntDataProxy.Level.Value);
            _health.Value = _levelBalance.PreyHealth;
        }
        
        public Transform[] FollowPositions => _view.FollowTransforms;

        public void ReceiveDamage(int damage)
        {
            if (damage >= _health.Value)
            {
                _health.Value = 0;
                _goldDataProxy.AddGold(_levelBalance.Reward);
                _view.StopRunning();
                Died?.Invoke();
                Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(delegate(long l)
                {   
                    _huntDataProxy.IncreaseLevel();
                    _sceneLoader.LoadScene(SceneName.MergeScene);
                }).AddTo(this);
                return;
            }

            _health.Value -= damage;
        }

        private void Awake()
        {
            _view = GetComponent<PreyView>();
            _health.Subscribe(delegate(int f)
            {
                _view.SetHealth(f, _levelBalance.PreyHealth);
            }).AddTo(this);
        }

        private void Start()
        {
            _view.StartRunning();
        }
    }
}