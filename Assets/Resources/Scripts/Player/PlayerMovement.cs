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
        private Rigidbody2D _rigidbody2D;
        
        // Scripts:
        [SerializeField] private ShadowMeter _shadowMeterScript;
        [SerializeField] private PlayerCollision _playerCollisionScript;
        private ActionMap _actionMapScript;
        
        // Movement Values
        [Range(0, 1000.0f)] [SerializeField] private float _jumpForce = 100f;
        [Range(0, 100.0f)] [SerializeField] private float _dashSpeed = 100f;
        [Range(0, 100f)] [SerializeField] private float _runSpeed = 37.5f;
        [Range(0, 30.0f)] [SerializeField] private float _knockBackX = 15f;
        [Range(0, 30.0f)] [SerializeField] private float _knockBackY = 15f;
        [SerializeField] private float _dashDuration = 0.2f;
        [SerializeField] private float _dashDelay = 0.1f;
        [SerializeField] private float _knockBackDelay = 0.1f;

        // Orientation:
        [SerializeField] internal bool _isFacingRight = true;
        // Inputs:
        private float _horizontalInput;
        private bool _jumpPress;
        private bool _jumpRelease;
        private bool _dashPress;
        
        // Air Control Values:
        private const float _maxAirTime = 0.5f;
        private float _airTimer;
        // Land Values:
        private const float _landDelay = 0.1f;
        private float _landTimer = _landDelay;
        // Dash Values:
        [SerializeField] private bool _dashAvailable;
        private float _dashTimer;
        private float _dashDelayTimer;
        // Knock back values:
        private float _knockBackTimer;
        
        public UnityEvent OnLandEvent;

        


        private void Awake(){
            
            // Set values:
            _dashAvailable = true;
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
            ProcessStateMovement();
        }
        
        
        // State Functions:
        private void IdleInput(){

            WalkCheck();
            JumpCheck();
            DashCheck();
        }
        private void IdleMovement(){
            ApplyNormMovement(8.0f);
        }
        private void WalkingInput(){

            IdleCheck();
            JumpCheck();
            DashCheck();
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
            if (_jumpRelease && !_playerCollisionScript._isGrounded){
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y / 2.0f);
                _state = playerMoveState.AirControl;
            }

            DashCheck();
        }
        private void JumpMovement(){
            
            // Apply force when jumping:
            if(_playerCollisionScript._isGrounded)
                _rigidbody2D.AddForce(new Vector2(_rigidbody2D.velocity.x, _jumpForce));
            ApplyNormMovement(7.0f);
        }
        private void AirControlInput(){
            
            // If player touches the ground [Land]:
            if (_playerCollisionScript._isGrounded){
                _state = playerMoveState.Land;
                // If player in light, spawn light leaves:
                if (_shadowMeterScript._lightDetectionScript._inLight){
                    Instantiate(UnityEngine.Resources.Load<GameObject>
                            ("Prefabs/Environment/CelestialGrove/PFX/Land-Leaves-Light"),
                        _playerCollisionScript._groundCheck.position,
                        Quaternion.identity);
                }
                // If player in light, spawn shadow leaves:
                else{
                    Instantiate(UnityEngine.Resources.Load<GameObject>
                            ("Prefabs/Environment/CelestialGrove/PFX/Land-Leaves"),
                        _playerCollisionScript._groundCheck.position,
                        Quaternion.identity);
                }
            }

            DashCheck();
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

            _dashAvailable = true; // Make dash available when on ground
            WalkCheck();
            JumpCheck();
        }
        private void DashInput(){

            // Check if the dash has ended:
            if (_dashTimer < 0f)
                SetDefaultState();
            
            // Check if the player hit an enemy:
            if (_playerCollisionScript._enemyCollision){
                _knockBackTimer = _knockBackDelay;
                _state = playerMoveState.DashHit;
            }

        }
        private void DashMovement(){
            _dashTimer -= Time.deltaTime;
            
            // Apply horizontal force depending on the player's facing direction:
            _rigidbody2D.velocity = _isFacingRight switch{
                true => // Dash right
                    new Vector2(_dashSpeed, 0f),
                false => // Dash left
                    new Vector2(-_dashSpeed, 0f)
            };
        }
        private void DashHitInput(){
            
            // On hit, the player can dash again:
            _dashAvailable = true;
            
            // Knock back timer:
            _knockBackTimer -= Time.deltaTime;
            if(_knockBackTimer <= 0f)
                SetDefaultState();
        }
        private void DashHitMovement(){
            // Knock back the player:
            _rigidbody2D.velocity = 
                _isFacingRight ? new Vector2(-_knockBackX, _knockBackY) : new Vector2(_knockBackX, _knockBackY);
            
        }
        
        // Process state functions:
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
                case playerMoveState.Dash:
                    DashInput();
                    break;
                case playerMoveState.DashHit:
                    DashHitInput();
                    break;
                case playerMoveState.Damaged:
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
                case playerMoveState.Dash:
                    DashMovement();
                    break;
                case playerMoveState.DashHit:
                    DashHitMovement();
                    break;
                case playerMoveState.Damaged:
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
            Vector2 targetVelocity = new Vector2(direction * movementSpeed, _rigidbody2D.velocity.y);
            
            // Apply smoothing (different for acceleration and deceleration):
            // _rigidbody2D.velocity = Vector2.SmoothDamp(_rigidbody2D.velocity, targetVelocity,
            //     ref _velocity, _accelerationSpeed);
            _rigidbody2D.velocity = targetVelocity;
        }
        private void ApplyRecoverMovement(float movementSpeed){
            
            // Calculate direction:
            float direction = _horizontalInput * Time.fixedDeltaTime;
            
            // Check if the player needs to be flipped depending on move direction:
            if (direction > 0.0f && !_isFacingRight) // Flip Right
                transform.localScale = UtilityFunctions.Flip(transform.localScale, ref _isFacingRight);
            
            else if (direction < 0.0f && _isFacingRight) // Flip Left
                transform.localScale = UtilityFunctions.Flip(transform.localScale, ref _isFacingRight);
            
            // Move the character via target velocity:
            Vector2 targetVelocity = new Vector2(direction * movementSpeed, _rigidbody2D.velocity.y);
            
            // Apply smoothing (different for acceleration and deceleration):
            // _rigidbody2D.velocity = Vector2.SmoothDamp(_rigidbody2D.velocity, targetVelocity,
            //     ref _velocity, _accelerationSpeed);
            _rigidbody2D.velocity += targetVelocity;
        }
        private void ProcessInput(){
            
            // Inputs Variables:
            _jumpPress = _actionMapScript.Player.JumpPress.triggered;
            _jumpRelease = _actionMapScript.Player.JumpRelease.triggered;
            _dashPress = _actionMapScript.Player.Dash.triggered;
            
            // Process movement:
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
            _dashPress = false;
        }

        // Input checks for switching state:
        private void SetDefaultState(){
            _state = _playerCollisionScript._isGrounded switch{
                true => playerMoveState.Idle,
                false => playerMoveState.AirControl
            };
        }
        private void IdleCheck(){
            
            // Check if the player is moving on the x-axis [Walking]:
            if (_horizontalInput == 0.0f && _playerCollisionScript._isGrounded)
                _state = playerMoveState.Idle;
        }
        private void WalkCheck(){
            
            // Check if the player is moving on the x-axis [Walking]:
            if (_horizontalInput < 0.0f || _horizontalInput > 0.0f && _playerCollisionScript._isGrounded)
                _state = playerMoveState.Walking;
        }
        private void JumpCheck(){
            // If player presses jump button [Jump]:
            if (_jumpPress){
                _state = playerMoveState.Jump;
                _airTimer = _maxAirTime;
            }
        }
        private void DashCheck(){
            
            // If player presses dash button [Dash]:
            if (!_playerCollisionScript._isGrounded){
                if (_dashPress && _dashAvailable){
                    _state = playerMoveState.Dash;
                    _dashTimer = _dashDuration;
                    _dashAvailable = false;
                }
            }
            else if (_playerCollisionScript._isGrounded){
                _dashDelayTimer -= Time.deltaTime;
                if (_dashPress && _dashDelayTimer < 0f){
                    _state = playerMoveState.Dash;
                    _dashTimer = _dashDuration;
                    _dashDelayTimer = _dashDelay;
                }
            }
        }
        
    }

    internal enum playerMoveState{
        Idle, Walking, Jump, AirControl, Land, Dash, DashHit, Damaged
    }
}
