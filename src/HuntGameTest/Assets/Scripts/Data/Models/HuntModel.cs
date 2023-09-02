using System;
using System.Collections.Generic;

namespace Data.Models
{
    [Serializable]
    public class HuntModel
    {
        public int level;
        public List<AnimalModel> huntingPack;
    }
}