using UnityEngine;

// Code within this class is responsible for casting a ray in front
// of an enemy (at a specified length), to detect a specified target:
namespace Resources.Scripts.Enemies.General{
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
