using UnityEngine;
using System.Collections;


namespace Nuss
{

    public class Center : MonoBehaviour
    {
        public Light pointLight;              // Reference to the Point Light
        public float activationTime = 2.0f;   // Duration the light stays on
        public float fadeDuration = 1.0f;     // Duration of the fade-out

        private void Start()
        {
            if (pointLight != null)
            {
                pointLight.enabled = false;   // Ensure the light is off initially
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Activatable"))
            {
                if (pointLight != null)
                {
                    StopAllCoroutines();       // Stop any ongoing coroutines to avoid conflicts
                    StartCoroutine(ActivateLight());
                }
            }
        }

        private IEnumerator ActivateLight()
        {
            pointLight.enabled = true;         // Turn on the light
            pointLight.intensity = 100.0f;       // Ensure light intensity is full
            yield return new WaitForSeconds(activationTime); // Wait for the activation duration

            // Start the fade-out process
            float startIntensity = pointLight.intensity;
            float elapsedTime = 0.0f;

            while (elapsedTime < fadeDuration)
            {
                pointLight.intensity = Mathf.Lerp(startIntensity, 0.0f, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null; // Wait for the next frame
            }

            pointLight.intensity = 0.0f;       // Ensure light is fully off
            pointLight.enabled = false;        // Disable the light
        }
    }
}