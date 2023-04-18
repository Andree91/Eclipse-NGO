using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; set; }

        public Camera mainCamera;

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

           mainCamera = Camera.main;
        }
    }
}