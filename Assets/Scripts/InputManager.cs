using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class InputManager : NetworkBehaviour
    {
        [SerializeField] private float speed = 15.0f;

        // Just for testing
        private void Update()
        {
            if (!NetworkObject.IsOwner) { return; }
            
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
            }
        }
    }
}