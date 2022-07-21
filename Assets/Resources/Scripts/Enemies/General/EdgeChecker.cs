using Resources.Scripts.General;
using UnityEngine;

// Code within this class is responsible for detecting the edge of a
// platform, and flipping the enemy to prevent it falling:
namespace Resources.Scripts.Enemies.General{
    public class EdgeChecker : MonoBehaviour{
        
        private EnemyData _enemyDataScript;
        private void Awake(){
            
            // Fetch Components:
            _enemyDataScript = GetComponent<EnemyData>();

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
