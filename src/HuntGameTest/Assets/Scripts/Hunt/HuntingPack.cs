using System;
using System.Collections.Generic;
using System.Linq;
using Data.DataProxies;
using Hunt.HuntAnimal;
using Hunt.Prey;
using Scenes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Hunt
{
    public class HuntingPack : IInitializable, IDisposable
    {
        private readonly GridDataProxy _gridDataProxy;
        private readonly HuntAnimalFactory _factory;
        private readonly PreyPresenter _preyPresenter;
        private readonly HuntCamera _huntCamera;
        private readonly HuntInput _huntInput;
        private readonly SceneLoader _sceneLoader;
        private readonly List<HuntAnimalPresenter> _huntAnimals = new();
        private readonly CompositeDisposable _compositeDisposable = new();
        private IDisposable _deltaPositionDisposable;
        private int _activeAnimalIndex;

        public HuntingPack(GridDataProxy gridDataProxy, HuntAnimalFactory factory, PreyPresenter preyPresenter,
            HuntCamera huntCamera, HuntInput huntInput, SceneLoader sceneLoader)
        {
            _gridDataProxy = gridDataProxy;
            _factory = factory;
            _preyPresenter = preyPresenter;
            _huntCamera = huntCamera;
            _huntInput = huntInput;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            int[] huntingPack = _gridDataProxy.HuntingPack.ToArray();
            for (int i = 0; i < huntingPack.Length; i++)
            {
                int level = huntingPack[i];
                HuntAnimalPresenter huntAnimal =
                    _factory.Create(new HuntAnimalSpawnInfo(level, _preyPresenter.FollowPositions[i]));
                _huntAnimals.Add(huntAnimal);
            }
            _huntCamera.SetLookTarget(_preyPresenter.transform);
            _activeAnimalIndex = 0;
            UpdateActiveAnimal();
            
            _huntInput.PointerDown += InputOnPointerDown;
            _huntInput.PointerUp += InputOnPointerUp;
            _preyPresenter.Died += OnPreyDied;
        }

        private void OnPreyDied()
        {
            foreach (HuntAnimalPresenter huntAnimal in _huntAnimals)
            {
                huntAnimal.StopFollow();
            }
        }

        private void InputOnPointerDown()
        {
            _deltaPositionDisposable = _huntInput.DeltaPosition.Subscribe(delegate(Vector2 jumpDirection)
            {
                HuntAnimalPresenter activeAnimal = _huntAnimals[_activeAnimalIndex];
                activeAnimal.SetJumpDirection(jumpDirection);
            });
        }

        private void InputOnPointerUp()
        {
            _deltaPositionDisposable?.Dispose();
            HuntAnimalPresenter activeAnimal = _huntAnimals[_activeAnimalIndex];
            if (HuntAnimalPresenter.CanJump(_huntInput.DeltaPosition.Value))
            {
                activeAnimal.Jump(_huntInput.DeltaPosition.Value, delegate
                {
                    if (_activeAnimalIndex + 1 == _huntAnimals.Count)
                    {
                        Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(delegate(long l)
                        {
                            _sceneLoader.LoadScene(SceneName.MergeScene);
                        }).AddTo(_compositeDisposable);
                        return;
                    }
                    _activeAnimalIndex++;
                    UpdateActiveAnimal();
                });
            }
        }

        private void UpdateActiveAnimal()
        {
            HuntAnimalPresenter activeAnimal = _huntAnimals[_activeAnimalIndex];
            _huntCamera.SetFollowTarget(activeAnimal.transform);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _deltaPositionDisposable?.Dispose();
            _huntInput.PointerDown -= InputOnPointerDown;
            _huntInput.PointerUp -= InputOnPointerUp;
        }
    }
}