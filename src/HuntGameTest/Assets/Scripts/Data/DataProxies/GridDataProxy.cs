using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Models.Grid;
using MergeGrid;

namespace Data.DataProxies
{
    public class GridDataProxy : IDataProxy
    {
        private GridModel _grid;

        public IEnumerable<GridElementInfo> HuntingPack => _grid.huntingPack.Select(GetGridElementInfo);
        public IEnumerable<GridElementInfo> Other => _grid.other.Select(GetGridElementInfo);

        public void AddAnimal(int x, int y, int level, bool isHuntingPack)
        {
            List<GridElementModel> gridPart = isHuntingPack ? _grid.huntingPack : _grid.other;
            gridPart.Add(new GridElementModel
            {
                animal = new AnimalModel
                {
                    level = level
                },
                x = x,
                y = y
            });
        }

        void IDataProxy.SetGameState(GameStateModel gameStateModel)
        {
            _grid = gameStateModel.grid;
        }

        private static GridElementInfo GetGridElementInfo(GridElementModel model)
        {
            return new GridElementInfo(model.x, model.y, model.animal.level);
        }
    }
}