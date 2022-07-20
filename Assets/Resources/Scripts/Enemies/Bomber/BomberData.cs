using Resources.Scripts.Enemies.General;
using Resources.Scripts.General;
using UnityEngine;

namespace Resources.Scripts.Enemies.Bomber{
    public class BomberData : EnemyData{
        
        [SerializeField] internal RadiusChecker _playerRadiusCheckerScript;
        [SerializeField] internal float _agroTime;
        internal float _agroTimer;
        [SerializeField] internal float _shootTime;
        internal float _shootTimer;
        [SerializeField] internal float _coolDownTime;
        internal float _coolDownTimer;
    }
}
