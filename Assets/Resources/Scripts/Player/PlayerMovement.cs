using System;
using Resources.Scripts.General;
using UnityEngine;

// Code within this class is responsible (only) for the movement of the 
// player character.
namespace Resources.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour{

        [SerializeField] internal playerMoveState _state = playerMoveState.Idle;


        // Scripts:
        private ShadowMeter _shadowMeterScript;
        [SerializeField] private PlayerCollision _playerCollisionScript;
        private PlayerData _playerDataScript;
        private PlayerPFXSpawner _playerPfxSpawnerScript;
        [SerializeField] private RadiusChecker _groundCheckScript;
        [SerializeField] private RadiusChecker _ceilingCheckScript;
        private PlayerUIHandler _playerUIHandler;
        private ActionMap _actionMapScript;
        private MonoBehaviourUtility _monoBehaviourUtilityScript;

        // Walking:
        [Range(0, 100f)] [SerializeField] protected float _walkSpeed = 37.5f;
        
        // Jump:
        [Range(0, 1000.0f)] [SerializeField] private float _jumpForce = 100f;
        [Range(0, 1.0f)] [SerializeField] private float _maxAirTime = 0.5f;
        private float _airTimer;
        private bool _inJump;
        
        // Double Jump:
        [SerializeField] private bool _doubleJumpAvailable = true;
        [Range(0, 100)] [SerializeField] private int _doubleJumpCost = 10;
        [Range(0, 1000.0f)] [SerializeField] private float _doubleJumpForce = 100f;
        [Range(0, 1.0f)] [SerializeField] private float _doubleJumpDelay = 0.15f;
        
        // Dash:
        [Range(0, 100.0f)] [SerializeField] private float _dashSpeed = 100f;
        [SerializeField] private float _dashThresholdCost = 20f;
        [Range(0, 100.0f)] [SerializeField] private float _dashDownSpeed = 100f;
        [SerializeField] private Vector2 _dashKnockBack;
        [SerializeField] private float _dashKnockBackDelay = 0.1f;
        [SerializeField] private float _dashShadowDecrement = 0.1f;
        [Range(0f, 10f)] [SerializeField] private float _dashMod = 5f;
       
        
        // Land:
        private const float _landDelay = 0.1f;
        private float _landTimer = _landDelay;
        
        // Damaged:
        [SerializeField] private float _damagedKnockBackDelay = 0.1f;
        private float _knockBackTimer;

        // Orientation:
        [SerializeField] internal bool _isFacingRight = true;

        // Damage values:
        [SerializeField] private float _damageIFrames = 0.5f;
        [SerializeField] internal bool _inIFrames;
        private float _damageIFramesTimer;

        // Inputs:
        private float _horizontalInput;
        private bool _jumpPress;
        private bool _jumpRelease;
        private bool _dashPress;
        private bool _dashRelease;
        private bool _dashDownPress;
        private bool _dashDownRelease;
        
        // Animator property index:
        private static readonly int State = Animator.StringToHash("State");

        private void Awake(){
            
            // Set values:
            _doubleJumpAvailable = true;
            
            // Fetch components:
            _shadowMeterScript = GetComponent<ShadowMeter>();
            // _playerCollisionScript = GetComponent<PlayerCollision>();
            _playerPfxSpawnerScript = GetComponent<PlayerPFXSpawner>();
            _playerUIHandler = GetComponent<PlayerUIHandler>();
            _monoBehaviourUtilityScript = GameObject.Find("Utility").GetComponent<MonoBehaviourUtility>();
            _playerDataScript = GetComponent<PlayerData>();
            
            // Generate action map:
            _actionMapScript = new ActionMap();
            _actionMapScript.Enable();
        }

        private void Update(){

            ProcessInput();
            ProcessStateInput();
            
            // Update player animations:
            _playerDataScript._hoodUpSpriteAnim.SetInteger(State, (int)_state);
            _playerDataScript._hoodDownSpriteAnim.SetInteger(State, (int)_state);
        }
        private void FixedUpdate(){
            ProcessStateMovement();
        }


        // State Functions:
        private void IdleInput(){

            WalkCheck();
            FallCheck();
            JumpCheck();
            DashCheck();
            DamagedCheck();
        }
        private void IdleMovement(){
            ApplyNormMovement(8.0f);
        }
        
        private void WalkingInput(){

            IdleCheck();
            FallCheck();
            JumpCheck();
            DashCheck();
            DamagedCheck();
        }
        private void WalkingMovement(){
            
            ApplyNormMovement(8.0f);
        }
        
        private void JumpInput(){

            _airTimer -= Time.deltaTime;

            // If jump time expires [Air Control]:
            if (_airTimer < 0.0f){
                _playerDataScript._rigidbody2D.velocity = new Vector2(_playerDataScript._rigidbody2D.velocity.x, 
                    _playerDataScript._rigidbody2D.velocity.y / 2.0f);
                _state = playerMoveState.AirControl;
            }

            // Player releases jump [Air Control]:
            if (_jumpRelease && !_groundCheckScript._collided){
                _playerDataScript._rigidbody2D.velocity = new Vector2(_playerDataScript._rigidbody2D.velocity.x, 
                    _playerDataScript._rigidbody2D.velocity.y / 2.0f);
                _state = playerMoveState.AirControl;
            }

            // Player hits ceiling [Air Control]:
            if (_ceilingCheckScript._collided){
                _playerDataScript._rigidbody2D.velocity = new Vector2(_playerDataScript._rigidbody2D.velocity.x, 
                    _playerDataScript._rigidbody2D.velocity.y / 2.0f);
                _state = playerMoveState.AirControl;
            }

            // Set bool to false to prevent another force being applied if player hits another ground:
            if (!_groundCheckScript._collided)
                _inJump = false;

            DashCheck();
            DamagedCheck();
        }
        private void JumpMovement(){

            // Apply force when jumping:
            if (_groundCheckScript._collided && _inJump)
                _playerDataScript._rigidbody2D.AddForce(new Vector2(_playerDataScript._rigidbody2D.velocity.x, _jumpForce));
            
            ApplyNormMovement(7.0f);
        }
        
        private void AirControlInput(){

            // If player touches the ground [Land]:
            if (_groundCheckScript._collided){
                _state = playerMoveState.Land;
                
                // Spawn pfx:
                _playerPfxSpawnerScript.SpawnLandPfx();
            }

            DoubleJumpCheck();
            DashCheck();
            DamagedCheck();
        }
        private void AirControlMovement(){
            
            ApplyNormMovement(8.0f);
        }
        
        private void LandInput(){
            
            _landTimer -= Time.deltaTime;

            // Amount of time the player can be in the Land state is finished:
            if (_landTimer <= 0.0f){
                _landTimer = _landDelay;
                _state = playerMoveState.Idle;
            }

            // Make double jump & dash available when on the ground:
            _doubleJumpAvailable = true;

            WalkCheck();
            JumpCheck();
            DamagedCheck();
        }
        
        private void DashInput(){
            
            // Check if the dash has ended:
            if (_shadowMeterScript._shadowMeter < 0f || _dashRelease)
                SetDefaultState();

            // Check if the player hit an enemy:
            DashHitCheck();
            IFramesCheck();

        }
        private void DashMovement(){

            // Reduce shadow meter:
            _shadowMeterScript.DecrementShadowMeter(_dashShadowDecrement * _dashMod);
            
            // Apply horizontal force depending on the player's facing direction:
            _playerDataScript._rigidbody2D.velocity = _isFacingRight switch{
                true => // Dash right
                    new Vector2(_dashSpeed, 0f),
                false => // Dash left
                    new Vector2(-_dashSpeed, 0f)
            };
        }
        
        private void DashDownInput(){
            
            // Check if the dash has ended:
            if (_shadowMeterScript._shadowMeter < 0f || _dashDownRelease)
                SetDefaultState();
            if (_groundCheckScript._collided){
                _state = playerMoveState.Land;
                _playerPfxSpawnerScript.SpawnLandPfx();
            }

            // Check if the player hit an enemy:
            DashHitCheck();
            IFramesCheck();
        }
        private void DashDownMovement(){
            
            // Reduce shadow meter:
            _shadowMeterScript.DecrementShadowMeter(_dashShadowDecrement * _dashMod);

            // Apply force:
            _playerDataScript._rigidbody2D.velocity = new Vector2(0f, -_dashDownSpeed);
        }
        
        private void DashHitInput(){
            
            // On hit, the player can dash & double jump again:
            _doubleJumpAvailable = true;
            
            // Check for end of dash hit:
            if (_dashRelease || _dashDownRelease || _shadowMeterScript._shadowMeter <= 0f || !
                _playerCollisionScript._enemyCollision){
                _state = playerMoveState.DashRecover;
                _knockBackTimer = _dashKnockBackDelay;
            }

            // Prevent the player from moving:
            _playerDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        private void DashHitMovement(){
            
            // Reduce shadow meter:
            _shadowMeterScript.DecrementShadowMeter(_dashShadowDecrement);
        }
        
        private void DashRecoverInput(){
            _knockBackTimer -= Time.deltaTime;
            _playerDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _playerDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        private void DashRecoverMovement(){
            
                // Knock back the player:
                _playerDataScript._rigidbody2D.velocity =
                    _isFacingRight
                        ? new Vector2(-_dashKnockBack.x, _dashKnockBack.y)
                        : new Vector2(_dashKnockBack.x, _dashKnockBack.y);
                if (_knockBackTimer <= 0f)
                    SetDefaultState();
        }
        
        private void DoubleJumpInput(){

            _airTimer -= Time.deltaTime;

            // If jump time expires [Air Control]:
            if (_airTimer < 0f){
                _playerDataScript._rigidbody2D.velocity = new Vector2(_playerDataScript._rigidbody2D.velocity.x, 
                    _playerDataScript._rigidbody2D.velocity.y / 2.0f);
                _state = playerMoveState.AirControl;
            }

            DamagedCheck();
        }
        private void DoubleJumpMovement(){

            // Apply force when jumping:
            if(_airTimer <= 0.1f)
                _playerDataScript._rigidbody2D.velocity = 
                    new Vector2(_playerDataScript._rigidbody2D.velocity.x, _doubleJumpForce);
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
            _playerDataScript._rigidbody2D.velocity =
                _isFacingRight ? new Vector2(-_dashKnockBack.x, _dashKnockBack.y) : 
                    new Vector2(_dashKnockBack.x, _dashKnockBack.y);
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
                case playerMoveState.DashDown:
                    DashDownInput();
                    break;
                case playerMoveState.DashRecover:
                    DashRecoverInput();
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
                case playerMoveState.DashDown:
                    DashDownMovement();
                    break;
                case playerMoveState.DashRecover:
                    DashRecoverMovement();
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
            Vector2 targetVelocity = new Vector2(direction * movementSpeed, _playerDataScript._rigidbody2D.velocity.y);
            
            _playerDataScript._rigidbody2D.velocity = targetVelocity;
        }

        private void ProcessInput(){

            // Inputs Variables:
            _jumpPress = _actionMapScript.Player.JumpPress.triggered;
            _jumpRelease = _actionMapScript.Player.JumpRelease.triggered;
            _dashPress = _actionMapScript.Player.DashPress.triggered;
            _dashRelease = _actionMapScript.Player.DashRelease.triggered;
            _dashDownPress = _actionMapScript.Player.DashDown.triggered;
            _dashDownRelease = _actionMapScript.Player.DashDownRelease.triggered;
            
            // Process horizontal input:
            if (_actionMapScript.Player.Movement.ReadValue<Vector2>().x > 0f)
                _horizontalInput = 1f * _walkSpeed;

            else if (_actionMapScript.Player.Movement.ReadValue<Vector2>().x < 0f)
                _horizontalInput = -1f * _walkSpeed;
            else
                _horizontalInput = 0f;
        }

        private void ResetInput(){
            _jumpPress = false;
            _jumpRelease = false;
            _dashPress = false;
            _dashRelease = false;
            _dashDownPress = false;
            _dashDownRelease = false;
        }

        // Input checks for switching state:
        private void SetDefaultState(){
            _state = _groundCheckScript._collided switch{
                true => playerMoveState.Idle,
                false => playerMoveState.AirControl
            };
        }
        private void IdleCheck(){

            // Check if the player is moving on the x-axis [Walking]:
            if (_horizontalInput == 0.0f && _groundCheckScript._collided)
                _state = playerMoveState.Idle;
        }
        private void WalkCheck(){

            // Check if the player is moving on the x-axis [Walking]:
            if (_horizontalInput < 0.0f || _horizontalInput > 0.0f && _groundCheckScript._collided)
                _state = playerMoveState.Walking;
        }

        private void FallCheck(){
            if (!_groundCheckScript._collided)
                _state = playerMoveState.AirControl;
        }
        private void JumpCheck(){
            // If player presses jump button [Jump]:
            if (_jumpPress && !_ceilingCheckScript._collided){
                _state = playerMoveState.Jump;
                _airTimer = _maxAirTime;
                _inJump = true;
            }
        }
        private void DashCheck(){
            
            if (_dashPress && _shadowMeterScript._shadowMeter >= _dashThresholdCost){
                _state = playerMoveState.Dash;
                        
                // Spawn pfx:
                _playerPfxSpawnerScript.SpawnDashPfx();
            }

            if (_dashDownPress && _shadowMeterScript._shadowMeter > 0f){
                _state = playerMoveState.DashDown;
                _playerPfxSpawnerScript.SpawnDashDownPfx();
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
                _state = playerMoveState.DashHit;
                _knockBackTimer = _dashKnockBackDelay;
                _monoBehaviourUtilityScript.StartSleep(0.05f);
            }

            HitArmourCheck();
        }
        private void DamagedCheck(){
            
            // Player touched enemy - they take damage:
            if (_playerCollisionScript._enemyCollision && !_inIFrames){
                _knockBackTimer = _damagedKnockBackDelay;
                _state = playerMoveState.Damaged;
                _monoBehaviourUtilityScript.StartSleep(0.2f);
                _playerUIHandler.ReduceHitPoint();
                _damageIFramesTimer = _damageIFrames;
                _inIFrames = true;
                _playerPfxSpawnerScript.SpawnDamagedPfx();
            }

            // Check for i frames:
            IFramesCheck();
        }
        private void HitArmourCheck(){
            
            // Player hit armour, they are deflected:
            if (_playerCollisionScript._enemyArmourCollision){
                _knockBackTimer = _damagedKnockBackDelay;
                _state = playerMoveState.Damaged;
                _playerPfxSpawnerScript.SpawnArmourSparkPfx();
            }
        }
        private void IFramesCheck(){
            
            // Check for i frames:
            _damageIFramesTimer -= Time.deltaTime;
            if (_damageIFramesTimer <= 0f)
                _inIFrames = false;
        }
}

    internal enum playerMoveState{
        Idle = 0, Walking = 1, Jump = 2, DoubleJump = 3, AirControl = 4, Land = 5, Dash = 6, DashHit = 7, 
        DashRecover = 8, Damaged = 9, DashDown = 10
    }
}

