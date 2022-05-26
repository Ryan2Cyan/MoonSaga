// Utility is used to house functions that can be called by any class. These
// functions are general in application, and non-specific.

using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts.General
{
    public static class UtilityFunctions
    {
        // Flip object:
        public static Vector3 Flip(Vector3 localScaleArg, ref bool facingRight){
            
            // Inverse bool value:
            facingRight = !facingRight;
            
            // Multiply x local scale by -1:
            return new Vector3(localScaleArg.x * -1.0f, localScaleArg.y, localScaleArg.z);
        }
        public static Vector3 Flip(Vector3 localScaleArg){
            
            // Multiply x local scale by -1:
            return new Vector3(localScaleArg.x * -1.0f, localScaleArg.y, localScaleArg.z);
        }
        
        // Set the base values for a slider:
        public static void SetSlider(ref Slider slider, int minVal, int maxVal, int startVal){
            slider.maxValue = maxVal;
            slider.minValue = minVal;
            slider.value = startVal;
        }
        
        // Using a line renderer, draw a circle:
        public static void DrawCircle(ref LineRenderer circleRenderer, int steps, float radius){

            // Set steps (how many lines) of the circle:
            circleRenderer.positionCount = steps;

            for (int currentStep = 0; currentStep < steps; currentStep++){
                // Calculate where the step is, relative to it's starting point, along the circle's circumference:
                float circumferenceProgress = (float) currentStep / steps;

                // Convert circumference progress into radians:
                float currentRadian = circumferenceProgress * 2f * Mathf.PI;

                // Calculate positions of each point:
                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);
                
                // Scale to the size of the circle to get the final positions:
                float x = xScaled * radius;
                float y = yScaled * radius;
                Vector3 currentPosition = new Vector3(x, y, 0f);
                Debug.Log(currentPosition);
                circleRenderer.SetPosition(currentStep, currentPosition);
            }
        }
    }
}
