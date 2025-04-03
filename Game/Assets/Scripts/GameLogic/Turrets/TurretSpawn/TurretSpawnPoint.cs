using UnityEngine;

namespace GameLogic.Turrets.TurretSpawn
{
    public class TurretSpawnPoint : MonoBehaviour
    {
        private bool _isFree = true;
        public Turret _turretActive;

        public void SetFreeState(bool active, Turret turret)
        {
            _isFree = active;
            _turretActive = turret;
        }

        public void DestroyTurretOnPoint()
        {
            Destroy(_turretActive.gameObject);
            _isFree = true;
        }
        
        public Turret GetActiveTurret() =>
            _turretActive;
        
        public bool IsFree() => _isFree;
    }
}