using UnityEngine;

namespace Resources.Scripts.Enemies.PillBug{
    public class EnemyRaycast : MonoBehaviour
    {
        // Scripts:
        private EnemyData _dataScript;
        
        // Values:
        [SerializeField] internal bool _hitTarget;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _targetLayer;
        private RaycastHit2D _hit2D;

        private void Awake(){
            _dataScript = GetComponent<EnemyData>();
        }

        private void FixedUpdate(){
            _hitTarget = Physics2D.Raycast(
                transform.position, _dataScript._isFacingRight ? Vector2.right : 
                    Vector2.left, _rayDistance, _targetLayer);
        }
    }
}
