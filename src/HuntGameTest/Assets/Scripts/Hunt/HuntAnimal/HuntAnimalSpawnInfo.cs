using UnityEngine;

namespace Hunt.HuntAnimal
{
    public readonly struct HuntAnimalSpawnInfo
    {
        public HuntAnimalSpawnInfo(int level, Transform followTransform)
        {
            Level = level;
            FollowTransform = followTransform;
        }
        
        public int Level { get; }
        public Transform FollowTransform { get; }
    }
}