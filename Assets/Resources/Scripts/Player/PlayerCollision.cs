using Resources.Scripts.General;
using UnityEngine;

// Code within this class is responsible (only) for collisions the player
// may have with object or enemies:
namespace Resources.Scripts.Player{
    public class PlayerCollision : MonoBehaviour{
        
        // Scripts:
        [SerializeField] private GroundCheck _groundCheckScript;
        [SerializeField] private PlayerMovement _playerMovementScript;
        [SerializeField] private GameObject[] _sceneEnemies;
        
        // Trigger collider:
        [SerializeField] internal Collider2D _boxCollider;
        
        // Values:
        private Rigidbody2D _rigidbody;
        [SerializeField] internal bool _enemyCollision;

        private void Awake(){
            _rigidbody = GetComponent<Rigidbody2D>();
            
            // Ignore collision with enemy ground collider:
            _sceneEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in _sceneEnemies){
                Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
            }
        }
        private void OnTriggerEnter2D(Collider2D other){

            if (other.gameObject.CompareTag("Enemy"))
                _enemyCollision = true;
            // If player collides with platform, but is not on top of it, make sure they slide off:
            if (other.gameObject.layer == 6 && !_groundCheckScript._isGrounded){
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y);
            }
        }
        private void OnTriggerExit2D(Collider2D other){
            
            if (other.gameObject.CompareTag("Enemy"))
                _enemyCollision = false;
        }
    }
}
