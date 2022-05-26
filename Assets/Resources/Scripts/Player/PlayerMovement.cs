using Resources.Scripts.General;
using UnityEngine;
using UnityEngine.Events;

// Code within this class is responsible (only) for the movement of the 
// player character.
namespace Resources.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour{
        [SerializeField] private playerMoveState _state = playerMoveState.Idle;
        [Range(0, 1000.0f)] [SerializeField] private float _jumpForce = 100f;
        [Range(0, 0.3f)] [SerializeField] private float _movementSpeed = 0.14f;
        [Range(0, 100f)] [SerializeField] private float _runSpeed = 37.5f;
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _ceilingCheck;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private float _horizontalInput;


        private const float _groundedRadius = 0.2f;
        private const float _jumpDelay = 0.05f;
        private const float _maxAirTime = 0.5f;
        private float _maxAirTimer;
        [SerializeField] private float _jumpTimer;
        [SerializeField] private bool _isGrounded;
        [SerializeField] private bool _isFacingRight = true;
        public UnityEvent OnLandEvent;
        private Vector3 _velocity = Vector3.zero;


        private void Awake(){
            
            // Fetch components:
            _rigidbody2D = GetComponent<Rigidbody2D>();

            // Set values:
            OnLandEvent ??= new UnityEvent();
        }

        private void Update(){
            
            // Process player input:
            ProcessInput();
            
            // Update current state:
            UpdateState(ref _state, _rigidbody2D, _isGrounded);
            
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
            if (collider2Ds.Length != 0){
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
            }

            // Move the player based on input:
            Move(_horizontalInput * Time.fixedDeltaTime, ref _state);
        }
        private void Move(float move, ref playerMoveState currentState){
            
            
            // Modify movement speed when the player is in the air:
            if (_state == playerMoveState.AirControl){
                move *= 1.2f;
            }
            
            // Check if the player needs to be flipped depending on move direction:
            if (move > 0.0f && !_isFacingRight){ // Flip Right
                transform.localScale = UtilityFunctions.Flip(transform.localScale, ref _isFacingRight);
            }
            else if (move < 0.0f && _isFacingRight){ // Flip Left
                transform.localScale = UtilityFunctions.Flip(transform.localScale, ref _isFacingRight);
            }
                
            // Move the character via target velocity:
            Vector3 targetVelocity = new Vector2(move * 10.0f, _rigidbody2D.velocity.y);
            // Apply smoothing:
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity,
                ref _velocity, _movementSpeed);
            
            // Change movement based on current state:
            switch (currentState){
                // Check if the player jumped, if they did, apply force:
                case playerMoveState.Jump when _isGrounded:
                    _rigidbody2D.AddForce(new Vector2(0.0f, _jumpForce));
                    break;
                // If player has released jump, reduce their y-axis velocity:
                case playerMoveState.ReleaseJump:
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y / 2.0f);
                    _state = playerMoveState.AirControl;
                    break;
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
            }
        }

        private void ProcessInput(){
            
            // Inputs Variables:
            bool _jumpPress = Input.GetButtonDown("Jump");
            bool _jumpRelease = Input.GetButtonUp("Jump");
            _horizontalInput = Input.GetAxisRaw("Horizontal") * _runSpeed;
            
            // JUMP inputs:
            
            if(_isGrounded)
                _jumpTimer -= Time.deltaTime;
            
            // Player jumped:
            if (_jumpPress && _jumpTimer <= 0.0f){
                _state = playerMoveState.Jump;
                _jumpTimer = _jumpDelay;
                _maxAirTimer = _maxAirTime;
            }

            // Player is in air:
            if (_state == playerMoveState.Jump){
                _maxAirTimer -= Time.deltaTime;
                if (_maxAirTimer < 0.0f){
                    _state = playerMoveState.ReleaseJump;
                }
            }
            
            // Player releases jump:
            if (_jumpRelease && _state == playerMoveState.Jump)
                _state = playerMoveState.ReleaseJump;
        }
        
        // NOTE: This function is for debugging purposes:
        public void Print(){
            
            Debug.Log("Landed");
        }
    }

    internal enum playerMoveState{
        Idle, Walking, Running, Jump, ReleaseJump, AirControl 
    }
}
