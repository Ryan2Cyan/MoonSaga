using System.Collections;
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
    }
}
