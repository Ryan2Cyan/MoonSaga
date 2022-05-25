using UnityEngine;
using UnityEngine.Events;

// Code within this class is responsible (only) for the movement of the 
// player character.
namespace Resources.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour{
        [SerializeField] private playerMoveState _state = playerMoveState.Idle;
        [Range(0, 1000.0f)] [SerializeField] private float _jumpForce = 400f;
        [Range(0, 0.3f)] [SerializeField] private float _movementSpeed = 0.14f;
        [Range(0, 100f)] [SerializeField] private float _runSpeed = 37.5f;
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _ceilingCheck;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private float _horizontalInput;


        private const float _groundedRadius = 0.2f;
        [SerializeField] private bool _isGrounded;
        public UnityEvent OnLandEvent;
        private Vector3 _velocity = Vector3.zero;

        private void Awake(){
            // Fetch components:
            _rigidbody2D = GetComponent<Rigidbody2D>();

            OnLandEvent ??= new UnityEvent();
        }

        private void Update(){
            
            // Update current state:
            UpdateState(ref _state, _rigidbody2D, _isGrounded);

            // Process keyboard input:
            _horizontalInput = Input.GetAxisRaw("Horizontal") * _runSpeed;

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
                _state = playerMoveState.AirControl;
            
            // Check all detected colliders for the ground:
            foreach (Collider2D colliderArg in collider2Ds){
                if (colliderArg.gameObject != gameObject){
                    _isGrounded = true;
                }
                if (!wasGrounded){
                    // If the player has landed, call OnLand event:
                    OnLandEvent.Invoke();
                }
            }
            
            // Move the player based on input:
            Move(_horizontalInput * Time.fixedDeltaTime, ref _state);
        }

        private void Move(float move, ref playerMoveState currentState){
            
            
            // Modify movement speed when the player is in the air:
            if (_state == playerMoveState.AirControl){
                move *= 1.2f;
            }
            
            // Move the character via target velocity:
            Vector3 targetVelocity = new Vector2(move * 10.0f, _rigidbody2D.velocity.y);
            // Apply smoothing:
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity,
                ref _velocity, _movementSpeed);
            
            // Check if the player jumped, if they did, apply force:
            if (currentState == playerMoveState.Jump){
                _isGrounded = false;
                _rigidbody2D.AddForce(new Vector2(0.0f, _jumpForce));
                _state = playerMoveState.AirControl;
            }
        }

        private static void UpdateState(ref playerMoveState currentState, Rigidbody2D rb, bool grounded){

            if (currentState == playerMoveState.Jump){
                return;
            }
            
            switch (grounded){
                // Jump check:
                case true when Input.GetKeyDown(KeyCode.Space):
                    currentState = playerMoveState.Jump;
                    return;
                // Idle check:
                case true when rb.velocity == Vector2.zero:
                    currentState = playerMoveState.Idle;
                    return;
                // Walking check:
                case true when rb.velocity != Vector2.zero:
                    currentState = playerMoveState.Walking;
                    return;
                // Air control check:
                case false:
                    currentState = playerMoveState.AirControl;
                    return;
            }
        }

        public void Print(){
            
            Debug.Log("Landed");
        }
    }

    internal enum playerMoveState{
        Idle, Walking, Running, Jump, AirControl 
    }
}
