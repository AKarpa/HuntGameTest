using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class GameSaveCleaner : EditorUtility
    {
        [MenuItem("Tools/Clear game save")]
        public static void ClearGameSave()
        {
            File.Delete(Application.persistentDataPath + "/game_save.txt");
        }
    }
}