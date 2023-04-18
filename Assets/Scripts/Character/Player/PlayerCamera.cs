using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; set; }

        public Camera mainCamera;

        [SerializeField] private float followSpeed = 20f;
        [SerializeField] private float cameraSphereRadius = 0.2f;
        [SerializeField] private float cameraCollisionOffSet = 0.2f;
        [SerializeField] private float minimunCollisionOffSet = 0.2f;

        [SerializeField] private float leftAndRightLookSpeed = 250f;
        [SerializeField] private float leftAndRightAimingLookSpeed = 25f;
        [SerializeField] private float upAndDownLookSpeed = 250f;
        [SerializeField] private float upAndDownAimingLookSpeed = 25f;

        [SerializeField] private float minimunLookDownAngle = -35;
        [SerializeField] private float maximunLookUpAngle = 35;

        private float leftAndRightAngle;
        private float upAndDownAngle;

        [SerializeField] private Transform cameraPivotTransform;

        private Vector3 cameraPos;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        private float targetZPosition;
        private float defaultZPosition;

        private PlayerManager player;

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
            defaultZPosition = mainCamera.transform.localPosition.z;

            player = FindObjectOfType<PlayerManager>();
        }

        public void FollowTarget(Transform targetTransform)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position,
                                                        ref cameraFollowVelocity, Time.deltaTime * followSpeed);
            transform.position = targetPosition;

            HandleCameraCollision();
        }

        private void HandleCameraCollision()
        {
            targetZPosition = defaultZPosition;
            RaycastHit hit;
            Vector3 direction = mainCamera.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast
                                (cameraPivotTransform.position, cameraSphereRadius, direction, out hit,
                                Mathf.Abs(targetZPosition)))
            {
                float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetZPosition = -(distance - cameraCollisionOffSet);
            }

            if (Mathf.Abs(targetZPosition) < minimunCollisionOffSet)
            {
                targetZPosition = -minimunCollisionOffSet;
            }

            cameraPos.z = Mathf.Lerp(mainCamera.transform.localPosition.z, targetZPosition, Time.deltaTime / 0.2f);
            mainCamera.transform.localPosition = cameraPos;
        }

        public void HandleCameraRotation()
        {
            leftAndRightAngle += (PlayerInputManager.Instance.mouseX * leftAndRightLookSpeed) * Time.deltaTime;
            upAndDownAngle -= (PlayerInputManager.Instance.mouseY * upAndDownLookSpeed) * Time.deltaTime;
            upAndDownAngle = Mathf.Clamp(upAndDownAngle, minimunLookDownAngle, maximunLookUpAngle);

            Vector3 rotation = Vector3.zero;
            rotation.y = leftAndRightAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = upAndDownAngle;
            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
    }
}