using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EnemyCollision : MonoBehaviour{
        
        // Values:
        [SerializeField] internal bool _collidingWithPlayer;

        private void OnTriggerEnter2D(Collider2D other){
            
            // Collision with player:
            if (other.gameObject.CompareTag("Player"))
                _collidingWithPlayer = true;
        }
        
        // private void OnTriggerStay2D(Collider2D other){
        //     
        //     // Collision with player:
        //     if (other.gameObject.CompareTag("Player"))
        //         _collidingWithPlayer = false;
        // }

        private void OnTriggerExit2D(Collider2D other){
            
            // Collision with player:
            if (other.gameObject.CompareTag("Player"))
                _collidingWithPlayer = false;
        }
        
    }
}
