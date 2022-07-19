using Resources.Scripts.General;
using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EdgeChecker : MonoBehaviour{
        
        private EnemyData _enemyDataScript;
        private void Awake(){
            
            // Fetch Components:
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            _enemyDataScript = GetComponent<EnemyData>();
            
            // Ignore collisions with player:
            Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<Collider2D>());
            
            // Check if enemy is facing left or right:
            if (transform.localScale.x < 0f)
                _enemyDataScript._isFacingRight = true;
            
        }

        private void OnCollisionEnter2D(Collision2D other){
            // Turn the enemy around if they reach an edge:
            if (other.gameObject.CompareTag("PlatformEdge"))
                transform.localScale = UtilityFunctions.Flip(transform.localScale, 
                    ref _enemyDataScript._isFacingRight);
        }
    }
}
