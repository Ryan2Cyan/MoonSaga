using System;
using Resources.Scripts.General;
using UnityEngine;

namespace Resources.Scripts.Player{
    public class PlayerAnimator : MonoBehaviour{
        
        // Scripts:
        private PlayerMovement _playerMovementScript;
        
        // Animator values:
        private playerMoveState _state;
        private Animator _animator;
        
        // I frames values:
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _iFrameColor;
        private bool _lerpTarget;
        [SerializeField] private float _lerpTime = 0.1f;
        [SerializeField] private float _lerpSpeed = 0.1f;
        private float _lerpTimer;
        
        // Property Indexes:
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Dash = Animator.StringToHash("Dash");
        private static readonly int DoubleJump = Animator.StringToHash("DoubleJump");
        private static readonly int Damaged = Animator.StringToHash("Damaged");

        private void Awake(){
            
            // Fetch components:
            _playerMovementScript = transform.parent.GetComponent<PlayerMovement>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

        }

        private void Update(){
            // Assign player's current state:
            _state = _playerMovementScript._state;
            
            // Change animation based on player's current state:
            ProcessStateAnimation();
            
            // Flash when in i frames:
            _lerpTimer = _lerpTime;
            IFramesFlash();
        }

        private void ResetAnimator(){
            _animator.SetBool(Walk, false);
            _animator.SetBool(Jump, false);
            _animator.SetBool(Falling, false);
            _animator.SetBool(Land, false);
            _animator.SetBool(Dash, false);
            _animator.SetBool(DoubleJump, false);
            _animator.SetBool(Damaged, false);
        }
        private void ProcessStateAnimation(){
            ResetAnimator();
            switch(_state) {
                case playerMoveState.Idle:
                    break;
                case playerMoveState.Walking:
                    _animator.SetBool(Walk, true);
                    break;
                case playerMoveState.Jump:
                    _animator.SetBool(Jump, true);
                    break;
                case playerMoveState.AirControl:
                    _animator.SetBool(Falling, true);
                    break;
                case playerMoveState.Land:
                    _animator.SetBool(Land, true);
                    break;
                case playerMoveState.Dash:
                    _animator.SetBool(Dash, true);
                    break;
                case playerMoveState.DashHit:
                    _animator.SetBool(Dash, true);
                    break;
                case playerMoveState.Damaged:
                    _animator.SetBool(Damaged, true);
                    break;
                case playerMoveState.DoubleJump:
                    _animator.SetBool(DoubleJump, true);
                    break;
                case playerMoveState.BounceDive:
                    _animator.SetBool(Falling, true);
                    break;
                case playerMoveState.BounceDiveHit:
                    _animator.SetBool(Falling, true);
                    break;
                case playerMoveState.DashRecover:
                    _animator.SetBool(Damaged, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
