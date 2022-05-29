using Resources.Scripts.General;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Resources.Scripts.Lighting{
    public class LightColorLerp : MonoBehaviour{

        [SerializeField] private float _lerpTime = 5.0f;
        [SerializeField] private float _lerpSpeed;
        [SerializeField] private Light2D _light;
        [SerializeField] private Color _targetColor;
        private Color _originalColor;
        private bool _lerpTarget = true;
        private float _lerpTimer;

        private void Awake(){
            _originalColor = _light.color;
            _lerpTimer = _lerpTime;
        }

        private void Update(){

            // Lerp between two alpha's:
            _light.color = UtilityFunctions.TwoColorLerpOverTime(_light.color, _originalColor, _targetColor, _lerpSpeed,
                ref _lerpTarget, ref _lerpTimer, _lerpTime);
        }
    }
}
