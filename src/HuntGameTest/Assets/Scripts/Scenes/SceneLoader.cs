using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SceneLoader
    {
        public void LoadScene(SceneName sceneName, Action callback = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName.ToString())
            {
                callback?.Invoke();
                return;
            }

            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(sceneName.ToString());
            loadSceneAsync.completed += delegate(AsyncOperation operation)
            {
                callback?.Invoke();
            };
        }
    }
}