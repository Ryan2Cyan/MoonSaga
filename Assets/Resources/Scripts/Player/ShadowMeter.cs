using System.Collections.Generic;
using Resources.Scripts.Lighting;
using Resources.Scripts.VFX;
using UnityEngine;
using UnityEngine.UI;
using UtilityFunctions = Resources.Scripts.General.UtilityFunctions;

namespace Resources.Scripts.Player{
    public class ShadowMeter : MonoBehaviour{
        
        [SerializeField] internal LightDetection _lightDetectionScript; 
        [SerializeField] private Slider _shadowSlider;

        [SerializeField] private int _shadowValue = 1;
        [SerializeField] private int _lightValue = 1;
        private const int _maxValue = 100;
        private const int _minValue = 0;
        [SerializeField] private float _incrementDelay = 0.1f;
        private float _incrementTimer;

        private void Awake(){
            
            // Set values:
            UtilityFunctions.SetSlider(ref _shadowSlider, _minValue, _maxValue, _minValue);
        }

        private void Update(){
            
            // Update shadow meter:
            UpdateShadowMeter();
        }
        private void UpdateShadowMeter(){

            // Decrease timer:
            _incrementTimer -= Time.deltaTime;

            // Player in light:
            if (_lightDetectionScript._inLight && _incrementTimer <= 0.0f){
                _shadowSlider.value += _lightValue;
                _incrementTimer = _incrementDelay;
            }
            // Player in shadow:
            else if (_incrementTimer <= 0.0f){
                _shadowSlider.value += _shadowValue;
                _incrementTimer = _incrementDelay;
            }
        }
    }
}
