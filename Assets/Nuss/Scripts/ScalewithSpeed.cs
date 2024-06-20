using UnityEngine;

namespace Nuss
{

    public class SizeBasedOnMovement : MonoBehaviour
    {
        public float minSize = 1f; // Minimum size of the object
        public float maxSize = 3f; // Maximum size of the object
        public float growthRate = 0.1f; // Rate at which the object grows
        public float shrinkRate = 0.05f; // Rate at which the object shrinks

        private Vector3 lastPosition;
        private float movementMagnitude;

        private void Start()
        {
            // Initialize the last position to the object's initial position
            lastPosition = transform.position;
        }

        private void Update()
        {
            // Calculate the magnitude of movement since the last frame
            movementMagnitude = Vector3.Distance(transform.position, lastPosition);

            // Update the last position for the next frame
            lastPosition = transform.position;

            // Determine the change in size based on the movement magnitude
            float sizeChange = movementMagnitude * growthRate - shrinkRate;

            // Apply the size change
            Vector3 newSize = transform.localScale + new Vector3(sizeChange, sizeChange, sizeChange);

            // Clamp the size within the specified range
            newSize = Vector3.Max(Vector3.Min(newSize, Vector3.one * maxSize), Vector3.one * minSize);

            // Apply the new size to the object
            transform.localScale = newSize;
        }
    }
}