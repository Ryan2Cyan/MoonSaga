using System;
using Resources.Scripts.Lighting;
using UnityEngine;

// This script calculates the value of the player's shadow meter, depending on whether
// they are standing in light or dark:
namespace Resources.Scripts.Player{
    public class ShadowMeter : MonoBehaviour{
        
        // Scripts:
        private LightDetection _lightDetectionScript;
        private PlayerUIHandler _playerUIHandlerScript;
        private PlayerMovement _playerMovementScript;
        
        // Values:
        [SerializeField] internal float _shadowMeter;
        [SerializeField] internal float _maxShadow;
        [SerializeField] private float _incrementDelayShadow = 0.1f;
        [SerializeField] private float _incrementDelayLight = 0.5f;
        private float _incrementTimer;
        [SerializeField] private int _shadowValue = 1;
        [SerializeField] private int _lightValue = 1;

        private void Awake(){
            
            // Set values:
            _shadowMeter = _maxShadow;
            
            // Fetch components:
            _lightDetectionScript = GetComponent<LightDetection>();
            _playerUIHandlerScript = GetComponent<PlayerUIHandler>();
            _playerMovementScript = GetComponent<PlayerMovement>();
        }

        private void Update(){
            
            // Increment shadow meter over time based on player state:
            switch (_playerMovementScript._state){
                case playerMoveState.Idle:
                    IncrementShadowMeter();
                    break;
                case playerMoveState.Walking:
                    IncrementShadowMeter();
                    break;
                case playerMoveState.Jump:
                    IncrementShadowMeter();
                    break;
                case playerMoveState.DoubleJump:
                    IncrementShadowMeter();
                    break;
                case playerMoveState.AirControl:
                    IncrementShadowMeter();
                    break;
                case playerMoveState.Land:
                    IncrementShadowMeter();
                    break;
                case playerMoveState.Dash:
                    break;
                case playerMoveState.DashHit:
                    break;
                case playerMoveState.DashRecover:
                    IncrementShadowMeter();
                    break;
                case playerMoveState.Damaged:
                    IncrementShadowMeter();
                    break;
                case playerMoveState.DashDown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void IncrementShadowMeter(){
            
            // Once timer expires, increment based on if player is in light or not:
            _incrementTimer -= Time.deltaTime;
            if (_incrementTimer <= 0f){
                _shadowMeter += _lightDetectionScript._inLight ? _lightValue : _shadowValue;
                _incrementTimer = _lightDetectionScript._inLight ? _incrementDelayLight : _incrementDelayShadow;
                
                // Clamp shadow value:
                if (_shadowMeter > _maxShadow)
                    _shadowMeter = _maxShadow;
            }
        }
        internal void DecrementShadowMeter(float value){
            
            _shadowMeter -= value;
            _playerUIHandlerScript.DecrementShadowSlider(value);
            _playerUIHandlerScript._sliderDecrementTimer = _playerUIHandlerScript._sliderDecrementDelay;
            
            // Clamp shadow value:
            if (_shadowMeter < 0f)
                _shadowMeter = 0f;
        }
    }
}
