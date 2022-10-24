using System;
using Resources.Scripts.Camera;
using Resources.Scripts.General;
using Resources.Scripts.Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("State")] 
    public IPlayerState State;
    public bool Debug = true;
    
    [Header("Scripts")]
    private ShadowMeter _shadowMeterScript;
    private CameraShake _cameraShakeScript;
    [SerializeField] private PlayerCollision _playerCollisionScript;
    public PlayerData PlayerDataScript;
    private PlayerPFXSpawner _playerPfxSpawnerScript;
    private PlayerUIHandler _playerUIHandler;
    private MonoBehaviourUtility _monoBehaviourUtilityScript;
    public PlayerInputHandler InputHandler;
    public RadiusChecker GroundCheckScript;
    [SerializeField] private RadiusChecker _ceilingCheckScript;
    
    private void Awake()
    {
        _shadowMeterScript = GetComponent<ShadowMeter>();
        _playerPfxSpawnerScript = GetComponent<PlayerPFXSpawner>();
        _playerUIHandler = GetComponent<PlayerUIHandler>();
        _monoBehaviourUtilityScript = GameObject.Find("Utility").GetComponent<MonoBehaviourUtility>();
        _cameraShakeScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    private void OnEnable()
    {
        State = new Idle();
    }

    private void Update()
    {
        HandleInput();
        State.OnUpdate(this);
    }

    private void FixedUpdate()
    {
        State.OnFixedUpdate(this);
    }

    private void HandleInput()
    {
        IPlayerState state = State.HandleInput(this);
        if (state != null)
        {
            State.OnExit(this);
            State = state;
            State.OnStart(this);
        }
    }
    
    /// <summary>Handles movement on the x-axis, and flips sprite according to direction.</summary>
    public void ApplyXMovement(){

        // Calculate direction:
        float direction = InputHandler.Movement.x * Time.fixedDeltaTime;

        // Check if the player needs to be flipped depending on move direction:
        switch (direction)
        {
            case > 0.0f when !PlayerDataScript._isFacingRight:
            case < 0.0f when PlayerDataScript._isFacingRight:
                transform.localScale = UtilityFunctions.Flip(transform.localScale, ref PlayerDataScript._isFacingRight);
                break;
        }

        // Move the character via target velocity:
        PlayerDataScript._rigidbody2D.velocity = new Vector2(direction * PlayerDataScript._walkSpeed, PlayerDataScript._rigidbody2D.velocity.y);
    }

    // private void FallCheck(){
    //     if (!_groundCheckScript._collided)
    //         _state = playerMoveState.AirControl;
    // }
    // private void JumpCheck(){
    //     
    //     // If player presses jump button [Jump]:
    //     if (_jumpPress && !_ceilingCheckScript._collided){
    //         _state = playerMoveState.Jump;
    //         _playerDataScript._airTimer = _playerDataScript._maxAirTime;
    //         _playerDataScript._isJumping = true;
    //     }
    // }
    // private void DashCheck(){
    //     
    //     if (_dashPress && _shadowMeterScript._shadowMeter >= _playerDataScript._dashThresholdCost){
    //         _playerPfxSpawnerScript.SpawnDashPfx();
    //         _state = playerMoveState.Dash;
    //     }
    //
    //     if (_dashDownPress && _shadowMeterScript._shadowMeter > 0f){
    //         _playerPfxSpawnerScript.SpawnDashDownPfx();
    //         _state = playerMoveState.DashDown;
    //     }
    // }
    // private void DoubleJumpCheck(){
    //
    //     // Check for conditions to double jump:
    //     if (_shadowMeterScript._shadowMeter >= _playerDataScript._doubleJumpCost && _jumpPress &&
    //         _playerDataScript._doubleJumpAvailable ){
    //         _shadowMeterScript.DecrementShadowMeter(_playerDataScript._doubleJumpCost);
    //         _playerDataScript._doubleJumpAvailable = false;
    //         _playerDataScript._airTimer = _playerDataScript._doubleJumpDelay;
    //         _playerPfxSpawnerScript.SpawnDoubleJumpPfx();
    //         _state = playerMoveState.DoubleJump;
    //     }
    // }
    // private void DashHitCheck(){
    //     
    //     // Check if the player hit an enemy:
    //     if (_playerCollisionScript._enemyCollision){
    //         _playerDataScript._doubleJumpAvailable = true;
    //         _playerDataScript._knockBackTimer = _playerDataScript._dashKnockBackDelay;
    //         _monoBehaviourUtilityScript.StartSleep(0.05f);
    //         _cameraShakeScript.StartShake(0.1f, 0.2f);
    //         _playerDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
    //         _state = playerMoveState.DashHit;
    //     }
    //
    //     HitArmourCheck();
    // }
    // private void DamagedCheck(){
    //     
    //     // Player touched enemy - they take damage:
    //     if (_playerCollisionScript._enemyCollision && !_playerDataScript._inIFrames){
    //         _playerDataScript._knockBackTimer = _playerDataScript._damagedKnockBackDelay;
    //         _monoBehaviourUtilityScript.StartSleep(0.2f);
    //         _cameraShakeScript.StartShake(0.1f, 0.4f);
    //         _playerUIHandler.ReduceHitPoint();
    //         _playerDataScript._damageIFramesTimer = _playerDataScript._damageIFrames;
    //         _playerDataScript._inIFrames = true;
    //         _playerPfxSpawnerScript.SpawnDamagedPfx();
    //         _state = playerMoveState.Damaged;
    //     }
    //     
    //     // Player touched active damage - they take damage:
    //     if (_playerCollisionScript._activeDamageCollision && !_playerDataScript._inIFrames){
    //         _playerDataScript._knockBackTimer = _playerDataScript._damagedKnockBackDelay;
    //         _monoBehaviourUtilityScript.StartSleep(0.2f);
    //         _cameraShakeScript.StartShake(0.1f, 0.4f);
    //         _playerUIHandler.ReduceHitPoint();
    //         _playerDataScript._damageIFramesTimer = _playerDataScript._damageIFrames;
    //         _playerDataScript._inIFrames = true;
    //         _playerPfxSpawnerScript.SpawnDamagedPfx();
    //         _state = playerMoveState.Damaged;
    //     }
    //
    //     // Check for i frames:
    //     IFramesCheck();
    // }
}
