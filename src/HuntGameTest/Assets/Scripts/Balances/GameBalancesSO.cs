using UnityEngine;
using Zenject;

namespace Balances
{
    [CreateAssetMenu(fileName = "GameBalancesSO", menuName = "Installers/GameBalancesSO")]
    public class GameBalancesSO : ScriptableObjectInstaller<GameBalancesSO>
    {
        [SerializeField] private StartingGoldBalance startingGoldBalance;
        [SerializeField] private BuyAnimalBalance buyAnimalBalance;
        
        public override void InstallBindings()
        {
            Container.BindInstance(startingGoldBalance);
            Container.BindInstance(buyAnimalBalance);
        }
    }
}