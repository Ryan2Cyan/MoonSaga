using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts.Player{
    public class EnergyBar : MonoBehaviour{
        [SerializeField] private float _lightDetectionDistance = 90.0f;
        [SerializeField] private bool _inLightLOS;
        [SerializeField] private bool _inLightCollider;
        [SerializeField] private GameObject[] _sceneLights;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Image _debugSymbol;

        private void Awake(){
            _sceneLights = GameObject.FindGameObjectsWithTag("Light");
        }

        private void FixedUpdate(){
            _inLightLOS = false;
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _sceneLights[0].transform.position);
            
            // Cast a ray from player to in-range light source:
            foreach (GameObject inRangeLightSource in FindLightsInRange(_sceneLights, _lightDetectionDistance)){
                // Check if the player is in line-of-sight of this light source:
                RayCastLightCheck(inRangeLightSource, ref _inLightLOS);
            }

            _debugSymbol.sprite = 
                UnityEngine.Resources.Load<Sprite>(!_inLightLOS ? 
                    "Sprites/MoonSaga-Symbols-dark" : "Sprites/MoonSaga-Symbols-light");
        }
        
        private IEnumerable<GameObject> FindLightsInRange(IEnumerable<GameObject> sceneLights, float maxDistance){
            
            // Check how far each light is from the player, if below max distance, add to list:
            List<GameObject> inRangeLights = new List<GameObject>();
            
            foreach (GameObject lightSource in sceneLights){
                float distance = Vector2.Distance(transform.position, lightSource.transform.position);
                if (distance < maxDistance){
                    inRangeLights.Add(lightSource);
                }
            }
            return inRangeLights;
        }

        private void RayCastLightCheck(GameObject lightSource, ref bool lineOfSight){
            
            // Calculate direction to cast the ray:
            Vector3 direction = lightSource.transform.position - transform.position;
            
            // Cast ray from player to light source:
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction);
            
            foreach (RaycastHit2D hitValue in hits){
                // Check for any game objects:
                if (hitValue.transform.gameObject != null){
                        
                    // If the ray hits a game object that is not a light, then exit:
                    if (!hitValue.transform.gameObject.CompareTag("Light")){
                        lineOfSight = false;
                        break;
                    }
                    if(_inLightCollider)
                        lineOfSight = true; // Player is in line-of-sight of the light
                }   
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
