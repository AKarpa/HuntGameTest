using System;
using UnityEngine;

namespace Balances
{
    [Serializable]
    public class AnimalBalances
    {
        [SerializeField] private AnimalLevelBalance[] animalLevelBalances;

        public int MaxLevel => animalLevelBalances.Length;
        
        public AnimalLevelBalance GetLevelBalance(int level)
        {
            return animalLevelBalances[level - 1];
        }
    }
}