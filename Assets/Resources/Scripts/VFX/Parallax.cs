using UnityEngine;

namespace Resources.Scripts.VFX{
    public class Parallax : MonoBehaviour{

        [SerializeField] private UnityEngine.Camera _mainCam;
        [SerializeField] private float _parallaxMod;
        private float _length;
        private float _startPos;
        

        private void Awake(){
            
            _startPos = transform.position.x;
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void FixedUpdate(){

            // Movement relative to the camera:
            float temp = _mainCam.transform.position.x * (1f - _parallaxMod);
            
            // Calculate how far the camera has moved in world space:
            float distance = _mainCam.transform.position.x * _parallaxMod;

            // Move the background:
            transform.position = new Vector3(
                _startPos + distance,
                transform.position.y,
                transform.position.z);

            // Check if the image is out of bounds, if it is, move to the end:
            if (temp > _startPos + _length)
                _startPos += _length;
            else if (temp < _startPos - _length)
                _startPos -= _length;
        }
    }
}
