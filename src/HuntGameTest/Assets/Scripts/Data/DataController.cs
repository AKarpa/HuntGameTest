using System.Collections.Generic;
using Data.DataProxies;
using Data.Models;
using UniRx;

namespace Data
{
    public class DataController
    {
        private readonly List<IDataProxy> _dataProxies;
        private readonly SaveSystem _saveSystem;
        private GameStateModel _gameStateModel;

        public DataController(List<IDataProxy> dataProxies, SaveSystem saveSystem)
        {
            _dataProxies = dataProxies;
            _saveSystem = saveSystem;
        }

        public void Initialize()
        {
            _gameStateModel = _saveSystem.RetrieveGameState();
            
            Observable.EveryApplicationPause().Subscribe(delegate(bool b)
            {
                _saveSystem.SaveGameState(_gameStateModel);
            });
            
            foreach (IDataProxy dataProxy in _dataProxies)
            {
                dataProxy.SetGameState(_gameStateModel);
            }
        }
    }
}