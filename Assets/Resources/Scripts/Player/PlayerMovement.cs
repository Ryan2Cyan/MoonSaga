using UnityEngine;
using UnityEngine.Events;

// Code within this class is responsible (only) for the movement of the 
// player character.
namespace Resources.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour{
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayerMask;

        private const float _groundedRadius = 0.2f;
        private bool _isGrounded;
        public UnityEvent OnLandEvent;

        private void Awake(){
            if (OnLandEvent == null)
                OnLandEvent = new UnityEvent();
        }

        private void FixedUpdate(){

            // Store all colliders within ground-check's radius, on the ground layer:
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(
                _groundCheck.position,
                _groundedRadius,
                _groundLayerMask
            );
            foreach (var colliderArg in collider2Ds){
                if (colliderArg.gameObject != gameObject)
                    _isGrounded = true;
            }
        }
    }
}
