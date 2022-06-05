using System.Collections.Generic;
using Resources.Scripts.General;
using Resources.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

// This script handles all UI in relation to the player:
namespace Resources.Scripts.Player{
    public class PlayerUIHandler : MonoBehaviour
    {
        // Scripts:
        [SerializeField] private ShadowMeter _shadowMeterScript;
        [SerializeField] private GameData _gameDataScript;
        
        // Shadow meter:
        [SerializeField] private Slider _shadowSlider;
        [SerializeField] private Slider _subtractSlider;
        private const int _maxValue = 100;
        private const int _minValue = 0;
        [SerializeField] private float _sliderDecrementDelay = 1.5f;
        private float _sliderDecrementTimer;
        internal bool _delay;
        
        // Hit points:
        [SerializeField] private GameObject _hitPointsParent;
        [SerializeField] private List<Image> _hitPointUI;
        [SerializeField] private float _hitPointSubtractDelay = 0.1f;
        private float _hitPointSubtractTimer;


        private void Awake(){
            
            // Set values:
            UtilityFunctions.SetSlider(ref _shadowSlider, _minValue, _maxValue, _minValue);
            _subtractSlider.value = _shadowSlider.value;
            _sliderDecrementTimer = 0f;

            for (int i = 0; i < _gameDataScript.maxPoints; i++){
                _hitPointUI.Add(_hitPointsParent.transform.GetChild(i).GetComponent<Image>());
            }
        }

        private void Update(){

            _hitPointSubtractTimer -= Time.deltaTime;
            // Update shadow slider:
            _shadowSlider.value = _shadowMeterScript._shadowMeter;
            // Update subtract slider:
            UpdateSubtractSlider();
            // Update hit points UI:
            UpdateHitPoints();
        }

        public void DecrementShadowSlider(int value){
            _shadowSlider.value -= value;
        }
        private void UpdateSubtractSlider(){
            
            _sliderDecrementTimer -= Time.deltaTime;
            // If the player uses a move and subtracts shadow, then delay the white bar:
            if (_delay){
                _sliderDecrementTimer = _sliderDecrementDelay;
                _delay = false;
            }
            // White bar delay, then catch up:
            if (_sliderDecrementTimer <= 0f){
                if (_subtractSlider.value > _shadowSlider.value)
                    _subtractSlider.value -= 1;
                else if (_subtractSlider.value < _shadowSlider.value)
                    _subtractSlider.value += 1;
            }
        }
        public void ReduceHitPoint(){
            _gameDataScript.hitPoints--;
            _hitPointSubtractTimer = _hitPointSubtractDelay;
        }
        private void UpdateHitPoints(){
            for (int i = 0; i < _gameDataScript.maxPoints; i++){
                // Full hit points:
                if (i < _gameDataScript.hitPoints){
                    _hitPointUI[i].sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/Hitpoints/hitpoint-full");
                }
                // Hit point the player just lost:
                else if (i == _gameDataScript.hitPoints && _hitPointSubtractTimer > 0f){
                    _hitPointUI[i].sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/Hitpoints/hitpoint-subtract");
                }
                // Empty hit points:
                else{
                    _hitPointUI[i].sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/Hitpoints/hitpoint-empty");
                }
            }
        }
    }
}

