using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UtilityFunctions = Resources.Scripts.General.UtilityFunctions;

namespace Resources.Scripts.Player{
    public class ShadowMeter : MonoBehaviour{
        
        [SerializeField] private float _lightDetectionDistance = 90.0f;
        [SerializeField] private bool _inLightLOS;
        [SerializeField] private bool _inLightCollider;
        [SerializeField] private Slider _shadowSlider;
        [SerializeField] private GameObject[] _sceneLights;

        [SerializeField] private int _shadowValue = 1;
        [SerializeField] private int _lightValue = 1;
        private const int _maxValue = 100;
        private const int _minValue = 0;
        [SerializeField] private float _incrementDelay = 0.1f;
        private float _incrementTimer;

        private void Awake(){
            
            // Fetch all lights in the scene:
            _sceneLights = GameObject.FindGameObjectsWithTag("Light");
            
            // Set values:
            UtilityFunctions.SetSlider(ref _shadowSlider, _minValue, _maxValue, _minValue);
        }

        private void Update(){
            
            // Update shadow meter:
            UpdateShadowMeter();
        }

        private void FixedUpdate(){
            _inLightLOS = false;
            
            // Cast a ray from player to in-range light source:
            foreach (GameObject inRangeLightSource in FindLightsInRange()){
                // Check if the player is in line-of-sight of this light source:
                RayCastLightCheck(inRangeLightSource);
            }
        }
        
        private IEnumerable<GameObject> FindLightsInRange(){
            
            // Check how far each light is from the player, if below max distance, add to list:
            List<GameObject> inRangeLights = new List<GameObject>();
            
            foreach (GameObject lightSource in _sceneLights){
                float distance = Vector2.Distance(transform.position, lightSource.transform.position);
                if (distance < _lightDetectionDistance){
                    inRangeLights.Add(lightSource);
                }
            }
            return inRangeLights;
        }

        private void RayCastLightCheck(GameObject lightSource){
            
            // Calculate direction to cast the ray:
            Vector3 direction = lightSource.transform.position - transform.position;
            
            // Cast ray from player to light source:
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction);
            
            foreach (RaycastHit2D hitValue in hits){
                // Check for any game objects:
                if (hitValue.transform.gameObject != null){
                        
                    // If the ray hits a game object that is not a light, then exit:
                    if (!hitValue.transform.gameObject.CompareTag("Light")){
                        _inLightLOS = false;
                        break;
                    }
                    if(_inLightCollider)
                        _inLightLOS = true; // Player is in line-of-sight of the light
                }   
            }
        }

        private void UpdateShadowMeter(){

            // Decrease timer:
            _incrementTimer -= Time.deltaTime;

            // Player in light:
            if (_inLightCollider && _inLightLOS && _incrementTimer <= 0.0f){
                _shadowSlider.value += _lightValue;
                _incrementTimer = _incrementDelay;
            }
            // Player in shadow:
            else if (_incrementTimer <= 0.0f){
                _shadowSlider.value += _shadowValue;
                _incrementTimer = _incrementDelay;
            }
        }

        private void OnTriggerEnter2D(Collider2D other){
            if(other.transform.gameObject.CompareTag("Light"))
                _inLightCollider = true;
        }
        
        private void OnTriggerExit2D(Collider2D other){
            if(other.transform.gameObject.CompareTag("Light"))
                _inLightCollider = false;
        }
    }
}
