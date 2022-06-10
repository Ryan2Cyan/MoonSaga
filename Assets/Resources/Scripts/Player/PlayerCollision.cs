using Resources.Scripts.Enemies.General;
using Resources.Scripts.Managers;
using UnityEngine;

// Code within this class is responsible (only) for collisions the player
// may have with object or enemies:
namespace Resources.Scripts.Player{
    public class PlayerCollision : MonoBehaviour{
        
        // Scripts:
        private GameData _gameDataScript;
        
        // Values:
        private GameObject[] _sceneEnemies;
        internal GameObject _collidedEnemy;
        internal bool _enemyCollision;
        [SerializeField] private CircleCollider2D _circleCollider2D;

        private void Awake(){
            
            // Fetch components:
            _gameDataScript = GameObject.Find("Data-Manager").GetComponent<GameData>();
            
            // Ignore collision with enemy ground collider (circle colliders):
            _sceneEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (_sceneEnemies.Length > 0){
                foreach (GameObject enemy in _sceneEnemies){
                    Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), _circleCollider2D);
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D other){

            // Collide with enemy:
            if (other.gameObject.CompareTag("Enemy")){
                _enemyCollision = true;
                _collidedEnemy = other.gameObject;
                _collidedEnemy.GetComponent<EnemyCollision>()._collidingWithPlayer = true;
            }
            
            // Collide with shadow sapphire:
            if (other.gameObject.CompareTag("ShadowSapphire0")){
                Destroy(other.gameObject);
                _gameDataScript.shadowSapphires += 1;
            }
        }

        private void OnTriggerStay2D(Collider2D other){
            
            // Collide with enemy:
            if (other.gameObject.CompareTag("Enemy")){
                _enemyCollision = true;
                _collidedEnemy = other.gameObject;
                _collidedEnemy.GetComponent<EnemyCollision>()._collidingWithPlayer = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other){

            // Collide with enemy:
            if (other.gameObject.CompareTag("Enemy")){
                _collidedEnemy = null;
                _enemyCollision = false;
            }
            
        }
    }
}
