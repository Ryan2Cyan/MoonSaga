using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EnemyCollision : MonoBehaviour{

        // Scripts:
        [SerializeField] private EnemyMovement _enemyMovementScript;
        // Trigger collider:
        [SerializeField] internal Collider2D _boxCollider;

        // Values:

        private void Awake(){
            // Fetch values:
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            // Ignore collisions with player:
            Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<Collider2D>());
        }

        private void OnCollisionEnter2D(Collision2D other){
            if (other.gameObject.CompareTag("PlatformEdge")){
                _enemyMovementScript._isFacingRight = !_enemyMovementScript._isFacingRight;
                Debug.Log("Hit Edge");
            }
        }
    }
}
