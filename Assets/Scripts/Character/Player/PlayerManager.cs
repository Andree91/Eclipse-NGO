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

        protected override void Update()
        {
            base.Awake();

            playerLocomotionManager.HandleAllMovement();
        }
    }
}