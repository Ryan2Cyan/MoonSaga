using Resources.Scripts.General;
using UnityEngine;

// Code within this class is responsible (only) for collisions the player
// may have with object or enemies:
namespace Resources.Scripts.Player{
    public class PlayerCollision : MonoBehaviour{
        
        // Trigger collider:
        [SerializeField] internal Collider2D _boxCollider;
        

        // Values:
        [SerializeField] internal bool _enemyCollision;
        private const float _groundedRadius = 0.2f;
        [SerializeField] internal bool _isGrounded;
        [SerializeField] internal Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private Transform _ceilingCheck;
        
        private void FixedUpdate(){
            
            // Check if the player is touching the ground or not:
            GroundCheck();
        }

        private void OnTriggerEnter2D(Collider2D other){

            if (other.gameObject.CompareTag("Enemy"))
                _enemyCollision = true;
        }

        private void OnTriggerExit2D(Collider2D other){
            
            if (other.gameObject.CompareTag("Enemy"))
                _enemyCollision = false;
        }

        private void GroundCheck(){
            bool wasGrounded = _isGrounded;
            _isGrounded = false;

            // Store all colliders within ground-check's radius, on the ground layer:
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(
                _groundCheck.position,
                _groundedRadius,
                _groundLayerMask
            );

            // Check if no ground is detected:
            if (collider2Ds.Length != 0){
                // Check all detected colliders for the ground:
                foreach (Collider2D colliderArg in collider2Ds){
                    if (colliderArg.gameObject != gameObject){
                        _isGrounded = true;
                    }
                }

            }
        }
    }
}
