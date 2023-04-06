using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        [Header("Movement Stats")]
        [SerializeField] private float movementSpeed = 5.0f;
        [SerializeField] private float rotationSpeed = 8.0f;

        private Vector3 movementVelocity;
        private float verticalVelocity;

        private PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void HandleAllMovement()
        {
            HandleGroundMovement();
            // Aerial Movement  (Jumping, Falling)
            // Rotation
        }

        private void HandleGroundMovement()
        {
            CalculatePlayerMovement();
            HandlePlayerMovement();
        }

        void CalculatePlayerMovement()
        {
            // Movement
            movementVelocity.Set(player.playerInputManager.horizontalInput, 0.0f, player.playerInputManager.verticalInput);
            movementVelocity.Normalize();
            movementVelocity *= movementSpeed * Time.deltaTime;

            // ROTATION
            if (movementVelocity != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementVelocity), rotationSpeed * Time.deltaTime);
            }
        }

        void HandlePlayerMovement()
        {
            player.characterController.Move(movementVelocity);
        }
    }
}