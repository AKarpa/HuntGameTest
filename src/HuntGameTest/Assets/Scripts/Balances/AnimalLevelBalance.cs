using System;
using UnityEngine;

namespace Balances
{
    [Serializable]
    public class AnimalLevelBalance
    {
        [SerializeField] private int damage;
        
        public int Damage => damage;
    }
}