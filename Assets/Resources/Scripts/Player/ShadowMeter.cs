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
            
            // Increment shadow meter over time, if it is not being decremented:
            if(_playerMovementScript._state != playerMoveState.Dash &&
               _playerMovementScript._state != playerMoveState.DashHit)
                IncrementShadowMeter();
            
            // Clamp shadow value:
            if (_shadowMeter > _maxShadow)
                _shadowMeter = _maxShadow;
            if (_shadowMeter < 0f)
                _shadowMeter = 0f;
        }
        private void IncrementShadowMeter(){

            // Decrease timer:
            _incrementTimer -= Time.deltaTime;

            // Player in light:
            if (_lightDetectionScript._inLight && _incrementTimer <= 0.0f){
                _shadowMeter += _lightValue;
                _incrementTimer = _incrementDelayLight;
            }
            // Player in shadow:
            else if (_incrementTimer <= 0.0f){
                _shadowMeter += _shadowValue;
                _incrementTimer = _incrementDelayShadow;
            }
        }
        internal void DecrementShadowMeter(float value){
            _shadowMeter -= value;
            _playerUIHandlerScript.DecrementShadowSlider(value);
            _playerUIHandlerScript._delay = true;
        }
    }
}
