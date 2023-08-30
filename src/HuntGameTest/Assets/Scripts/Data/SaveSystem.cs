using System.IO;
using Data.Models;
using UnityEngine;

namespace Data
{
    public class SaveSystem
    {
        private readonly string _savePath;

        public SaveSystem()
        {
            _savePath = Application.persistentDataPath + "/game_save.txt";
        }

        public void SaveGameState(GameStateModel gameStateModel)
        {
            string json = JsonUtility.ToJson(gameStateModel);
            
            StreamWriter writer = new(_savePath, true);
            writer.WriteLine(json);
            writer.Close();
        }

        public GameStateModel RetrieveGameState()
        {
            string path = Application.persistentDataPath + "/test.txt";
            StreamReader reader = new(path);
            string json = reader.ReadToEnd();
            reader.Close();
            return JsonUtility.FromJson<GameStateModel>(json);
        }
    }
}