using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}