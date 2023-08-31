using System;
using System.Collections.Generic;

namespace Data.Models.Grid
{
    [Serializable]
    public class GridModel
    {
        public List<GridElementModel> huntingPack;
        public List<GridElementModel> other;
    }
}