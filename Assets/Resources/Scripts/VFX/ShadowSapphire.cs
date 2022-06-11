using System;
using Resources.Scripts.General;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources.Scripts.VFX{
    public class ShadowSapphire : MonoBehaviour{
        
        // Scripts:
        [SerializeField] private GroundCheck _groundCheckScript;
        
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxY;
        private GameObject[] _sceneEnemies;
        private void Awake(){
            
            // Fetch components:
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            
            // Apply force on spawn:
            _rigidbody2D.AddForce(new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY)));
            
            // Ignore collision with enemy ground collider (circle colliders):
            _sceneEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (_sceneEnemies.Length > 0){
                foreach (GameObject enemy in _sceneEnemies){
                    Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
                    Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
                }
            }
        }

        private void Update(){

            if (_groundCheckScript._isGrounded){
                GetComponent<BoxCollider2D>().enabled = true;
                GetComponent<CircleCollider2D>().enabled = true;
                _animator.enabled = true;
                _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);
            }
        }
        

        private void OnCollisionEnter2D(Collision2D other){
            if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("PlatformEdge") ){
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }
}
