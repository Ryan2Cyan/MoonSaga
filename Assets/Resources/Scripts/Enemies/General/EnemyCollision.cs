using UnityEngine;

// This script is responsible for detecting all general collisions required
// by all enemies. This includes death zones, the player, and others:
namespace Resources.Scripts.Enemies.General{
    public class EnemyCollision : MonoBehaviour{
        
        // Values:
        [SerializeField] internal bool _collidingWithPlayer;
        private GameObject[] _sceneEnemies;
        [SerializeField] private CircleCollider2D _circleCollider2D;
        
        private void Awake(){
            
            // Ignore collision with enemy ground collider (circle colliders):
            _sceneEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (_sceneEnemies.Length > 0){
                foreach (GameObject enemy in _sceneEnemies){
                    Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), _circleCollider2D);
                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other){
            
            // Collision with player:
            if (other.gameObject.CompareTag("Player"))
                _collidingWithPlayer = true;

            // Collision with death zone:
            if (other.gameObject.CompareTag("DeathZone")){
                transform.parent.GetComponent<EnemyData>()._hp = 0;
            }
        }

        private void OnTriggerExit2D(Collider2D other){
            
            // Collision with player:
            if (other.gameObject.CompareTag("Player"))
                _collidingWithPlayer = false;
        }
    }
}
