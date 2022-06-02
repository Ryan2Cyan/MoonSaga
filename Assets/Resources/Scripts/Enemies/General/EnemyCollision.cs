using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EnemyCollision : MonoBehaviour{

        // Scripts:
        [SerializeField] private EnemyMovement _enemyMovementScript;
        // Trigger collider:
        [SerializeField] internal Collider2D _boxCollider;

        // Values:
        private const float _groundedRadius = 0.2f;
        [SerializeField] internal bool _isGrounded;
        [SerializeField] internal bool _atEdge;
        [SerializeField] internal bool _wasAtEdge;
        [SerializeField] internal Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayerMask;
        private const float _edgeCheckDelay = 0.05f;
        private float _edgeCheckTimer;

        private void Awake(){
            // Fetch values:
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            // Ignore collisions with player:
            Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<Collider2D>());
            
            // Set values:
            _edgeCheckTimer = _edgeCheckDelay;
        }

        private void Update(){
            _atEdge = false;
            _edgeCheckTimer -= Time.deltaTime;
            
            // Check if the enemy is touching the ground or not:
            GroundCheck();
            // Check if enemy is at an edge, flip if true:
            if (_edgeCheckTimer <= 0f){
                Debug.Log("Check");
                _atEdge = EdgeCheck();
                if (_atEdge)
                    _enemyMovementScript._isFacingRight = !_enemyMovementScript._isFacingRight;
                _edgeCheckTimer = _edgeCheckDelay;
            }
        }
        


        private void GroundCheck(){
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

        private bool EdgeCheck(){

            // Cast ray from enemy, downwards, to check if there is any ground:
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 2f, _groundLayerMask);
            foreach (RaycastHit2D hitValue in hits){
                return false;
            }

            return true;
        }
    }
}
