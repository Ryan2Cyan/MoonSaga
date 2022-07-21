using System.Collections.Generic;
using UnityEngine;

// Code within this class is responsible detecting whether or not
// a specified position is touching light:
namespace Resources.Scripts.Lighting{
    public class LightDetection : MonoBehaviour
    {
        
        [SerializeField] private float _lightDetectionDistance = 90.0f;
        [SerializeField] internal bool _inLight;
        [SerializeField] private GameObject[] _sceneLights;
        private bool _inLightLOS;
        private bool _inLightCollider;
        private bool _hitObstacle;
        
        private void Awake(){
            
            // Fetch all lights in the scene:
            _sceneLights = GameObject.FindGameObjectsWithTag("Light");
        }

        private void FixedUpdate(){
            _inLightLOS = false;
            _inLight = false;
            _hitObstacle = false;
            
            // Check if light is within range:
            foreach (GameObject inRangeLightSource in FindLightsInRange()){
                // Check if in line-of-sight:
                RayCastLightCheck(inRangeLightSource);
            }

            if (_inLightLOS && _inLightCollider)
                _inLight = true;
        }
        
        private IEnumerable<GameObject> FindLightsInRange(){
            
            // Check how far each light is from the player, if within specified distance, add to list:
            List<GameObject> inRangeLights = new List<GameObject>();
            foreach (GameObject lightSource in _sceneLights){
                float distance = Vector2.Distance(transform.position, lightSource.transform.position);
                if (distance < _lightDetectionDistance)
                    inRangeLights.Add(lightSource);
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
                    if (hitValue.transform.gameObject == gameObject)
                        continue; // Hit itself [skip]:
                    if (hitValue.transform.gameObject.CompareTag("Player"))
                        continue; // Hit player [skip]:
                    if (hitValue.transform.gameObject.CompareTag("Obstacle"))
                        _hitObstacle = true;
                }   
            }
            // If the object has not hit an obstacle, they are in the light:
            _inLightLOS = !_hitObstacle;
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
