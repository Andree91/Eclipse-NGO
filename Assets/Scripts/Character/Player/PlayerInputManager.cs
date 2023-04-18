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

        [SerializeField] private Vector2 movementInput = Vector2.zero;

        public float verticalInput = 0f;
        public float horizontalInput = 0f;
        public float moveAmount = 0f;

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

        // If window is not active, stop adjusting inputs (good for testing with two project windows/builds)
        private void OnApplicationFocus(bool focusStatus)
        {
            if (enabled)
            {
                if (focusStatus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
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

            // Clamping values to 0, 0.5 and 1 for better control while using gamepad
            if (moveAmount <= 0.5f && moveAmount > 0f)
            {
                moveAmount = 0.5f; // Character is walking
            }
            else if (moveAmount > 0.5f && moveAmount <= 1f)
            {
                moveAmount = 1f; // Character is running
            }
        }
    }
}