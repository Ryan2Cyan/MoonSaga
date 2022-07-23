using System;
using Resources.Scripts.Camera;
using Resources.Scripts.General;
using UnityEngine;

// Code within this class is a state machine of which controls
// the movement and actions of the player:
namespace Resources.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour{

        // State:
        [SerializeField] internal playerMoveState _state = playerMoveState.Idle;
        
        // Scripts:
        private ShadowMeter _shadowMeterScript;
        private CameraShake _cameraShakeScript;
        [SerializeField] private PlayerCollision _playerCollisionScript;
        private PlayerData _playerDataScript;
        private PlayerPFXSpawner _playerPfxSpawnerScript;
        [SerializeField] private RadiusChecker _groundCheckScript;
        [SerializeField] private RadiusChecker _ceilingCheckScript;
        private PlayerUIHandler _playerUIHandler;
        private ActionMap _actionMapScript;
        private MonoBehaviourUtility _monoBehaviourUtilityScript;

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
            
            // Fetch components:
            _shadowMeterScript = GetComponent<ShadowMeter>();
            // _playerCollisionScript = GetComponent<PlayerCollision>();
            _playerPfxSpawnerScript = GetComponent<PlayerPFXSpawner>();
            _playerUIHandler = GetComponent<PlayerUIHandler>();
            _monoBehaviourUtilityScript = GameObject.Find("Utility").GetComponent<MonoBehaviourUtility>();
            _playerDataScript = GetComponent<PlayerData>();
            _cameraShakeScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
            
            // Generate action map to read user inputs:
            _actionMapScript = new ActionMap();
            _actionMapScript.Enable();
        }
        private void Update(){

            // Check inputs from the user:
            ProcessInput();
            
            // Process inputs depending on player's state:
            ProcessStateInput();
            
            // Update player animations:
            _playerDataScript._hoodUpSpriteAnim.SetInteger(State, (int)_state);
            _playerDataScript._hoodDownSpriteAnim.SetInteger(State, (int)_state);
        }
        private void FixedUpdate(){
            
            // Process all state movements:
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

            // If jump time expires, player releases jump key, or player hits ceiling [Air Control]:
            _playerDataScript._airTimer -= Time.deltaTime;
            if (_playerDataScript._airTimer < 0.0f || _jumpRelease || _ceilingCheckScript._collided){
                StopYAxisMovement();
                SetDefaultState();
            }

            // Set bool to false to prevent another force being applied if player hits another ground:
            if (!_groundCheckScript._collided)
                _playerDataScript._isJumping = false;

            DashCheck();
            DamagedCheck();
        }
        private void JumpMovement(){

            // Apply force when jumping:
            if (_playerDataScript._isJumping)
                _playerDataScript._rigidbody2D.AddForce(
                    new Vector2(_playerDataScript._rigidbody2D.velocity.x, _playerDataScript._jumpForce));
            
            ApplyNormMovement(7.0f);
        }
        
        private void AirControlInput(){

            // If player touches the ground [Land]:
            if (_groundCheckScript._collided){
                _playerDataScript._landTimer = _playerDataScript._landDelay;
                _playerDataScript._doubleJumpAvailable = true;
                _playerPfxSpawnerScript.SpawnLandPfx();
                _state = playerMoveState.Land;
            }

            DoubleJumpCheck();
            DashCheck();
            DamagedCheck();
        }
        private void AirControlMovement(){
            
            ApplyNormMovement(8.0f);
        }
        
        private void LandInput(){

            // Once timer expires [Idle]:
            _playerDataScript._landTimer -= Time.deltaTime;
            if (_playerDataScript._landTimer <= 0.0f)
                SetDefaultState();

            WalkCheck();
            JumpCheck();
            DamagedCheck();
            DashCheck();
        }
        
        private void DashInput(){
            
            // Check if the dash has ended:
            if (_shadowMeterScript._shadowMeter <= 0f || _dashRelease)
                SetDefaultState();

            // Check if the player hit an enemy:
            DashHitCheck();
            IFramesCheck();
        }
        private void DashMovement(){

            // Reduce shadow meter:
            _shadowMeterScript.DecrementShadowMeter(_playerDataScript._dashShadowDecrement);
            
            // Apply horizontal force depending on the player's facing direction:
            _playerDataScript._rigidbody2D.velocity = _playerDataScript._isFacingRight switch{
                true => // Dash right
                    new Vector2(_playerDataScript._dashSpeed, 0f),
                false => // Dash left
                    new Vector2(-_playerDataScript._dashSpeed, 0f)
            };
        }
        
        private void DashDownInput(){
            
            // Check if the dash has ended:
            if (_shadowMeterScript._shadowMeter < 0f || _dashDownRelease)
                _state = playerMoveState.AirControl;
            
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
            _shadowMeterScript.DecrementShadowMeter(_playerDataScript._dashShadowDecrement);

            // Apply force:
            _playerDataScript._rigidbody2D.velocity = new Vector2(0f, -_playerDataScript._dashDownSpeed);
        }
        
        private void DashHitInput(){

            // Check for end of dash hit:
            if (_dashRelease || _dashDownRelease || _shadowMeterScript._shadowMeter <= 0f || !
                _playerCollisionScript._enemyCollision){
                _playerDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.None;
                _playerDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                _playerDataScript._knockBackTimer = _playerDataScript._dashKnockBackDelay;
                _state = playerMoveState.DashRecover;
            }
        }
        private void DashHitMovement(){
            
            // Reduce shadow meter:
            _shadowMeterScript.DecrementShadowMeter(_playerDataScript._dashShadowDecrement);
        }
        
        private void DashRecoverInput(){
            
            // Once timer expires [Idle/AirControl]:
            _playerDataScript._knockBackTimer -= Time.deltaTime;
            if (_playerDataScript._knockBackTimer <= 0f)
                SetDefaultState();
        }
        private void DashRecoverMovement(){
            
            // Knock back the player:
            _playerDataScript._rigidbody2D.velocity =
                _playerDataScript._isFacingRight
                    ? new Vector2(-_playerDataScript._dashKnockBack.x, _playerDataScript._dashKnockBack.y)
                    : new Vector2(_playerDataScript._dashKnockBack.x, _playerDataScript._dashKnockBack.y);
        }
        
        private void DoubleJumpInput(){

            _playerDataScript._airTimer -= Time.deltaTime;

            // If jump time expires [Air Control]:
            if (_playerDataScript._airTimer < 0f){
                StopYAxisMovement();
                _state = playerMoveState.AirControl;
            }

            DamagedCheck();
        }
        private void DoubleJumpMovement(){

            // Apply force when jumping:
            if(_playerDataScript._airTimer <= _playerDataScript._doubleJumpAirTime)
                _playerDataScript._rigidbody2D.velocity = 
                    new Vector2(_playerDataScript._rigidbody2D.velocity.x, _playerDataScript._doubleJumpForce);
            ApplyNormMovement(7.0f);
        }
        
        private void DamagedInput(){
            
            // Knock back timer:
            _playerDataScript._knockBackTimer -= Time.deltaTime;
            if (_playerDataScript._knockBackTimer <= 0f)
                SetDefaultState();
            
            // Make sure the player is unlocked when no longer colliding with enemy:
            _playerDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _playerDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        private void DamagedMovement(){
            
            // Knock back the player:
            _playerDataScript._rigidbody2D.velocity =
                _playerDataScript._isFacingRight ? 
                    new Vector2(-_playerDataScript._dashKnockBack.x, _playerDataScript._dashKnockBack.y) : 
                    new Vector2(_playerDataScript._dashKnockBack.x, _playerDataScript._dashKnockBack.y);
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
                _horizontalInput = 1f * _playerDataScript._walkSpeed;

            else if (_actionMapScript.Player.Movement.ReadValue<Vector2>().x < 0f)
                _horizontalInput = -1f * _playerDataScript._walkSpeed;
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
                _playerDataScript._airTimer = _playerDataScript._maxAirTime;
                _playerDataScript._isJumping = true;
            }
        }
        private void DashCheck(){
            
            if (_dashPress && _shadowMeterScript._shadowMeter >= _playerDataScript._dashThresholdCost){
                _playerPfxSpawnerScript.SpawnDashPfx();
                _state = playerMoveState.Dash;
            }

            if (_dashDownPress && _shadowMeterScript._shadowMeter > 0f){
                _playerPfxSpawnerScript.SpawnDashDownPfx();
                _state = playerMoveState.DashDown;
            }
        }
        private void DoubleJumpCheck(){

            // Check for conditions to double jump:
            if (_shadowMeterScript._shadowMeter >= _playerDataScript._doubleJumpCost && _jumpPress &&
                _playerDataScript._doubleJumpAvailable ){
                _shadowMeterScript.DecrementShadowMeter(_playerDataScript._doubleJumpCost);
                _playerDataScript._doubleJumpAvailable = false;
                _playerDataScript._airTimer = _playerDataScript._doubleJumpDelay;
                _playerPfxSpawnerScript.SpawnDoubleJumpPfx();
                _state = playerMoveState.DoubleJump;
            }
        }
        private void DashHitCheck(){
            
            // Check if the player hit an enemy:
            if (_playerCollisionScript._enemyCollision){
                _playerDataScript._doubleJumpAvailable = true;
                _playerDataScript._knockBackTimer = _playerDataScript._dashKnockBackDelay;
                _monoBehaviourUtilityScript.StartSleep(0.05f);
                _cameraShakeScript.StartShake(0.1f, 0.2f);
                _playerDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                _state = playerMoveState.DashHit;
            }

            HitArmourCheck();
        }
        private void DamagedCheck(){
            
            // Player touched enemy - they take damage:
            if (_playerCollisionScript._enemyCollision && !_playerDataScript._inIFrames){
                _playerDataScript._knockBackTimer = _playerDataScript._damagedKnockBackDelay;
                _monoBehaviourUtilityScript.StartSleep(0.2f);
                _cameraShakeScript.StartShake(0.1f, 0.4f);
                _playerUIHandler.ReduceHitPoint();
                _playerDataScript._damageIFramesTimer = _playerDataScript._damageIFrames;
                _playerDataScript._inIFrames = true;
                _playerPfxSpawnerScript.SpawnDamagedPfx();
                _state = playerMoveState.Damaged;
            }
            
            // Player touched active damage - they take damage:
            if (_playerCollisionScript._activeDamageCollision && !_playerDataScript._inIFrames){
                _playerDataScript._knockBackTimer = _playerDataScript._damagedKnockBackDelay;
                _monoBehaviourUtilityScript.StartSleep(0.2f);
                _cameraShakeScript.StartShake(0.1f, 0.4f);
                _playerUIHandler.ReduceHitPoint();
                _playerDataScript._damageIFramesTimer = _playerDataScript._damageIFrames;
                _playerDataScript._inIFrames = true;
                _playerPfxSpawnerScript.SpawnDamagedPfx();
                _state = playerMoveState.Damaged;
            }

            // Check for i frames:
            IFramesCheck();
        }
        private void HitArmourCheck(){
            
            // Player hit armour, they are deflected:
            if (_playerCollisionScript._enemyArmourCollision){
                _playerDataScript._knockBackTimer = _playerDataScript._damagedKnockBackDelay;
                _cameraShakeScript.StartShake(0.1f, 0.2f);
                _playerPfxSpawnerScript.SpawnArmourSparkPfx();
                _state = playerMoveState.Damaged;
            }
        }
        private void IFramesCheck(){
            
            // Check for i frames:
            _playerDataScript._damageIFramesTimer -= Time.deltaTime;
            if (_playerDataScript._damageIFramesTimer <= 0f)
                _playerDataScript._inIFrames = false;
        }
        
        // Movement functions:
        private void ApplyNormMovement(float movementSpeed){

            // Calculate direction:
            float direction = _horizontalInput * Time.fixedDeltaTime;

            // Check if the player needs to be flipped depending on move direction:
            if (direction > 0.0f && !_playerDataScript._isFacingRight) // Flip Right
                transform.localScale = UtilityFunctions.Flip(transform.localScale, ref _playerDataScript._isFacingRight);
            else if (direction < 0.0f && _playerDataScript._isFacingRight) // Flip Left
                transform.localScale = UtilityFunctions.Flip(transform.localScale, ref _playerDataScript._isFacingRight);

            // Move the character via target velocity:
            _playerDataScript._rigidbody2D.velocity = new Vector2(
                direction * movementSpeed, 
                _playerDataScript._rigidbody2D.velocity.y);
        }
        private void StopYAxisMovement(){
            
            // Set y-axis velocity to half:
            _playerDataScript._rigidbody2D.velocity = new Vector2(
                _playerDataScript._rigidbody2D.velocity.x, 
                _playerDataScript._rigidbody2D.velocity.y / 2.0f);
        }
    }

    internal enum playerMoveState{
        Idle = 0, Walking = 1, Jump = 2, DoubleJump = 3, AirControl = 4, Land = 5, Dash = 6, DashHit = 7, 
        DashRecover = 8, Damaged = 9, DashDown = 10
    }
}

