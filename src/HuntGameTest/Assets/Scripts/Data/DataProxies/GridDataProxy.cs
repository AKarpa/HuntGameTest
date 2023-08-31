using Data.Models;
using Data.Models.Grid;

namespace Data.DataProxies
{
    public class GridDataProxy : IDataProxy
    {
        private GridModel _grid;

        void IDataProxy.SetGameState(GameStateModel gameStateModel)
        {
            _grid = gameStateModel.grid;
        }
    }
}