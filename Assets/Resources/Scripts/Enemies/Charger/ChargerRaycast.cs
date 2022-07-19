using Resources.Scripts.Enemies.General;
using UnityEngine;

namespace Resources.Scripts.Enemies.Charger{
    public class ChargerRaycast : MonoBehaviour{

        // Scripts:
        private EnemyData _chargerDataScript;
        
        // Values:
        [SerializeField] internal bool _hitTarget;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _targetLayer;
        private RaycastHit2D _hit2D;

        private void Awake(){
            _chargerDataScript = GetComponent<ChargerData>();
        }

        private void FixedUpdate(){
            _hitTarget = Physics2D.Raycast(
                transform.position, _chargerDataScript._isFacingRight ? Vector2.right : 
                    Vector2.left, _rayDistance, _targetLayer);
        }
    }
}
