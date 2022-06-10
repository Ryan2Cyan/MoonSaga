using UnityEngine;

namespace Resources.Scripts.General{
    public class GroundCheck : MonoBehaviour
    {
        // Ground check:
        [SerializeField] private float _groundedRadius = 0.2f;
        [SerializeField] internal bool _isGrounded;
        [SerializeField] internal Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayerMask;
        
        // Ceiling check:
        [SerializeField] private float _ceilingRadius = 0.2f;
        [SerializeField] internal bool _isCeiling;
        [SerializeField] internal Transform _ceilingCheck;
        [SerializeField] private LayerMask _ceilingLayerMask;

        private bool _checkGround;
        private bool _checkCeiling;

        private void Awake(){
            if (_groundCheck != null)
                _checkGround = true;
            if (_ceilingCheck != null)
                _checkCeiling = true;
        }

        private void Update(){
            
            // Check for ground:
            if (_checkGround){
                _isGrounded = false;
                GroundChecker();
            }

            // Check for ceiling:
            if (_checkCeiling){
                _isCeiling = false;
                CeilingChecker();
            }
        }

        private void GroundChecker(){
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
        private void CeilingChecker(){
            // Store all colliders within ground-check's radius, on the ground layer:
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(
                _ceilingCheck.position,
                _ceilingRadius,
                _ceilingLayerMask
            );

            // Check if no ground is detected:
            if (collider2Ds.Length != 0){
                // Check all detected colliders for the ground:
                foreach (Collider2D colliderArg in collider2Ds){
                    if (colliderArg.gameObject != gameObject){
                        _isCeiling = true;
                    }
                }
            }
        }
    }
}
