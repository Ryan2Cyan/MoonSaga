using UnityEngine;

// Code within this class is responsible (only) for collisions the player
// may have with object or enemies:
namespace Resources.Scripts.Player{
    public class PlayerCollision : MonoBehaviour{
        
        // Trigger collider:
        [SerializeField] internal Collider2D _boxCollider;
        

        // Values:
        [SerializeField] internal bool _enemyCollision;
        
        private void OnTriggerEnter2D(Collider2D other){

            if (other.gameObject.CompareTag("Enemy"))
                _enemyCollision = true;
        }

        private void OnTriggerExit2D(Collider2D other){
            
            if (other.gameObject.CompareTag("Enemy"))
                _enemyCollision = false;
        }
    }
}
