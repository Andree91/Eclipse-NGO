using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController characterController;

        private CharacterNetworkManager characterNetworkManager;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
            // Only owner of this network object can set it's network position & rotation
            if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            
            // Assinging other network object position & rotation locally by the position & rotation of its network transform
            else
            {
                // POSITION
                transform.position = Vector3.SmoothDamp(
                                        transform.position,
                                        characterNetworkManager.networkPosition.Value, 
                                        ref characterNetworkManager.networkPositionVelocity, 
                                        characterNetworkManager.networkPositionSmoothTime);

                // ROTATION
                transform.rotation = Quaternion.Slerp(
                                        transform.rotation, 
                                        characterNetworkManager.networkRotation.Value, 
                                        characterNetworkManager.networkRotationSmoothTime);
            }
        }
    }
}