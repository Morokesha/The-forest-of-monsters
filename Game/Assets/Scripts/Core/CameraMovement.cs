using System;
using UnityEngine;

namespace Core
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        private float _cameraSpeed;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                transform.position += Vector3.forward * _cameraSpeed;
            if (Input.GetKeyDown(KeyCode.S))
                transform.position += Vector3.back* _cameraSpeed;
            if (Input.GetKeyDown(KeyCode.A))
                transform.position += Vector3.left* _cameraSpeed;
            if (Input.GetKeyDown(KeyCode.D))
                transform.position += Vector3.right* _cameraSpeed;
        }
    }
}