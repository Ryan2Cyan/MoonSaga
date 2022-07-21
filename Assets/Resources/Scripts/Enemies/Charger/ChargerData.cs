using Resources.Scripts.Enemies.General;
using UnityEngine;

// Code within this class is responsible for storing additional
// data unique to the "Charger" enemy class:
namespace Resources.Scripts.Enemies.Charger{
    public class ChargerData : EnemyData{

        [SerializeField] internal BoxCollider2D _armourCollider;
        [SerializeField] internal float _chargeSpeed;
        [SerializeField] internal float _chargePauseTime;
        [SerializeField] internal float _chargeTime;
        internal float _chargePauseTimer;
        internal float _chargeTimer;
        [SerializeField] internal float _stunTime;
        internal float _stunTimer;
    }
}
