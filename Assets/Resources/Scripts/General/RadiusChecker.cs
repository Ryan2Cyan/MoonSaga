using UnityEngine;

// Code within this class checks whether any colliders of a
// specified layer are located within a radius around a 
// specified position:
namespace Resources.Scripts.General{
    public class RadiusChecker : MonoBehaviour
    {
        // Ground check:
        [SerializeField] private float _radius = 0.2f;
        [SerializeField] internal bool _collided;
        [SerializeField] internal Transform _transform;
        [SerializeField] private LayerMask _layerMask;

        private void Update(){
            
            _collided = false;
            RadiusCheck();
        }

        private void RadiusCheck(){
            
            // Store all colliders within the radius, on specified layer:
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(
                _transform.position,
                _radius,
                _layerMask
            );
            // If a collider is found, then collision is true:
            _collided = collider2Ds.Length != 0;
        }
    }
}
