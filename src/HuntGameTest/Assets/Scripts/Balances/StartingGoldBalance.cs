using System;
using UnityEngine;

namespace Balances
{
    [Serializable]
    public class StartingGoldBalance
    {
        [SerializeField] private int startingGold;
        
        public int StartingGold => startingGold;
    }
}