using System;
using Resources.Scripts.General;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Resources.Scripts.Lighting{
    public class AlphaFade : MonoBehaviour{

        [SerializeField] private float _lerpTime;
        [SerializeField] private Light2D _light;
        [SerializeField] private Color _targetColor;
        [SerializeField] private Color _startColor;
        [SerializeField] private bool _lerpDown = true;

        private void Awake(){
            _startColor = _light.color;
        }

        private void FixedUpdate(){
            if (_lerpDown){
                _light.color = Color.Lerp(
                    _light.color,
                    _targetColor,
                    _lerpTime * Time.fixedDeltaTime);
            }
            if (!_lerpDown){
                _light.color = Color.Lerp(
                    _light.color,
                    _startColor,
                    _lerpTime * Time.fixedDeltaTime);
            }
            
            if (_light.color.a <= _targetColor.a + 0.01f)
                _lerpDown = false;
            
            if (_light.color.a >= _startColor.a - 0.01f)
                _lerpDown = true;
        }
    }
}
