using System;
using UnityEngine;

namespace Resources.Scripts.VFX{
    public class LightMatSwap : MonoBehaviour{
        
        [SerializeField] private Material _shadowMat;
        [SerializeField] private Material _lightMat;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private bool _inLight;
        
        private void FixedUpdate(){
            _renderer.material = _inLight ? _lightMat : _shadowMat;
        }

        private void OnTriggerStay2D(Collider2D other){
            if (other.gameObject.CompareTag("Light"))
                _inLight = true;
        }

        private void OnTriggerExit2D(Collider2D other){
            if (other.gameObject.CompareTag("Light"))
                _inLight = false;
        }
    }
}
