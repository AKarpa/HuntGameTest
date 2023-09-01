using System;
using System.Collections.Generic;
using Data.Models.Grid;

namespace Data.Models
{
    [Serializable]
    public class GameStateModel
    {
        public int gold;
        public GridModel grid;

        public static GameStateModel CreateNewModel(int startingGold)
        {
            return new GameStateModel
            {
                gold = startingGold,
                grid = new GridModel
                {
                    gridElementModels = new List<GridElementModel>(),
                }
            };
        }
    }
}