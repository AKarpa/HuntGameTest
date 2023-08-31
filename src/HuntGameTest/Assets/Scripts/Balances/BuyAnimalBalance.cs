using System;
using UnityEngine;

namespace Balances
{
    [Serializable]
    public class BuyAnimalBalance
    {
        [SerializeField] private int buyPrice;
        
        public int BuyPrice => buyPrice;
    }
}