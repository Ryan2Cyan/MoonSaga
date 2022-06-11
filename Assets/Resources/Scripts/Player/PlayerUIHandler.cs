using System.Collections.Generic;
using Resources.Scripts.General;
using Resources.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This script handles all UI in relation to the player:
namespace Resources.Scripts.Player{
    public class PlayerUIHandler : MonoBehaviour
    {
        // Scripts:
        private ShadowMeter _shadowMeterScript;
        private GameData _gameDataScript;
        
        // Shadow meter:
        [SerializeField] private Slider _shadowSlider;
        [SerializeField] private Slider _subtractSlider;
        private float _maxValue = 100;
        private const float _minValue = 0;
        [SerializeField] private float _sliderDecrementDelay = 1.5f;
        private float _sliderDecrementTimer;
        internal bool _delay;
        
        // Hit points:
        [SerializeField] private GameObject _hitPointsParent;
        [SerializeField] private List<Image> _hitPointUI;
        [SerializeField] private float _hitPointSubtractDelay = 0.1f;
        private float _hitPointSubtractTimer;
        
        // Shadow sapphires:
        [SerializeField] private TextMeshProUGUI _totalCounter;
        [SerializeField] private TextMeshProUGUI _addCounter;
        [SerializeField] private float _addDelay;
        [SerializeField] private float _addDelayTimer;
        [SerializeField] private float _addCounterTime;
        [SerializeField] private float _addCounterTimer;
        private bool _deactivateAdd;
        [SerializeField] private int _addValue;
        [SerializeField] private int _totalValue;


        private void Awake(){
            
            // Fetch components:
            _shadowMeterScript = GetComponent<ShadowMeter>();
            _gameDataScript = GameObject.Find("Data-Manager").GetComponent<GameData>();
            
            // Set values for sliders:
            _maxValue = _shadowMeterScript._maxShadow;
            UtilityFunctions.SetSliderF(ref _shadowSlider, _minValue, _maxValue, _minValue);
            UtilityFunctions.SetSliderF(ref _subtractSlider, _minValue, _maxValue, _minValue);
            _sliderDecrementTimer = 0f;

            // For each hit point, add UI element:
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
            // Update shadow sapphires:
            UpdateShadowSapphireUI();
        }

        public void DecrementShadowSlider(float value){
            _subtractSlider.value = _shadowSlider.value;
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
                    _subtractSlider.value -= 1f;
                else if (_subtractSlider.value < _shadowSlider.value)
                    _subtractSlider.value = 0f;
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

        public void IncrementShadowSapphires(){
            
            // If add counter is not visible, fade in:
            StopAllCoroutines();
            StartCoroutine(MonoBehaviourUtility.FadeColorTMP(
                1.0f,
                new Color(_addCounter.color.r, _addCounter.color.g, _addCounter.color.b, 1f),
                _addCounter));

            // Increment values:
            _gameDataScript.shadowSapphires++;
            _addValue++;
            _addCounterTimer = _addCounterTime;
            _addDelayTimer = _addDelay;
        }

        private void UpdateShadowSapphireUI(){
            
            // Update sliders:
            _addCounter.text = "+" + _addValue;
            _totalCounter.text = _totalValue.ToString();

            _addDelayTimer -= Time.deltaTime;

            if (_addDelayTimer <= 0f){
                // Check if the add counter is active:
                if (_addCounter.gameObject.activeInHierarchy && _addValue > 0){
                    _addCounterTimer -= Time.deltaTime;
                    if (_addCounterTimer <= 0f){
                        _addValue--;
                        _totalValue++;
                        _addCounterTimer = _addCounterTime;
                        if (_addValue == 0){
                            _deactivateAdd = true;
                            _addDelayTimer = _addDelay;
                        }
                    }
                }
                
                // Fade out add bar when no longer in use:
                if (_deactivateAdd){
                    StopAllCoroutines();
                    StartCoroutine(MonoBehaviourUtility.FadeColorTMP(
                        1.0f,
                        new Color(_addCounter.color.r, _addCounter.color.g, _addCounter.color.b, 0f),
                        _addCounter));
                    _deactivateAdd = false;
                }
            }
        }
    }
}

