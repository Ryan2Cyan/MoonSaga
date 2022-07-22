using UnityEngine;

namespace Resources.Scripts.Player{
    public class PlayerData : MonoBehaviour
    {
        // Components:
        internal Rigidbody2D _rigidbody2D;
        
        // Orientation:
        [SerializeField] internal bool _isFacingRight = true;
        
        // Sprites:
        [SerializeField] internal GameObject _hoodUpSprite;
        [SerializeField] internal GameObject _hoodDownSprite;
        [SerializeField] internal Animator _hoodUpSpriteAnim;
        [SerializeField] internal Animator _hoodDownSpriteAnim;
        
        // Walking:
        [Range(0, 100f)] [SerializeField] internal float _walkSpeed = 37.5f;
        
        // Jump:
        [Range(0, 1000.0f)] [SerializeField] internal float _jumpForce = 100f;
        [Range(0, 1.0f)] [SerializeField] internal float _maxAirTime = 0.5f;
        internal float _airTimer;
        internal bool _isJumping;
        
        // Double Jump:
        [SerializeField] internal bool _doubleJumpAvailable = true;
        [Range(0, 2)][SerializeField] internal float _doubleJumpAirTime = 0.5f;
        [Range(0, 100)] [SerializeField] internal int _doubleJumpCost = 10;
        [Range(0, 1000.0f)] [SerializeField] internal float _doubleJumpForce = 100f;
        [Range(0, 1.0f)] [SerializeField] internal float _doubleJumpDelay = 0.15f;
        
        // Dash:
        [Range(0, 100.0f)] [SerializeField] internal float _dashSpeed = 100f;
        [SerializeField] internal float _dashThresholdCost = 20f;
        [Range(0, 100.0f)] [SerializeField] internal float _dashDownSpeed = 100f;
        [SerializeField] internal Vector2 _dashKnockBack;
        [SerializeField] internal float _dashKnockBackDelay = 0.1f;
        [SerializeField] internal float _dashShadowDecrement = 0.1f;


        // Land:
        [SerializeField] internal float _landDelay = 0.1f;
        internal float _landTimer;
        
        // Damaged:
        [SerializeField] internal float _damagedKnockBackDelay = 0.1f;
        internal float _knockBackTimer;

        // Damage values:
        [SerializeField] internal float _damageIFrames = 0.5f;
        [SerializeField] internal bool _inIFrames;
        internal float _damageIFramesTimer;

        private void Awake(){
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}
