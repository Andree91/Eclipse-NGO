using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class TitleScreenManager : MonoBehaviour
    {
        [SerializeField] private Button pressStartButton = null;

        private void Awake()
        {
            pressStartButton.Select();
        }

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
        }
    }
}