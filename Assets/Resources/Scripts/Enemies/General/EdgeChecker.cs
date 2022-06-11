using Resources.Scripts.General;
using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EdgeChecker : MonoBehaviour
    {
        // Scripts:
        [SerializeField] private EnemyMovement _enemyMovementScript;

        private void Awake(){
            
            // Fetch Components:
            _enemyMovementScript = GetComponent<EnemyMovement>();
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
