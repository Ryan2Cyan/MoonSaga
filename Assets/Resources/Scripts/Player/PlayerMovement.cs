using System;
using Resources.Scripts.General;
using UnityEngine;

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
        [SerializeField] private MonoBehaviourUtility _monoBehaviourUtilityScript;
        [SerializeField] private PlayerPFXSpawner _playerPfxSpawnerScript;
        [SerializeField] private GroundCheck _groundCheckScript;
        private ActionMap _actionMapScript;

        // Movement Values
        [Range(0, 1000.0f)] [SerializeField] private float _jumpForce = 100f;
        [Range(0, 1000.0f)] [SerializeField] private float _doubleJumpForce = 100f;
        [Range(0, 100.0f)] [SerializeField] private float _dashSpeed = 100f;
        [Range(0, 100f)] [SerializeField] private float _runSpeed = 37.5f;
        [Range(0, 30.0f)] [SerializeField] private float _dashKnockBackX = 15f;
        [Range(0, 30.0f)] [SerializeField] private float _dashKnockBackY = 15f;
        [Range(0, 30.0f)] [SerializeField] private float _damagedKnockBackX = 15f;
        [Range(0, 30.0f)] [SerializeField] private float _damagedKnockBackY = 15f;
        [SerializeField] private float _dashDuration = 0.2f;
        [SerializeField] private float _dashDelay = 0.1f;
        [SerializeField] private float _dashKnockBackDelay = 0.1f;
        [SerializeField] private float _damagedKnockBackDelay = 0.1f;
        [Range(0, 1.0f)] [SerializeField] private float _maxAirTime = 0.5f;
        [Range(0, 1.0f)] [SerializeField] private float _doubleJumpDelay = 0.15f;

        // Attack costs:
        [Range(0, 100)] [SerializeField] private int _dashCost = 20;
        [Range(0, 100)] [SerializeField] private int _doubleJumpCost = 10;

        // Orientation:
        [SerializeField] internal bool _isFacingRight = true;

        // Inputs:
        private float _horizontalInput;
        private bool _jumpPress;
        private bool _jumpRelease;
        private bool _dashPress;
        private bool _diveBouncePress;

        // Air control values:
        private float _airTimer;

        // Land values:
        private const float _landDelay = 0.1f;

        private float _landTimer = _landDelay;

        // Dash values:
        [SerializeField] private bool _dashAvailable;
        private float _dashTimer;

        private float _dashDelayTimer;

        // Knock back values:
        private float _knockBackTimer;

        // Double jump values:
        [SerializeField] private bool _doubleJumpAvailable;
        
        // Bounce dive values:
        [SerializeField] private float _bounceAirTimer;
        [SerializeField] private float _bounceAirTime;



        private void Awake(){

            // Set values:
            _dashAvailable = true;
            _doubleJumpAvailable = true;
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
            DamagedCheck();
        }
        private void IdleMovement(){
            ApplyNormMovement(8.0f);
        }
        private void WalkingInput(){

            IdleCheck();
            JumpCheck();
            DashCheck();
            DamagedCheck();
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
            if (_jumpRelease && !_groundCheckScript._isGrounded){
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y / 2.0f);
                _state = playerMoveState.AirControl;
            }

            DashCheck();
            DamagedCheck();
            BounceDiveCheck();
        }
        private void JumpMovement(){

            // Apply force when jumping:
            if (_groundCheckScript._isGrounded)
                _rigidbody2D.AddForce(new Vector2(_rigidbody2D.velocity.x, _jumpForce));
            ApplyNormMovement(7.0f);
        }
        private void AirControlInput(){

            // If player touches the ground [Land]:
            if (_groundCheckScript._isGrounded){
                _state = playerMoveState.Land;
                
                // Spawn pfx:
                _playerPfxSpawnerScript.SpawnLandPfx();
            }

            DoubleJumpCheck();
            DashCheck();
            DamagedCheck();
            BounceDiveCheck();
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

            // Make double jump & dash available when on the ground:
            _doubleJumpAvailable = true;
            _dashAvailable = true;
            
            WalkCheck();
            JumpCheck();
            DamagedCheck();
        }
        private void DashInput(){

            // Check if the dash has ended:
            if (_dashTimer < 0f)
                SetDefaultState();

            // Check if the player hit an enemy:
            DashHitCheck();

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

            // On hit, the player can dash & double jump again:
            _doubleJumpAvailable = true;
            _dashAvailable = true;

            // Knock back timer:
            _knockBackTimer -= Time.deltaTime;
            if (_knockBackTimer <= 0f)
                SetDefaultState();
        }
        private void DashHitMovement(){
            // Knock back the player:
            _rigidbody2D.velocity =
                _isFacingRight ? new Vector2(-_dashKnockBackX, _dashKnockBackY) : 
                    new Vector2(_dashKnockBackX, _dashKnockBackY);

        }
        private void DoubleJumpInput(){

            _airTimer -= Time.deltaTime;

            // If jump time expires [Air Control]:
            if (_airTimer < 0f){
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y / 2.0f);
                _state = playerMoveState.AirControl;
            }

            DamagedCheck();
        }
        private void DoubleJumpMovement(){

            // Apply force when jumping:
            if(_airTimer <= 0.1f)
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _doubleJumpForce);
            ApplyNormMovement(7.0f);
        }
        private void DamagedInput(){
            
            // Knock back timer:
            _knockBackTimer -= Time.deltaTime;
            if (_knockBackTimer <= 0f)
                SetDefaultState();
        }
        private void DamagedMovement(){
            // Knock back the player:
            _rigidbody2D.velocity =
                _isFacingRight ? new Vector2(-_dashKnockBackX, _dashKnockBackY) : 
                    new Vector2(_damagedKnockBackX, _damagedKnockBackY);
        }
        private void BounceDiveInput(){

            _playerCollisionScript._boxCollider.enabled = true;
            // If the player hits the ground [Land]:
            if (_groundCheckScript._isGrounded)
                _state = playerMoveState.Land;
            
            
            // If player collides with enemy [BounceDiveHit]:
            if (_playerCollisionScript._enemyCollision){
                _state = playerMoveState.BounceDiveHit;
                _rigidbody2D.velocity = new Vector2(0f, 0f);
            }

            DoubleJumpCheck();
        }
        private void BounceDiveMovement(){
            
            // Clamp velocity to prevent high upward force:
            if (_rigidbody2D.velocity.y >= 30f){
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 30f);
            }

            ApplyNormMovement(8.0f);
        }
        private void BounceDiveHitInput(){

            _state = playerMoveState.BounceDive;
            _bounceAirTimer = _bounceAirTime;
            _doubleJumpAvailable = true;
            _dashAvailable = true;

            _bounceAirTimer -= Time.deltaTime;
        }
        private void BounceDiveHitMovement(){
            
            // Apply bounce force:
            
            _rigidbody2D.AddForce(new Vector2(_rigidbody2D.velocity.x, _jumpForce * 4f));
            _playerCollisionScript._boxCollider.enabled = false;
            
            ApplyNormMovement(8.0f);
        }

        // Process state functions:
        private void ProcessStateInput(){
            switch (_state){
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
                    DamagedInput();
                    break;
                case playerMoveState.DoubleJump:
                    DoubleJumpInput();
                    break;
                case playerMoveState.BounceDive:
                    BounceDiveInput();
                    break;
                case playerMoveState.BounceDiveHit:
                    BounceDiveHitInput();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessStateMovement(){
            switch (_state){
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
                    DamagedMovement();
                    break;
                case playerMoveState.DoubleJump:
                    DoubleJumpMovement();
                    break;
                case playerMoveState.BounceDive:
                    BounceDiveMovement();
                    break;
                case playerMoveState.BounceDiveHit:
                    BounceDiveHitMovement();
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
            
            _rigidbody2D.velocity = targetVelocity;
        }

        private void ProcessInput(){

            // Inputs Variables:
            _jumpPress = _actionMapScript.Player.JumpPress.triggered;
            _jumpRelease = _actionMapScript.Player.JumpRelease.triggered;
            _dashPress = _actionMapScript.Player.Dash.triggered;
            _diveBouncePress = _actionMapScript.Player.DiveBounce.triggered;

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
            _diveBouncePress = false;
        }

        // Input checks for switching state:
        private void SetDefaultState(){
            _state = _groundCheckScript._isGrounded switch{
                true => playerMoveState.Idle,
                false => playerMoveState.AirControl
            };
        }
        private void IdleCheck(){

            // Check if the player is moving on the x-axis [Walking]:
            if (_horizontalInput == 0.0f && _groundCheckScript._isGrounded)
                _state = playerMoveState.Idle;
        }
        private void WalkCheck(){

            // Check if the player is moving on the x-axis [Walking]:
            if (_horizontalInput < 0.0f || _horizontalInput > 0.0f && _groundCheckScript._isGrounded)
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

            // Check if the player has enough shadow meter:
            if (_shadowMeterScript._shadowMeter >= _dashCost){
                switch (_groundCheckScript._isGrounded){
                    // Dash while in-air:
                    case false:{
                        if (_dashPress && _dashAvailable){
                            _state = playerMoveState.Dash;
                            _dashTimer = _dashDuration;
                            _dashAvailable = false;
                            _shadowMeterScript.DecrementShadowMeter(_dashCost);
                            
                            // Spawn pfx:
                            _playerPfxSpawnerScript.SpawnDashPfx();
                        }

                        break;
                    }
                    case true:{
                        // Dash while grounded:
                        _dashDelayTimer -= Time.deltaTime;
                        if (_dashPress && _dashDelayTimer < 0f){
                            _state = playerMoveState.Dash;
                            _dashTimer = _dashDuration;
                            _dashDelayTimer = _dashDelay;
                            _shadowMeterScript.DecrementShadowMeter(_dashCost);
                            
                            // Spawn pfx:
                            _playerPfxSpawnerScript.SpawnDashPfx();
                        }

                        break;
                    }
                }
            }
        }
        private void DoubleJumpCheck(){

            // Check if the player has enough shadow meter:
            if (_shadowMeterScript._shadowMeter >= _doubleJumpCost){
                // Check if player presses jump input:
                if (_jumpPress && _doubleJumpAvailable){
                    _state = playerMoveState.DoubleJump;
                    _shadowMeterScript.DecrementShadowMeter(_doubleJumpCost);
                    _doubleJumpAvailable = false;
                    _airTimer = _doubleJumpDelay;
                    
                    // Spawn pfx:
                    _playerPfxSpawnerScript.SpawnDoubleJumpPfx();
                }
            }
        }
        private void DashHitCheck(){
            // Check if the player hit an enemy:
            if (_playerCollisionScript._enemyCollision){
                _knockBackTimer = _dashKnockBackDelay;
                _state = playerMoveState.DashHit;
                _monoBehaviourUtilityScript.StartSleep(0.05f);
            }
        }
        private void DamagedCheck(){
            // Check if the player hit an enemy:
            if (_playerCollisionScript._enemyCollision){
                _knockBackTimer = _damagedKnockBackDelay;
                _state = playerMoveState.Damaged;
                _monoBehaviourUtilityScript.StartSleep(0.2f);
                Debug.Log("Damage");
            }
        }
        private void BounceDiveCheck(){
            // Check if the player hit an enemy:
            if (_diveBouncePress){
                _bounceAirTimer = _bounceAirTime;
                _state = playerMoveState.BounceDive;
            }
        }
    }

    internal enum playerMoveState{
        Idle, Walking, Jump, DoubleJump, AirControl, Land, Dash, DashHit, Damaged, BounceDive, BounceDiveHit
    }
}
