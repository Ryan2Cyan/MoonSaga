using System;
using Resources.Scripts.Enemies.General;
using Resources.Scripts.Enemies.PillBug;
using Resources.Scripts.VFX;
using UnityEngine;

// Code within this class is responsible (only) for collisions the player
// may have with object or enemies:
namespace Resources.Scripts.Player{
    public class PlayerCollision : MonoBehaviour{

        // Scripts:
        private PlayerMovement _playerMovementScript;
        
        // Values:
        private GameObject[] _sceneEnemies;
        internal GameObject _collidedEnemy;
        internal bool _enemyCollision;
        [SerializeField] private BoxCollider2D _boxCollider2D;
        [SerializeField] private CircleCollider2D _circleCollider2D;
        [SerializeField] private Vector2 _originalColliderSize;
        [SerializeField] private Vector2 _originalColliderOffset;
        [SerializeField] private Vector2 _dashColliderSize;
        [SerializeField] private Vector2 _dashColliderOffset;
        [SerializeField] private Vector2 _dashDownColliderSize;
        [SerializeField] private Vector2 _dashDownColliderOffset;

        private void Awake(){
            
            // Fetch components:
            _playerMovementScript = transform.parent.gameObject.GetComponent<PlayerMovement>();
            // Ignore collision with enemy ground collider (circle colliders):
            _sceneEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (_sceneEnemies.Length > 0){
                foreach (GameObject enemy in _sceneEnemies){
                    Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), _circleCollider2D);
                }
            }
        }

        private void Update(){
            
            // Change the shape of the players trigger collider depending on state:
            UpdateColliderSize();
        }

        private void OnTriggerEnter2D(Collider2D other){

            // Collide with enemy:
            if (other.gameObject.CompareTag("EnemyTrigger")){
                // Check if enemy is alive:
                if (other.transform.parent.GetComponent<EnemyData>()._isActive){
                    _enemyCollision = true;
                    _collidedEnemy = other.gameObject;
                }
            }
            
            // Collide with shadow sapphire:
            if (other.gameObject.CompareTag("ShadowSapphire"))
                other.gameObject.GetComponent<ShadowSapphire>()._collided = true;
        }

        private void OnTriggerExit2D(Collider2D other){

            // Collide with enemy:
            if (other.gameObject.CompareTag("EnemyTrigger")){
                _collidedEnemy = null;
                _enemyCollision = false;
            }
        }

        private void UpdateColliderSize(){
            switch (_playerMovementScript._state){
                
                case playerMoveState.Idle:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.Walking:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.Jump:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.DoubleJump:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.AirControl:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.Land:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.Dash:
                    _boxCollider2D.size = _dashColliderSize;
                    _boxCollider2D.offset = _dashColliderOffset;
                    break;
                case playerMoveState.DashHit:
                    break;
                case playerMoveState.DashRecover:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.Damaged:
                    break;
                case playerMoveState.DashDown:
                    _boxCollider2D.size = _dashDownColliderSize;
                    _boxCollider2D.offset = _dashDownColliderOffset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
