using System.Collections;
using TMPro;
using UnityEngine;

// MonoBehaviour Utility is used to house functions that can be called by any class. These
// functions are general in application, but limited to the MonoBehaviour class:
namespace Resources.Scripts.General{
    public class MonoBehaviourUtility : MonoBehaviour
    {
        // Sleep function - pause the game for brief time:
        public  void StartSleep(float duration){
            StartCoroutine(HitSleep(duration));
        }
        private static IEnumerator HitSleep(float duration){
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1;
        }
        
        // Fade script:
        public static IEnumerator FadeColorTMP(float duration, Color target, TextMeshProUGUI arg){

            if (arg.color != target){
                for (float i = 0; i < duration; i += Time.deltaTime){
                    float normalisedTime = i / duration;
                    arg.color = Color.Lerp(
                        arg.color,
                        target,
                        normalisedTime
                    );
                    yield return null;
                }

                arg.color = target;
            }
            else
                yield return null;
            
        }
        
        // Check whether current object is touching the ground:
        public void GroundCheck(ref bool isGrounded, Transform groundCheck, float radius, LayerMask groundLayerMask){
            bool wasGrounded = isGrounded;
            isGrounded = false;
            
            // Store all colliders within ground-check's radius, on the ground layer:
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(
                groundCheck.position,
                radius,
                groundLayerMask
            );
            
            // Check if no ground is detected:
            if (collider2Ds.Length != 0){
                // Check all detected colliders for the ground:
                foreach (Collider2D colliderArg in collider2Ds){
                    if (colliderArg.gameObject != gameObject){
                        isGrounded = true;
                    }
                }
            }
        }
    }
}
