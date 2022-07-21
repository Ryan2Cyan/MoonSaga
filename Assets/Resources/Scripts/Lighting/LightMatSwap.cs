using UnityEngine;

// Code within this class is responsible for changing material
// when the connected game object is in light:
namespace Resources.Scripts.Lighting{
    public class LightMatSwap : MonoBehaviour{
        
        [SerializeField] private Material _shadowMat;
        [SerializeField] private Material _lightMat;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private bool _inLight;
        [SerializeField] private LightDetection _lightDetectionScript;
        
        private void FixedUpdate(){
            _inLight = _lightDetectionScript._inLight;
            _renderer.material = _inLight ? _lightMat : _shadowMat;
        }
    }
}
