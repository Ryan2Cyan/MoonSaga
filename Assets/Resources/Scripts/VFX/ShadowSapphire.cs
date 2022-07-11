using Resources.Scripts.General;
using Resources.Scripts.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources.Scripts.VFX{
    public class ShadowSapphire : MonoBehaviour{
        
        // Scripts:
        [SerializeField] private GroundCheck _groundCheckScript;
        
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        [SerializeField] public int _value;
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxY;
        private GameObject[] _sceneEnemies;
        [SerializeField] private float _shineTimeMin = 3f;
        [SerializeField] private float _shineTimeMax = 10f;
        [SerializeField] private float _shineTimer;

        public bool _collided;
        
        // Property index:
        private static readonly int Shine = Animator.StringToHash("Shine");

        private void Awake(){
            
            // Fetch components:
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            
            // Set and randomise shine time:
            _shineTimer = Random.Range(_shineTimeMin, _shineTimeMax);
            
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
            
            // Ignore player's circle collider:
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>(), 
                GetComponent<BoxCollider2D>());
        }

        private void Update(){

            // Once on the ground, play animation, and allow the player to pick the sapphire up:
            if (_groundCheckScript._isGrounded){
                GetComponent<BoxCollider2D>().enabled = true;
                GetComponent<CircleCollider2D>().enabled = true;
                _animator.enabled = true;
                _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);
            }

            // Play the shine animation after certain period:
            _shineTimer -= Time.deltaTime;
            if (_shineTimer <= 0f){
                _animator.SetTrigger(Shine);
                _shineTimer = Random.Range(_shineTimeMin, _shineTimeMax);
            }
            
            // Check for collision with player:
            if (_collided){
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUIHandler>().
                    IncrementShadowSapphires(_value);
                Destroy(gameObject);
            }
        }
        
        private void OnCollisionEnter2D(Collision2D other){
            if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("PlatformEdge") ){
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            if (other.gameObject.CompareTag("Player")){
                Destroy(gameObject);
            }
        }
    }
}
