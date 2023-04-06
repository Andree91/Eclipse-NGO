using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance { get; set; }

        private PlayerControls playerControls;

        public float horizontalInput = 0;
        public float verticalInput = 0;
        public float moveAmount = 0;

        [SerializeField] private Vector2 movementInput = Vector2.zero;

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

        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChange;

            Instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
            {
                Instance.enabled = true;
            }
            else
            {
                // Stops player movement when we are on the Character Creation menu etc
                Instance.enabled = false; ;
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            // When destroing this object, Unsubscribe for stoping memory leaks
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void Update()
        {
            HandleMoveInput();
        }

        private void HandleMoveInput()
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        }
    }
}