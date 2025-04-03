using System;
using System.Collections.Generic;
using GameLogic.Enemys;
using UnityEngine;

namespace GameLogic.Turrets
{
    public class TurretRotation : MonoBehaviour
    {
        [SerializeField]
        private float _speedRotation = 8f;
        
        private float rotationTimer;

        private Vector3 _direction = Vector3.forward;
        
        public void RotateTurret(Enemy target)
        {
            _direction = target.transform.position - transform.parent.position;
            Quaternion rotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _speedRotation * Time.deltaTime);
        }

        public bool LookToTarget(Enemy target)
        {
            Vector3 direction = target.transform.position - transform.parent.position;
            float angle = Vector3.Angle(transform.forward, direction);

            if (angle < 3f)
                return true;
            return false;
        }

        public void LookToLastPos()
        {
            transform.forward = _direction;
        }
    }
}