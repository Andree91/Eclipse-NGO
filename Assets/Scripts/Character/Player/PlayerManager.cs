using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerManager : CharacterManager
    {
        public PlayerInputManager playerInputManager;
        public PlayerLocomotionManager playerLocomotionManager;

        private PlayerCamera playerCamera;

        protected override void Awake()
        {
            base.Awake();

            // Only for the player

            if (playerInputManager == null)
            {
                playerInputManager = FindObjectOfType<PlayerInputManager>();
            }

            playerCamera = FindObjectOfType<PlayerCamera>();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner) { return; }

            playerLocomotionManager.HandleAllMovement();

            playerCamera.FollowTarget(this.transform);
            //playerCamera.HandleCameraRotation();
        }
    }
}