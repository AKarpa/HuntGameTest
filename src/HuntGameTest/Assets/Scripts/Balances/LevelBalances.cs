using System;
using System.Collections.Generic;
using UnityEngine;

namespace Balances
{
    [Serializable]
    public class LevelBalances
    {
        [SerializeField] private List<LevelBalance> levelBalances;

        public int MaxLevel => levelBalances.Count;
        
        public LevelBalance GetLevelBalance(int level)
        {
            return levelBalances[level - 1];
        }
    }
}