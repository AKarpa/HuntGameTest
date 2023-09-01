using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Models.Grid;
using MergeGrid;
using UnityEngine;

namespace Data.DataProxies
{
    public class GridDataProxy : IDataProxy
    {
        private GridModel _grid;

        public IEnumerable<GridElementInfo> GridElementInfos => _grid.gridElementModels.Select(GetGridElementInfo);

        public void AddAnimal(Vector2Int coords, int level)
        {
            _grid.gridElementModels.Add(new GridElementModel
            {
                animal = new AnimalModel
                {
                    level = level
                },
                x = coords.x,
                y = coords.y
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

        public void MoveAnimal(Vector2Int prevCoords, Vector2Int coords)
        {
            GridElementModel gridElementModel =
                _grid.gridElementModels.Find(model => model.x == prevCoords.x && model.y == prevCoords.y);
            gridElementModel.x = coords.x;
            gridElementModel.y = coords.y;
        }

        public void MergeAnimals(Vector2Int droppedAnimalCoords, Vector2Int coords)
        {
            GridElementModel droppedAnimal =
                _grid.gridElementModels.Find(model =>
                    model.x == droppedAnimalCoords.x && model.y == droppedAnimalCoords.y);
            _grid.gridElementModels.Remove(droppedAnimal);
            GridElementModel mergedAnimal =
                _grid.gridElementModels.Find(model =>
                    model.x == coords.x && model.y == coords.y);
            mergedAnimal.animal.level++;
        }
    }
}