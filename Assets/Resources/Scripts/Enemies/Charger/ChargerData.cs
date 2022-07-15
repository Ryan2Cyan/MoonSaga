using Resources.Scripts.Enemies.PillBug;
using UnityEngine;

namespace Resources.Scripts.Enemies.Charger{
    public class ChargerData : EnemyData{

        [SerializeField] internal float _chargeSpeed;
        [SerializeField] internal float _chargePauseTime;
        [SerializeField] internal float _chargeTime;
        internal float _chargeTimer;
    }
}
