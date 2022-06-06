using Resources.Scripts.Lighting;
using UnityEngine;

// This script calculates the value of the player's shadow meter, depending on whether
// they are standing in light or dark:
namespace Resources.Scripts.Player{
    public class ShadowMeter : MonoBehaviour{
        
        // Scripts:
        [SerializeField] internal LightDetection _lightDetectionScript;
        [SerializeField] private PlayerUIHandler _playerUIHandlerScript;
        [SerializeField] internal int _shadowMeter;
        [SerializeField] private float _incrementDelayShadow = 0.1f;
        [SerializeField] private float _incrementDelayLight = 0.5f;
        private float _incrementTimer;
        [SerializeField] private int _shadowValue = 1;
        [SerializeField] private int _lightValue = 1;
        private bool _delay;

        private void Update(){
            
            // Update shadow meter:
            IncrementShadowMeter();
        }
        private void IncrementShadowMeter(){

            // Decrease timer:
            _incrementTimer -= Time.deltaTime;

            if (_shadowMeter >= 0 && _shadowMeter < 100){
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

                // Clamp shadow value:
                if (_shadowMeter > 100)
                    _shadowMeter = 100;
            }
        }
        internal void DecrementShadowMeter(int value){
            _shadowMeter -= value;
            _playerUIHandlerScript.DecrementShadowSlider(value);
            _playerUIHandlerScript._delay = true;
        }
    }
}
