using Resources.Scripts.Lighting;
using UnityEngine;
using UnityEngine.UI;
using UtilityFunctions = Resources.Scripts.General.UtilityFunctions;

namespace Resources.Scripts.Player{
    public class ShadowMeter : MonoBehaviour{
        
        [SerializeField] internal LightDetection _lightDetectionScript; 
        [SerializeField] private Slider _shadowSlider;
        [SerializeField] private Slider _subtractSlider;

        [SerializeField] internal int _shadowMeter;
        [SerializeField] private int _shadowValue = 1;
        [SerializeField] private int _lightValue = 1;
        private const int _maxValue = 100;
        private const int _minValue = 0;
        [SerializeField] private float _incrementDelay = 0.1f;
        [SerializeField] private float _decrementDelay = 1.5f;
        private float _incrementTimer;
        [SerializeField] private float _decrementTimer;
        private bool _delay;

        private void Awake(){
            
            // Set values:
            UtilityFunctions.SetSlider(ref _shadowSlider, _minValue, _maxValue, _minValue);
            _subtractSlider.value = _shadowSlider.value;
            _decrementTimer = 0f;
        }
        private void Update(){
            
            // Update shadow meter:
            IncrementShadowMeter();
            // Update subtract meter:
            DecrementSubtractMeter();
        }
        private void IncrementShadowMeter(){

            // Decrease timer:
            _incrementTimer -= Time.deltaTime;

            if (_shadowMeter >= 0 && _shadowMeter < 100){
                // Player in light:
                if (_lightDetectionScript._inLight && _incrementTimer <= 0.0f){
                    _shadowMeter += _lightValue;
                    _incrementTimer = _incrementDelay;
                }
                // Player in shadow:
                else if (_incrementTimer <= 0.0f){
                    _shadowMeter += _shadowValue;
                    _incrementTimer = _incrementDelay;
                }

                // Clamp shadow value:
                if (_shadowMeter > 100)
                    _shadowMeter = 100;

                _shadowSlider.value = _shadowMeter;
            }
        }
        internal void DecrementShadowMeter(int value){
            _shadowMeter -= value;
            _shadowSlider.value -= value;
            _delay = true;
        }
        private void DecrementSubtractMeter(){
            
            _decrementTimer -= Time.deltaTime;
            // If the player uses a move and subtracts shadow, then delay the white bar:
            if (_delay){
                _decrementTimer = _decrementDelay;
                _delay = false;
            }
            // White bar delay, then catch up:
            if (_decrementTimer <= 0f){
                if (_subtractSlider.value > _shadowSlider.value)
                    _subtractSlider.value -= 1;
                else if (_subtractSlider.value < _shadowSlider.value)
                    _subtractSlider.value += 1;
            }
        }
    }
}
