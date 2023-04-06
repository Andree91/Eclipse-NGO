using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance { get; set;}

        [SerializeField] private int mainMenuSceneIndex = 0;
        [SerializeField] private int worldSceneIndex = 1;

        public int GetMainMenuSceneIndex()
        {
            return mainMenuSceneIndex;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);
            }
        }

        // For Loading Screen purpose
        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex, LoadSceneMode.Single); // Before Character Creator has been integrated into this project, scene "World_01" will be first level

            yield return null;
        }
    }
}