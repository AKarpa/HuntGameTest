using System.Collections.Generic;
using System.Linq;
using Data.DataProxies;
using Hunt.HuntAnimal;
using Hunt.Prey;
using Zenject;

namespace Hunt
{
    public class HuntingPack : IInitializable
    {
        private readonly GridDataProxy _gridDataProxy;
        private readonly HuntAnimalFactory _factory;
        private readonly PreyPresenter _preyPresenter;
        private readonly HuntCamera _huntCamera;
        private readonly List<HuntAnimalPresenter> _huntAnimals = new();

        public HuntingPack(GridDataProxy gridDataProxy, HuntAnimalFactory factory, PreyPresenter preyPresenter,
            HuntCamera huntCamera)
        {
            _gridDataProxy = gridDataProxy;
            _factory = factory;
            _preyPresenter = preyPresenter;
            _huntCamera = huntCamera;
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
            _huntCamera.SetFollowTarget(_huntAnimals[0].transform);
            _huntCamera.SetLookTarget(_preyPresenter.transform);
        }
    }
}