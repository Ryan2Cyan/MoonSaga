using System.Collections;
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
        private Slider _shadowSlider;
        private Slider _subtractSlider;
        private const float _minValue = 0;
        [SerializeField] internal float _sliderDecrementDelay = 1.5f;
        internal float _sliderDecrementTimer;
        
        // Hit points:
        private GameObject _hitPointsParent;
        private List<Image> _hitPointUI;
        [SerializeField] private float _hitPointSubtractDelay = 0.1f;
        private float _hitPointSubtractTimer;
        
        // Shadow sapphires:
        private GameObject _shadowSapphiresUI;
        private TextMeshProUGUI _totalCounter;
        private TextMeshProUGUI _tempCounter;
        [SerializeField] private float _tempDelay;
        private float _tempDelayTimer;
        [SerializeField] private float _tempSubtractTime;
        private float _tempSubtractTimer; 
        [SerializeField] private int _totalValue;
        private bool _deactivateAdd;
        private int _tempValue;


        private void Awake(){
            
            // Fetch components:
            _shadowMeterScript = GetComponent<ShadowMeter>();
            _shadowSlider = GameObject.Find("Shadow-Slider").GetComponent<Slider>();
            _subtractSlider = GameObject.Find("Subtract-Slider").GetComponent<Slider>();
            _gameDataScript = GameObject.Find("Data-Manager").GetComponent<GameData>();
            _hitPointsParent = GameObject.Find("Hit-Points");
            _shadowSapphiresUI = GameObject.Find("Shadow-Sapphires");
            _totalCounter = _shadowSapphiresUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _tempCounter = _shadowSapphiresUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            
            // Set values for sliders:
            UtilityFunctions.SetSliderF(ref _shadowSlider, _minValue, _shadowMeterScript._maxShadow, _minValue);
            UtilityFunctions.SetSliderF(ref _subtractSlider, _minValue, _shadowMeterScript._maxShadow, _minValue);

            // For each hit point, add UI element:
            _hitPointUI = new List<Image>();
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

            // Once timer expires, make the subtract value same as shadow value:
            _sliderDecrementTimer -= Time.deltaTime;
            if (_sliderDecrementTimer <= 0f){
                if (_subtractSlider.value > _shadowSlider.value)
                    _subtractSlider.value -= 1f;
            }
        }
        public void ReduceHitPoint(){
            
            _gameDataScript.hitPoints--;
            _hitPointSubtractTimer = _hitPointSubtractDelay;
        }
        private void UpdateHitPoints(){
            
            for (int i = 0; i < _gameDataScript.maxPoints; i++){
                // Full hit points:
                if (i < _gameDataScript.hitPoints)
                    _hitPointUI[i].sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/Hitpoints/hitpoint-full");
                // Hit point the player just lost:
                else if (i == _gameDataScript.hitPoints && _hitPointSubtractTimer > 0f)
                    _hitPointUI[i].sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/Hitpoints/hitpoint-subtract");
                // Empty hit points:
                else
                    _hitPointUI[i].sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/Hitpoints/hitpoint-empty");
            }
        }
        public void IncrementShadowSapphires(int value){
            
            // If add counter is not visible, fade in:
            StopAllCoroutines();
            StartCoroutine(MonoBehaviourUtility.FadeColorTMP(
                1.0f,
                new Color(_tempCounter.color.r, _tempCounter.color.g, _tempCounter.color.b, 1f),
                _tempCounter));

            // Increment values:
            _gameDataScript.shadowSapphires += value;
            _tempValue += value;
            _tempSubtractTimer = _tempSubtractTime;
            _tempDelayTimer = _tempDelay;
            
            // Prevent the program from closing the temp counter:
            _deactivateAdd = false;
        }
        private void UpdateShadowSapphireUI(){
            
            // Update sliders:
            _tempCounter.text = "+" + _tempValue;
            _totalCounter.text = _totalValue.ToString();

            // Decrement delay times (when 0, open/close the add counter):
            _tempDelayTimer -= Time.deltaTime;

            // Keep taking values away from the temp counter, and adding them to the total:
            if (_tempDelayTimer <= 0f && _tempValue > 0){
                _tempSubtractTimer -= Time.deltaTime;
                if (_tempSubtractTimer <= 0f){
                    _tempValue--;
                    _totalValue++;
                    StartCoroutine(ShadowSapphireBob(_tempSubtractTime / 3, 3f));
                    _tempSubtractTimer = _tempSubtractTime;
                    if (_tempValue == 0){
                        _deactivateAdd = true;
                        _tempDelayTimer = _tempDelay;
                    }
                }
            }
            
            // Fade out temp bar when no longer in use:
            if (_tempDelayTimer <= 0f && _deactivateAdd){
                StopAllCoroutines();
                StartCoroutine(MonoBehaviourUtility.FadeColorTMP(
                    1.0f,
                    new Color(_tempCounter.color.r, _tempCounter.color.g, _tempCounter.color.b, 0f),
                    _tempCounter));
                _deactivateAdd = false;
            }
        }
        private IEnumerator ShadowSapphireBob(float duration, float posMod)
        {
            // Store original UI pos:
            Vector3 originalPos = _shadowSapphiresUI.transform.position;

            // Change UI position for duration:
            float elapsed = 0.0f;
            while (elapsed < duration){
                _shadowSapphiresUI.transform.position = new Vector2(
                    originalPos.x,
                    originalPos.y + posMod);
                elapsed += Time.deltaTime;
                yield return null;
            }
            // Reset to original pos:
            _shadowSapphiresUI.transform.position = originalPos;
        }
    }
}

