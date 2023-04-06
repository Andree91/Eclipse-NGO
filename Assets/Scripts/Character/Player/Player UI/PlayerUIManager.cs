using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Instance { get; set; }

        [Header("NETWORK JOINED")]
        [SerializeField] private bool startGameAsClient;

        public bool GetStartGameAsClient()
        {
            return startGameAsClient;
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

        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                // Must first shutdown, because usually we start as a host during the title screen
                NetworkManager.Singleton.Shutdown();
                // Then restart as a client
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}