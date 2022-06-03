using UnityEngine;

namespace Resources.Scripts.General{
    public class GroundCheck : MonoBehaviour
    {
        // Trigger collider:
        [SerializeField] internal Collider2D _groundCollider;

        // Values:
        private const float _groundedRadius = 0.2f;
        [SerializeField] internal bool _isGrounded;
        [SerializeField] internal Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayerMask;

        private void Update(){
            _isGrounded = false;

            // Store all colliders within ground-check's radius, on the ground layer:
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(
                _groundCheck.position,
                _groundedRadius,
                _groundLayerMask
            );

            // Check if no ground is detected:
            if (collider2Ds.Length != 0){
                // Check all detected colliders for the ground:
                foreach (Collider2D colliderArg in collider2Ds){
                    if (colliderArg.gameObject != gameObject){
                        _isGrounded = true;
                    }
                }
            }
        }
    }
}
