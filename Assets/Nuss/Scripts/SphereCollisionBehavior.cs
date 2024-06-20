using UnityEngine;
using System.Collections;

namespace Nuss
{

    public class SphereCollisionBehavior : MonoBehaviour
    {
        public Material collisionMaterial; // Assigned in the Unity Editor
        public float resetInterval = 5f; // Interval at which spheres reset in seconds
        public float resetDuration = 1f; // Duration of the reset animation in seconds
        public float maxCollisionSize = 4f; // Maximum size the sphere can reach through collision
        public AnimationCurve resetCurve; // Animation curve for smoothing the transition

        private bool isColliding = false;
        private Vector3 originalScale;
        private Material originalMaterial;
        private Renderer sphereRenderer;
        private Vector3 originalPosition;
        private Rigidbody rb;

        private void Start()
        {
            // Store the original scale of the sphere
            originalScale = transform.localScale;

            // Store the original material of the sphere
            sphereRenderer = GetComponent<Renderer>();
            originalMaterial = sphereRenderer.material;

            // Store the original position of the sphere
            originalPosition = transform.position;

            // Get the Rigidbody component
            rb = GetComponent<Rigidbody>();

            // Start the reset timer
            InvokeRepeating("ResetSpheres", resetInterval, resetInterval);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Check if the collision is with another sphere
            if (collision.gameObject.CompareTag("Sphere"))
            {
                // If the spheres are not already colliding
                if (!isColliding)
                {
                    // Double the size of both spheres
                    float newScale = Mathf.Min(transform.localScale.x * 1.5f, maxCollisionSize);
                    transform.localScale = new Vector3(newScale, newScale, newScale);

                    float otherNewScale = Mathf.Min(collision.gameObject.transform.localScale.x * 1.5f, maxCollisionSize);
                    collision.gameObject.transform.localScale = new Vector3(otherNewScale, otherNewScale, otherNewScale);

                    // Change the material of both spheres
                    sphereRenderer.material = collisionMaterial;
                    collision.gameObject.GetComponent<Renderer>().material = collisionMaterial;

                    isColliding = true;
                }
                else
                {
                    // Restore the original size of both spheres
                    transform.localScale = originalScale;
                    collision.gameObject.transform.localScale = originalScale;

                    // Restore the original material of both spheres
                    sphereRenderer.material = originalMaterial;
                    collision.gameObject.GetComponent<Renderer>().material = originalMaterial;

                    isColliding = false;
                }
            }
        }

        private IEnumerator SmoothReset(Vector3 targetPosition)
        {
            // Disable the Rigidbody component during the transition
            rb.isKinematic = true;

            Vector3 initialPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < resetDuration)
            {
                transform.position = Vector3.Lerp(initialPosition, targetPosition, resetCurve.Evaluate(elapsedTime / resetDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;

            // Re-enable the Rigidbody component after the transition
            rb.isKinematic = false;

            // Stop the sphere's movement
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        private void ResetSpheres()
        {
            // Start the smooth reset coroutine
            StartCoroutine(SmoothReset(originalPosition));
        }
    }
}
