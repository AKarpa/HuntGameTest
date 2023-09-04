using System;
using UnityEngine;

namespace Balances
{
    [Serializable]
    public class LevelBalance
    {
        [SerializeField] private int preyHealth;
        [SerializeField] private int reward;
        
        public int PreyHealth => preyHealth;
        public int Reward => reward;
    }
}