using Resources.Scripts.Managers;
using Resources.Scripts.VFX;
using UnityEngine;

// Code within this class is responsible (only) for collisions the player
// may have with object or enemies:
namespace Resources.Scripts.Player{
    public class PlayerCollision : MonoBehaviour{
        
        // Scripts:
        private GameData _gameDataScript;
        [SerializeField] private PlayerUIHandler _playerUIHandlerScript;
        
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
            }
            
            // Collide with shadow sapphire:
            if (other.gameObject.CompareTag("ShadowSapphire")){
                Destroy(other.gameObject);
                _playerUIHandlerScript.IncrementShadowSapphires(other.gameObject.GetComponent<ShadowSapphire>()._value);
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
