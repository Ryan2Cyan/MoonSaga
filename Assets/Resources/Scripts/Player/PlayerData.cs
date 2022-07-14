using UnityEngine;

namespace Resources.Scripts.Player{
    public class PlayerData : MonoBehaviour
    {
        internal Rigidbody2D _rigidbody2D;
        [SerializeField] internal GameObject _hoodUpSprite;
        [SerializeField] internal GameObject _hoodDownSprite;
        [SerializeField] internal Animator _hoodUpSpriteAnim;
        [SerializeField] internal Animator _hoodDownSpriteAnim;

        private void Awake(){
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}
