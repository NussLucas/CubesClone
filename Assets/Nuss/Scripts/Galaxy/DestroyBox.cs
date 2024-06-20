
using UnityEngine;
using System.Collections;

namespace Nuss
{

    public class ChangeMaterialAndFadeOnCollision : MonoBehaviour
    {
        public Material newMaterial; // Assign this in the Inspector
        public string targetTag = "Center"; // Tag of the specific cube
        public float fadeDuration = 2.0f; // Duration of the fade-out
        public float initialFadeDuration = 2.0f; // Duration of the fade-in

        private Renderer objectRenderer;
        private bool isFading = false;

        void Start()
        {
            // Get the Renderer component attached to this GameObject
            objectRenderer = GetComponent<Renderer>();
            if (objectRenderer == null)
            {
                Debug.LogError("No Renderer found on the GameObject.");
            }

            // Start the fade-in process at the beginning of the game
            StartCoroutine(FadeInAlbedo());
        }

        void OnCollisionEnter(Collision collision)
        {
            // Check if the collided object has the specific tag
            if (collision.gameObject.CompareTag(targetTag))
            {
                // Change the material of this GameObject
                if (objectRenderer != null && newMaterial != null)
                {
                    objectRenderer.material = newMaterial;
                }

                // Start fading out this GameObject
                if (!isFading && !gameObject.CompareTag(targetTag))
                {
                    StartCoroutine(FadeOutAlbedo());
                }
            }
        }

        private IEnumerator FadeOutAlbedo()
        {
            isFading = true;
            Material mat = objectRenderer.material;
            Color startColor = mat.color;
            Color endColor = new(startColor.r, startColor.g, startColor.b, 0);

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                float progress = t / fadeDuration;
                Color currentColor = Color.Lerp(startColor, endColor, progress);
                mat.color = currentColor;
                yield return null;
            }

            mat.color = endColor;
            gameObject.SetActive(false); // Optionally disable the object after fading out
        }

        private IEnumerator FadeInAlbedo()
        {
            Material mat = objectRenderer.material;
            Color startColor = new(mat.color.r, mat.color.g, mat.color.b, 0);
            Color endColor = mat.color;
            mat.color = startColor;

            for (float t = 0; t < initialFadeDuration; t += Time.deltaTime)
            {
                float progress = t / initialFadeDuration;
                Color currentColor = Color.Lerp(startColor, endColor, progress);
                mat.color = currentColor;
                yield return null;
            }

            mat.color = endColor;
            isFading = false;
        }

        // Method to reset the game
        public void ResetGame()
        {
            gameObject.SetActive(true); // Reactivate the object if it was deactivated
            StartCoroutine(FadeInAlbedo()); // Start the fade-in process
        }
    }
}