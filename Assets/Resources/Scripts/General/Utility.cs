// Utility is used to house functions that can be called by any class. These
// functions are general in application, and non-specific.

using UnityEngine;

namespace Resources.Scripts.General
{
    public class Utility
    {
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
