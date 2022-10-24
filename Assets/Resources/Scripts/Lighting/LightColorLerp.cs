using Resources.Scripts.General;
using UnityEngine;


// Code within this class is responsible for lerping the color
// of a 2D light between a target color, and the light's original color:
namespace Resources.Scripts.Lighting{
    public class LightColorLerp : MonoBehaviour{

        [SerializeField] private float _lerpTime = 5.0f;
        [SerializeField] private float _lerpSpeed;
        [SerializeField] private Color _targetColor;
        private UnityEngine.Rendering.Universal.Light2D _light;
        private Color _originalColor;
        private bool _lerpTarget = true;
        private float _lerpTimer;

        private void Awake(){
            
            // Fetch components:
            _light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            
            // Set values:
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
