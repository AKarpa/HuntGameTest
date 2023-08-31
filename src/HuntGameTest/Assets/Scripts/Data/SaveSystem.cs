using System.IO;
using Balances;
using Data.Models;
using UnityEngine;

namespace Data
{
    public class SaveSystem
    {
        private readonly StartingGoldBalance _startingGoldBalance;
        private readonly string _savePath;

        public SaveSystem(StartingGoldBalance startingGoldBalance)
        {
            _startingGoldBalance = startingGoldBalance;
            _savePath = Application.persistentDataPath + "/game_save.txt";
        }

        public void SaveGameState(GameStateModel gameStateModel)
        {
            string json = JsonUtility.ToJson(gameStateModel);
            File.WriteAllText(_savePath, json);
        }

        public GameStateModel RetrieveGameState()
        {
            if (!File.Exists(_savePath))
            {
                FileStream fileStream = File.Create(_savePath);
                fileStream.Close();
            }
            
            string json = File.ReadAllText(_savePath);
            return string.IsNullOrEmpty(json)
                ? GameStateModel.CreateNewModel(_startingGoldBalance.StartingGold)
                : JsonUtility.FromJson<GameStateModel>(json);
        }
    }
}