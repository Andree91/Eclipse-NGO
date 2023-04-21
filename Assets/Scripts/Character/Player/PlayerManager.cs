using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerManager : CharacterManager
    {
        public PlayerInputManager playerInputManager;
        public PlayerLocomotionManager playerLocomotionManager;

        protected override void Awake()
        {
            base.Awake();

            // Only for the player

            if (playerInputManager == null)
            {
                playerInputManager = FindObjectOfType<PlayerInputManager>();
            }

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // If this is the player object owned by this client
            if (IsOwner)
            {
                PlayerCamera.Instance.player = this;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner) { return; }

            playerLocomotionManager.HandleAllMovement();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (!IsOwner) { return; }

            PlayerCamera.Instance.HandleAllCameraActions();
        }
    }
}