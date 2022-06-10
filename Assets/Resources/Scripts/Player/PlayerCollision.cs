using Resources.Scripts.Enemies.General;
using UnityEngine;

// Code within this class is responsible (only) for collisions the player
// may have with object or enemies:
namespace Resources.Scripts.Player{
    public class PlayerCollision : MonoBehaviour{
        
        // Trigger collider:
        [SerializeField] internal Collider2D _boxCollider;
        
        // Values:
        private GameObject[] _sceneEnemies;
        internal GameObject _collidedEnemy;
        internal bool _enemyCollision;

        private void Awake(){
            
            // Ignore collision with enemy ground collider (circle colliders):
            _sceneEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in _sceneEnemies){
                Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
            }
        }
        private void OnTriggerEnter2D(Collider2D other){

            if (other.gameObject.CompareTag("Enemy")){
                _enemyCollision = true;
                _collidedEnemy = other.gameObject;
                _collidedEnemy.GetComponent<EnemyCollision>()._collidingWithPlayer = true;
            }
        }

        private void OnTriggerStay2D(Collider2D other){
            if (other.gameObject.CompareTag("Enemy")){
                _enemyCollision = true;
                _collidedEnemy = other.gameObject;
                _collidedEnemy.GetComponent<EnemyCollision>()._collidingWithPlayer = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other){

            if (other.gameObject.CompareTag("Enemy")){
                _collidedEnemy = null;
                _enemyCollision = false;
            }
            
        }
    }
}
