using Resources.Scripts.Enemies.General;
using Resources.Scripts.General;
using UnityEngine;

// Code within this class is responsible for detecting the edge of a
// platform, and flipping the enemy to prevent it falling. This script
// is unique to the "Charger" enemy class:
namespace Resources.Scripts.Enemies.Charger{
    public class ChargerEdgeChecker : MonoBehaviour
    {
        private EnemyData _enemyDataScript;
        private ChargerMovement _chargerMovementScript;
        private void Awake(){
            
            // Fetch Components:
            _enemyDataScript = GetComponent<EnemyData>();
            _chargerMovementScript = GetComponent<ChargerMovement>();

            // Check if enemy is facing left or right:
            if (transform.localScale.x < 0f)
                _enemyDataScript._isFacingRight = true;
        }

        private void OnCollisionEnter2D(Collision2D other){
            
            // If not charging - turn the enemy around if they reach an edge:
            if (other.gameObject.CompareTag("PlatformEdge") && _chargerMovementScript._state != enemyMoveState.Charge)
                transform.localScale = UtilityFunctions.Flip(transform.localScale, 
                    ref _enemyDataScript._isFacingRight);
        }
    }
}
