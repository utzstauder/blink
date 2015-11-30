using System;
using UnityEngine;

    public class Restarter : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
    }
