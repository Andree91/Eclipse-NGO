using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; set; }
        public PlayerManager player;
        public Camera mainCamera;
        [SerializeField] private Transform cameraPivotTransform;

        [Header("Camera Settings")]
        [Tooltip("The bigger this number is, the longer for the camera to reach its position duing movement")]
        [SerializeField] private float cameraSmoothSpeed = 1f;
        [SerializeField] private float leftAndRightRotationSpeed = 220f;
        [SerializeField] private float upAndDownRotationSpeed = 220f;
        [SerializeField] private float cameraCollisonRadius = 0.2f;
        [SerializeField] private float cameraCollisionCorrectionSpeed = 20f;

        [SerializeField] private float minimunLookDownAngle = -35f;
        [SerializeField] private float maximunLookUpAngle = 50f;

        [SerializeField] private LayerMask collideWithLayers;

        // [SerializeField] private float cameraCollisionOffSet = 0.2f; // Maybe use these if you want to tweek camera collision
        // [SerializeField] private float minimunCollisionOffSet = 0.2f;

        // [SerializeField] private float leftAndRightAimingLookSpeed = 25f;
        // [SerializeField] private float upAndDownAimingLookSpeed = 25f;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; // USED WHEN CAMERA IS COLLIDING, MOVES CAMERA OBJECT TO THIS POSTION WHEN COLLIDING
        private float leftAndRightLookAngle;
        private float upAndDownLookAngle;
        private float defaultCameraZPosition; // USED WHEN CAMERA IS COLLIDING
        private float targetCameraZPosition;  // USED WHEN CAMERA IS COLLIDING

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
            defaultCameraZPosition = mainCamera.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (player == null) { return; }

            HandleFollowTarget();
            HandleRotations();
            HandleCollision();
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position,
                                                        ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotations()
        {
            // If locked on, force rotation around target

            // Rotation while aiming with bow

            //else Regular rotation

            // ROTATE LEFT AND RIGHT BASED ON HORIZONTAL MOVEMENT OF THE RIGHT STICK / MOUSE
            leftAndRightLookAngle += (PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            // ROTATE UP AND DOWN BASED ON HORIZONTAL MOVEMENT OF THE RIGHT STICK / MOUSE
            upAndDownLookAngle -= (PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            // CLAMP UP AND DOWN BETWEEN MIN AND MAX ANGLE (LEFT AND RIGHT CAN ROTATE 360*)
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimunLookDownAngle, maximunLookUpAngle);


            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation = Quaternion.identity;

            // ROTATE THIS GAMEOBJECT TO LEFT AND RIGHT
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // ROTATE PIVOT GAMEOBJECT TO UP AND DOWN
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollision()
        {
            targetCameraZPosition = defaultCameraZPosition;
            RaycastHit hit;
            Vector3 direction = mainCamera.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position,
                                    cameraCollisonRadius,
                                    direction,
                                    out hit,
                                    Mathf.Abs(targetCameraZPosition), collideWithLayers)) // DEFAULT LAYERMASK (WILL CHANGE)
            {
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisonRadius);
            }

            if (Mathf.Abs(targetCameraZPosition) < cameraCollisonRadius)
            {
                targetCameraZPosition = -cameraCollisonRadius;
            }

            cameraObjectPosition.z = Mathf.Lerp(mainCamera.transform.localPosition.z, targetCameraZPosition, cameraCollisionCorrectionSpeed * Time.deltaTime);
            mainCamera.transform.localPosition = cameraObjectPosition;
        }
    }
}