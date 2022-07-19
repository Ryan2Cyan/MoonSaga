using UnityEngine;

namespace Resources.Scripts.General{
    public class RadiusChecker : MonoBehaviour
    {
        // Ground check:
        [SerializeField] private float _radius = 0.2f;
        [SerializeField] internal bool _collided;
        [SerializeField] internal Transform _transform;
        [SerializeField] private LayerMask _layerMask;
        

        private bool _checkGround;
        private bool _checkCeiling;

        private void Awake(){
            if (_transform != null)
                _checkGround = true;
            if (_transform != null)
                _checkCeiling = true;
        }

        private void Update(){
            
            // Check for ground:
            if (_checkGround){
                _collided = false;
                GroundChecker();
            }
        }

        private void GroundChecker(){
            // Store all colliders within ground-check's radius, on the ground layer:
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(
                _transform.position,
                _radius,
                _layerMask
            );

            // Check if no ground is detected:
            if (collider2Ds.Length != 0){
                // Check all detected colliders for the ground:
                foreach (Collider2D colliderArg in collider2Ds){
                    if (colliderArg.gameObject != gameObject){
                        _collided = true;
                    }
                }
            }
        }
    }
}
