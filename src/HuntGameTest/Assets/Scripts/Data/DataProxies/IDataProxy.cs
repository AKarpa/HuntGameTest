using Data.Models;

namespace Data.DataProxies
{
    public interface IDataProxy
    {
        void SetGameState(GameStateModel gameStateModel);
    }
}