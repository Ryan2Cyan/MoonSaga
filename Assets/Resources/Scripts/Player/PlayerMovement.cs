using UnityEngine;
using UnityEngine.Events;

// Code within this class is responsible (only) for the movement of the 
// player character.
namespace Resources.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour{
        [Range(0, 1000.0f)] [SerializeField] private float _jumpForce = 400f;
        [Range(0, 0.3f)] [SerializeField] private float _movementSpeed = 0.14f;
        [Range(0, 100f)] [SerializeField] private float _runSpeed = 37.5f;
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _ceilingCheck;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private bool _airControl = false;
        [SerializeField] private bool _isJumping = false;
        [SerializeField] private float _horizontalInput;


        private const float _groundedRadius = 0.2f;
        private bool _isGrounded;
        public UnityEvent OnLandEvent;
        private Vector3 _velocity = Vector3.zero;

        private void Awake(){
            
            // Fetch components:
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            if (OnLandEvent == null)
                OnLandEvent = new UnityEvent();
        }

        private void Update(){
            
            // Process keyboard input:
            _horizontalInput = Input.GetAxisRaw("Horizontal") * _runSpeed;
            if (Input.GetKeyDown(KeyCode.Space)){
                _isJumping = true;
            }
        }

        private void FixedUpdate(){

            bool wasGrounded = _isGrounded;
            _isGrounded = false;
            
            // Store all colliders within ground-check's radius, on the ground layer:
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(
                _groundCheck.position,
                _groundedRadius,
                _groundLayerMask
            );
            // Check if no ground is detected:
            if(collider2Ds.Length == 0)
                _airControl = true;
            foreach (var colliderArg in collider2Ds){
                if (colliderArg.gameObject != gameObject)
                    _isGrounded = true;
                else
                    _airControl = true;
                if (!wasGrounded){
                    // If the player has landed, call OnLand event:
                    OnLandEvent.Invoke();
                    _airControl = false;
                }
            }
            
            // Move the player based on input:
            Move(_horizontalInput * Time.fixedDeltaTime, _isJumping);
        }

        private void Move(float move, bool jump){

            // Modify movement speed when the player is in the air:
            if (_airControl){
                move *= 1.2f;
            }
            
            // Move the character via target velocity:
            Vector3 targetVelocity = new Vector2(move * 10.0f, _rigidbody2D.velocity.y);
            // Apply smoothing:
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity,
                ref _velocity, _movementSpeed);
            
            // Check if the player could jump:
            if (_isGrounded && jump){
                _isGrounded = false;
                _rigidbody2D.AddForce(new Vector2(0.0f, _jumpForce));
            }
        }

        public void Print(){
            
            _isJumping = false;
            Debug.Log("Landed");
        }
    }
}
