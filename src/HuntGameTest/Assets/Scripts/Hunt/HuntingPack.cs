using System;
using System.Collections.Generic;
using System.Linq;
using Data.DataProxies;
using Hunt.HuntAnimal;
using Hunt.Prey;
using UniRx;
using UnityEngine;
using Zenject;

namespace Hunt
{
    public class HuntingPack : IInitializable
    {
        private readonly GridDataProxy _gridDataProxy;
        private readonly HuntAnimalFactory _factory;
        private readonly PreyPresenter _preyPresenter;
        private readonly HuntCamera _huntCamera;
        private readonly HuntInput _huntInput;
        private readonly List<HuntAnimalPresenter> _huntAnimals = new();
        private IDisposable _deltaPositionDisposable;
        private int _activeAnimalIndex;

        public HuntingPack(GridDataProxy gridDataProxy, HuntAnimalFactory factory, PreyPresenter preyPresenter,
            HuntCamera huntCamera, HuntInput huntInput)
        {
            _gridDataProxy = gridDataProxy;
            _factory = factory;
            _preyPresenter = preyPresenter;
            _huntCamera = huntCamera;
            _huntInput = huntInput;
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
            if (activeAnimal.CanJump(_huntInput.DeltaPosition.Value))
            {
                activeAnimal.Jump(_huntInput.DeltaPosition.Value);
            }
        }

        private void UpdateActiveAnimal()
        {
            HuntAnimalPresenter activeAnimal = _huntAnimals[_activeAnimalIndex];
            _huntCamera.SetFollowTarget(activeAnimal.transform);
        }
    }
}