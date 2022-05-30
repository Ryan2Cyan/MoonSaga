using Resources.Scripts.Player;
using UnityEngine;

// Code within this class is responsible for making the camera follow
// the player character.
namespace Resources.Scripts.Camera{
    public class CameraFollow : MonoBehaviour{
        
        [SerializeField] private Transform _target;
        [SerializeField] private Vector2 _offset;
        [Range(0f, 1f)] [SerializeField] private float _smoothSpeed = 0.125f;
        [SerializeField] private PlayerMovement _playerMovementScript;
        [SerializeField] private bool _isFacingRight;
        

        private void FixedUpdate(){
            
            // Check if the player is facing right or left:
            _isFacingRight = _playerMovementScript._isFacingRight;

            Vector2 desiredPos = _isFacingRight switch{
                // Calculate the desired position of the camera - alter x-axis offset depending on player's direction:
                true => new Vector2(_target.position.x + _offset.x + 1f, _target.position.y + _offset.y),
                false => new Vector2(_target.position.x + _offset.x - 1f, _target.position.y + _offset.y)
            };

            // Lerp the camera towards the target:
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(desiredPos.x, desiredPos.y, transform.position.z),
                _smoothSpeed);
        }
    }
}
