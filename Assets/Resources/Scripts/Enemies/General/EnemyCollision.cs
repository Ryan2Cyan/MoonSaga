using Resources.Scripts.General;
using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EnemyCollision : MonoBehaviour{

        // Scripts:
        [SerializeField] private EnemyMovement _enemyMovementScript;
        // Trigger collider:
        [SerializeField] internal Collider2D _boxCollider;

        // Values:
        [SerializeField] internal bool _collidingWithPlayer;

        private void Awake(){
            // Fetch values:
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            // Ignore collisions with player:
            Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<Collider2D>());
        }

        private void OnCollisionEnter2D(Collision2D other){
            // Turn the enemy around if they reach an edge:
            if (other.gameObject.CompareTag("PlatformEdge"))
                transform.localScale = UtilityFunctions.Flip(transform.localScale, 
                    ref _enemyMovementScript._isFacingRight);
        }
    }
}
