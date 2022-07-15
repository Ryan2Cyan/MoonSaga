using UnityEngine;

namespace Resources.Scripts.Enemies.Charger{
    public class ChargerRaycast : MonoBehaviour{

        // Scripts:
        private ChargerData _chargerDataScript;
        
        // Values:
        [SerializeField] internal bool _hitPlayer;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _targetLayer;
        private RaycastHit2D _hit2D;

        private void Awake(){
            _chargerDataScript = GetComponent<ChargerData>();
        }

        private void FixedUpdate(){
            if (_chargerDataScript._isFacingRight){
                _hitPlayer = Physics2D.Raycast(transform.position, Vector2.right, _rayDistance,
                    _targetLayer);
            }
            else{
                _hitPlayer = Physics2D.Raycast(transform.position, Vector2.left, _rayDistance,
                    _targetLayer);
            }
        }
    }
}
