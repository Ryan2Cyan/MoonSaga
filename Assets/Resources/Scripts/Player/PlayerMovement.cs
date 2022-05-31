using System;
using Resources.Scripts.General;
using UnityEngine;
using UnityEngine.Events;

// Code within this class is responsible (only) for the movement of the 
// player character.
namespace Resources.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour{
        
        [SerializeField] internal playerMoveState _state = playerMoveState.Idle;
        [SerializeField] private ShadowMeter _shadowMeterScript;
        [Range(0, 1000.0f)] [SerializeField] private float _jumpForce = 100f;
        [Range(0, 100f)] [SerializeField] private float _runSpeed = 37.5f;
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _ceilingCheck;
        [SerializeField] private Transform _groundCheck;
        private const float _groundedRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private float _horizontalInput;


        private const float _landDelay = 0.1f;
        private float _landTimer = _landDelay;
        private const float _maxAirTime = 0.5f;
        private float _airTimer;
        [SerializeField] private bool _isGrounded;
        [SerializeField] internal bool _isFacingRight = true;
        [SerializeField] private bool _jumpPress;
        [SerializeField] private bool _jumpRelease;
        public UnityEvent OnLandEvent;

        private ActionMap _actionMapScript;
        


        private void Awake(){
            
            // Fetch components:
            _rigidbody2D = GetComponent<Rigidbody2D>();
            // Generate action map:
            _actionMapScript = new ActionMap();
            _actionMapScript.Enable();
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
            _jumpPress = _actionMapScript.Player.JumpPress.triggered;
            _jumpRelease = _actionMapScript.Player.JumpRelease.triggered;
            if (_actionMapScript.Player.Movement.ReadValue<Vector2>().x > 0f)
                _horizontalInput = 1f * _runSpeed;
            
            else if (_actionMapScript.Player.Movement.ReadValue<Vector2>().x < 0f)
                _horizontalInput = -1f * _runSpeed;
            else
                _horizontalInput = 0f;
        }
        private void ResetInput(){
            _jumpPress = false;
            _jumpRelease = false;
        }
        private void IdleInput(){

            // If the player is moving [Walking]:
            if (_horizontalInput < 0.0f || _horizontalInput > 0.0f && _isGrounded)
                _state = playerMoveState.Walking;

            // If player presses jump button [Jump]:
            if (_jumpPress){
                _state = playerMoveState.Jump;
                _airTimer = _maxAirTime;
            }
        }
        private void IdleMovement(){
            ApplyNormMovement(8.0f);
        }
        private void WalkingInput(){

            // If the player isn't moving [Idle]:
            if (_horizontalInput == 0.0f && _isGrounded)
                _state = playerMoveState.Idle;
            
            // If player presses jump button [Jump]:
            if (_jumpPress){
                _state = playerMoveState.Jump;
                _airTimer = _maxAirTime;
            }
        }
        private void WalkingMovement(){
            
            // Calc movement from input:
            ApplyNormMovement(8.0f);
        }
        private void JumpInput(){
            
            _airTimer -= Time.deltaTime;
            
            // If jump time expires [Air Control]:
            if (_airTimer < 0.0f){
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y / 2.0f);
                _state = playerMoveState.AirControl;
            }
            
            // Player releases jump:
            if (_jumpRelease && !_isGrounded){
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y / 2.0f);
                _state = playerMoveState.AirControl;
            }
        }
        private void JumpMovement(){
            // Apply force when jumping:
            if(_isGrounded)
                _rigidbody2D.AddForce(new Vector2(_rigidbody2D.velocity.x, _jumpForce));
            ApplyNormMovement(7.0f);
        }
        private void AirControlInput(){
            
            // If player touches the ground [Land]:
            if (_isGrounded){
                _state = playerMoveState.Land;
                // If player in light, spawn light leaves:
                if (_shadowMeterScript._lightDetectionScript._inLight){
                    Instantiate(UnityEngine.Resources.Load<GameObject>
                            ("Prefabs/Environment/CelestialGrove/PFX/Land-Leaves-Light"),
                        _groundCheck.position,
                        Quaternion.identity);
                }
                // If player in light, spawn shadow leaves:
                else{
                    Instantiate(UnityEngine.Resources.Load<GameObject>
                            ("Prefabs/Environment/CelestialGrove/PFX/Land-Leaves"),
                        _groundCheck.position,
                        Quaternion.identity);
                }
            }
        }
        private void AirControlMovement(){
            ApplyNormMovement(7.0f);
        }
        private void LandInput(){
            _landTimer -= Time.deltaTime;

            // Amount of time the player can be in the Land state is finished:
            if (_landTimer <= 0.0f){
                _state = playerMoveState.Idle;
                _landTimer = _landDelay;
            }

            // If the player is moving [Walking]:
            if (_horizontalInput < 0.0f || _horizontalInput > 0.0f && _isGrounded)
                _state = playerMoveState.Walking;

            // If player presses jump button [Jump]:
            if (_jumpPress){
                _state = playerMoveState.Jump;
                _airTimer = _maxAirTime;
            }
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
                case playerMoveState.Land:
                    LandInput();
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
                case playerMoveState.Land:
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
        
    }

    internal enum playerMoveState{
        Idle, Walking, Jump, AirControl, Land
    }
}
