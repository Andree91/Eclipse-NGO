using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        [Header("Movement Stats")]
        [SerializeField] private float walkingSpeed = 2.0f;
        [SerializeField] private float runningSpeed = 5.0f;
        [SerializeField] private float rotationSpeed = 8.0f;

        public float verticalMovement = 0f;
        public float horizontalMovement = 0f;
        public float moveAmount = 0f;

        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;

        private PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            if (player == null)
            {
                player = GetComponent<PlayerManager>();
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundMovement();
            // Aerial Movement  (Jumping, Falling)
            HandleRotation();
        }

        private void HandleGroundMovement()
        {
            GetVerticalAndHorizontalInputs();

            // Move direction is based on player's camera perspective and player's movement inputs
            moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0f;

            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }

        private void GetVerticalAndHorizontalInputs()
        {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;
        }

        private void HandleRotation()
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.Instance.mainCamera.transform.forward * verticalMovement;
            targetRotationDirection += PlayerCamera.Instance.mainCamera.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0f;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;

            // if (moveDirection != Vector3.zero)
            // {
            //     transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed * Time.deltaTime);
            // }
        }
    }
}