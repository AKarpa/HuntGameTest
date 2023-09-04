using UnityEngine;
using Zenject;

namespace Balances
{
    [CreateAssetMenu(fileName = "GameBalancesSO", menuName = "Installers/GameBalancesSO")]
    public class GameBalancesSO : ScriptableObjectInstaller<GameBalancesSO>
    {
        [SerializeField] private StartingGoldBalance startingGoldBalance;
        [SerializeField] private BuyAnimalBalance buyAnimalBalance;
        [SerializeField] private LevelBalances levelBalances;
        [SerializeField] private AnimalBalances animalBalances;
        
        public override void InstallBindings()
        {
            Container.BindInstance(startingGoldBalance);
            Container.BindInstance(buyAnimalBalance);
            Container.BindInstance(levelBalances);
            Container.BindInstance(animalBalances);
        }
    }
}