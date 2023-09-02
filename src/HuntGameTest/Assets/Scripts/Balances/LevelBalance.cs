using System;
using UnityEngine;

namespace Balances
{
    [Serializable]
    public class LevelBalance
    {
        [SerializeField] private float preyHealth;
        
        public float PreyHealth => preyHealth;
    }
}