using System;
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
        [Range(0, 1.0f)] [SerializeField] private float _accelerationSpeed = 0.14f;
        [Range(0, 0.3f)] [SerializeField] private float _decelerationSpeed = 0.14f;
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
        [SerializeField] private bool _jumpPress;
        [SerializeField] private bool _jumpRelease;
        public UnityEvent OnLandEvent;
        private Vector2 _velocity = Vector2.zero;
        private Vector2 _lastVelocity = Vector2.zero;


        private void Awake(){
            
            // Fetch components:
            _rigidbody2D = GetComponent<Rigidbody2D>();

            // Set values:
            OnLandEvent ??= new UnityEvent();
        }

        private void Update(){
            
            ProcessInput();
            ProcessStateInput();
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
            
            ProcessStateMovement();
        }
        
        private void ProcessInput(){
            
            // Inputs Variables:
            _jumpPress = Input.GetButtonDown("Jump") || _jumpPress;
            _jumpRelease = Input.GetButtonUp("Jump") || _jumpRelease;
            _horizontalInput = Input.GetAxisRaw("Horizontal") * _runSpeed;
        }
        private void ResetInput(){
            _jumpPress = false;
            _jumpRelease = false;
        }
        
        private void IdleInput(){
            
            // Decrement time till player can jump again:
            if(_isGrounded)
                _jumpTimer -= Time.deltaTime;
            
            // If the player is moving [Walking]:
            if (_horizontalInput < 0.0f || _horizontalInput > 0.0f && _isGrounded)
                _state = playerMoveState.Walking;

            // If player presses jump button [Jump]:
            if (Input.GetButtonDown("Jump")){
                _state = playerMoveState.Jump;
                _maxAirTimer = _maxAirTime;
            }
        }
        private void IdleMovement(){
            ApplyNormMovement(8.0f);
        }
        private void WalkingInput(){
            
            // Decrement time till player can jump again:
            if(_isGrounded)
                _jumpTimer -= Time.deltaTime;
            
            // If the player isn't moving [Idle]:
            if (_horizontalInput == 0.0f && _isGrounded)
                _state = playerMoveState.Idle;
            
            // If player presses jump button [Jump]:
            if (Input.GetButtonDown("Jump") && _jumpTimer <= 0.0f){
                _state = playerMoveState.Jump;
                _jumpTimer = _jumpDelay;
                _maxAirTimer = _maxAirTime;
            }
        }
        private void WalkingMovement(){
            
            ApplyNormMovement(8.0f);
        }
        private void JumpInput(){
            
            _maxAirTimer -= Time.deltaTime;
            
            // If jump time expires [Air Control]:
            if (_maxAirTimer < 0.0f){
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y / 2.0f);
                _state = playerMoveState.AirControl;
            }
            
            // Player releases jump:
            if (_jumpRelease){
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y / 2.0f);
                _state = playerMoveState.AirControl;
            }
        }
        private void JumpMovement(){
            
            // Apply force when jumping:
            if (_isGrounded && _jumpPress)
                _rigidbody2D.AddForce(new Vector2(_rigidbody2D.velocity.x, _jumpForce));
            ApplyNormMovement(3.0f);
        }
        private void AirControlInput(){
            
            // If player touches the ground [Idle]:
            if (_isGrounded && _rigidbody2D.velocity.x == 0.0f)
                _state = playerMoveState.Idle;
            
            // If player touches the ground [Walking]:
            if (_isGrounded && _rigidbody2D.velocity.x != 0.0f)
                _state = playerMoveState.Walking;
        }
        private void AirControlMovement(){
            
            ApplyNormMovement(3.0f);
        }
        private void ProcessStateInput(){
            switch(_state) {
                case playerMoveState.Idle:
                    IdleInput();
                    break;
                case playerMoveState.Walking:
                    WalkingInput();
                    break;
                case playerMoveState.Jump:
                    JumpInput();
                    break;
                case playerMoveState.AirControl:
                    AirControlInput();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void ProcessStateMovement(){
            switch(_state) {
                case playerMoveState.Idle:
                    IdleMovement();
                    break;
                case playerMoveState.Walking:
                    WalkingMovement();
                    break;
                case playerMoveState.Jump:
                    JumpMovement();
                    break;
                case playerMoveState.AirControl:
                    AirControlMovement();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ResetInput();
        }
        private void ApplyNormMovement(float movementSpeed){
            
            // Calculate direction:
            float direction = _horizontalInput * Time.fixedDeltaTime;
            
            // Check if the player needs to be flipped depending on move direction:
            if (direction > 0.0f && !_isFacingRight) // Flip Right
                transform.localScale = UtilityFunctions.Flip(transform.localScale, ref _isFacingRight);
            
            else if (direction < 0.0f && _isFacingRight) // Flip Left
                transform.localScale = UtilityFunctions.Flip(transform.localScale, ref _isFacingRight);
            
            // Move the character via target velocity:
            Vector3 targetVelocity = new Vector2(direction * movementSpeed, _rigidbody2D.velocity.y);
            
            // Apply smoothing (different for acceleration and deceleration):
            // _rigidbody2D.velocity = Vector2.SmoothDamp(_rigidbody2D.velocity, targetVelocity,
            //     ref _velocity, _accelerationSpeed);
            _rigidbody2D.velocity = targetVelocity;
        }
        
        // NOTE: This function is for debugging purposes:
        public void Print(){
            
            Debug.Log("Landed");
        }
    }

    internal enum playerMoveState{
        Idle, Walking, Jump, AirControl 
    }
}
