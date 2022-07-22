using Resources.Scripts.General;
using UnityEngine;

// Code within this class is responsible for modifying the
// player's sprites whenever they have i-frames:
namespace Resources.Scripts.Player{
    public class IFramesVFX : MonoBehaviour{
        
        // Scripts:
        private PlayerData _playerDataScript;

        // I frames values:
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _iFrameColor;
        private bool _lerpTarget;
        [SerializeField] private float _lerpTime = 0.1f;
        [SerializeField] private float _lerpSpeed = 0.1f;
        private float _lerpTimer;

        private void Awake(){
            
            // Fetch components:
            _playerDataScript = transform.parent.GetComponent<PlayerData>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update(){

            // Flash when in i frames:
            IFramesFlash();
        }
        
        private void IFramesFlash(){
            
            // If player is in i frames, flash black:
            if (_playerDataScript._inIFrames){
                _lerpTimer = _lerpTime;
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
