using Resources.Scripts.General;
using UnityEngine;

namespace Resources.Scripts.Player{
    public class IFramesVFX : MonoBehaviour{
        
        // Scripts:
        private PlayerMovement _playerMovementScript;

        // I frames values:
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _iFrameColor;
        private bool _lerpTarget;
        [SerializeField] private float _lerpTime = 0.1f;
        [SerializeField] private float _lerpSpeed = 0.1f;
        private float _lerpTimer;

        private void Awake(){
            
            // Fetch components:
            _playerMovementScript = transform.parent.GetComponent<PlayerMovement>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

        }

        private void Update(){

            // Flash when in i frames:
            _lerpTimer = _lerpTime;
            IFramesFlash();
        }
        
        private void IFramesFlash(){
            // If player is in i frames, flash black:
            if (_playerMovementScript._inIFrames){
                _spriteRenderer.color = UtilityFunctions.TwoColorLerpOverTime(
                        _spriteRenderer.color,
                        Color.white,
                        _iFrameColor,
                        _lerpSpeed,
                        ref _lerpTarget,
                        ref _lerpTimer,
                        _lerpTime
                        );
            }
            // If player is not in i frames, stay normal color:
            else if(_spriteRenderer.color != Color.white)
                _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.white, _lerpSpeed);
            
        }
    }
}
