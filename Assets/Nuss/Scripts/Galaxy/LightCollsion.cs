using UnityEngine;
using System.Collections;

namespace Nuss
{

    public class ActivateLightOnCollision : MonoBehaviour
    {
        public Material activeMaterial; // Assign this in the Inspector
        public Material originalMaterial; // Assign this in the Inspector

        private Light pointLight;
        private Coroutine fadeCoroutine;
        private Renderer objectRenderer;


        void Start()
        {
            // Find the Point Light component in the children of this GameObject
            pointLight = GetComponentInChildren<Light>();
            objectRenderer = GetComponent<Renderer>();

            if (pointLight != null)
            {
                pointLight.enabled = false; // Ensure the light is initially disabled
            }
            else
            {
                Debug.LogError("No Point Light found on the cube.");
            }

            if (objectRenderer == null)
            {
                Debug.LogError("No Renderer found on the GameObject.");
            }
            else if (originalMaterial == null)
            {
                // Set the original material to the current material if not assigned
                originalMaterial = objectRenderer.material;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            // Check if the object we collided with has a collider and is not itself
            if (collision.collider != null && collision.gameObject != this.gameObject)
            {
                if (pointLight != null)
                {
                    if (fadeCoroutine != null)
                    {
                        StopCoroutine(fadeCoroutine); // Stop any existing fade coroutine
                    }
                    pointLight.enabled = true; // Enable the light on collision
                    pointLight.intensity = 250.0f; // Set the light intensity to maximum
                    fadeCoroutine = StartCoroutine(FadeOutLight()); // Start the fade-out coroutine

                    // Change the material to the active material
                    if (objectRenderer != null && activeMaterial != null)
                    {
                        objectRenderer.material = activeMaterial;
                    }
                }
            }
        }

        private IEnumerator FadeOutLight()
        {
            yield return new WaitForSeconds(1); // Wait for 1 second

            float fadeDuration = 2.0f; // Duration of the fade-out in seconds
            float startIntensity = pointLight.intensity;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                pointLight.intensity = Mathf.Lerp(startIntensity, 0, t / fadeDuration);
                yield return null;
            }

            pointLight.intensity = 0;
            pointLight.enabled = false; // Disable the light after fading out

            // Revert to the original material after the light fades out
            if (objectRenderer != null && originalMaterial != null)
            {
                objectRenderer.material = originalMaterial;
            }
        }



        private float EaseInOut(float t)
        {
            return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
        }
    }
}