using System.Collections;
using UnityEngine;

// Code within this class is responsible for shaking the camera
// to create a dynamic visual effect:
namespace Resources.Scripts.Camera{
    public class CameraShake : MonoBehaviour
    {
        public void StartShake(float dur, float mag)
        {
            StartCoroutine(Shake(dur, mag));
        }

        private IEnumerator Shake(float duration, float magnitude)
        {
            // Store original cam pos:
            Vector3 originalPos = transform.localPosition;
            
            // Randomise and set pos for duration:
            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                transform.localPosition = new Vector3(x, y, originalPos.z);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            // Reset to original pos:
            transform.localPosition = originalPos;
        }
    }
}
