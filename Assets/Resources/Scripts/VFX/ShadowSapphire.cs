using Resources.Scripts.General;
using Resources.Scripts.Managers;
using Resources.Scripts.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources.Scripts.VFX{
    public class ShadowSapphire : MonoBehaviour{
        
        // Scripts:
        [SerializeField] private RadiusChecker _groundCheckScript;
        
        private Rigidbody2D _rigidbody2D;
        private CircleCollider2D _circleCollider2D;
        private BoxCollider2D _boxCollider2D;
        private Animator _animator;
        [SerializeField] public int _value;
        [SerializeField] private Vector2 _minForce;
        [SerializeField] private Vector2 _maxForce;
        private GameObject[] _sceneEnemies;
        [SerializeField] private float _shineTimeMin = 3f;
        [SerializeField] private float _shineTimeMax = 10f;
        private float _shineTimer;

        public bool _collided;
        
        // Property index:
        private static readonly int Shine = Animator.StringToHash("Shine");

        private void Awake(){
            
            // Fetch components:
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            
            // Set and randomise shine time:
            _shineTimer = Random.Range(_shineTimeMin, _shineTimeMax);
            
            // Apply force on spawn:
            _rigidbody2D.AddForce(new Vector2(
                Random.Range(_minForce.x, _maxForce.x), 
                Random.Range(_minForce.y, _maxForce.y)));
            
            // Ignore colliders:
            _sceneEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (_sceneEnemies.Length > 0){
                foreach (GameObject enemy in _sceneEnemies){
                    Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
                }
            }
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>(), 
                GetComponent<BoxCollider2D>());
        }

        private void Update(){

            // Once on the ground, play animation, and allow the player to pick the sapphire up:
            if (_groundCheckScript._collided){
                _boxCollider2D.enabled = true;
                _circleCollider2D.enabled = true;
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
                // Increment number of shadow sapphires:
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUIHandler>().
                    IncrementShadowSapphires(_value);
                Destroy(gameObject);
            }
        }
    }
}
