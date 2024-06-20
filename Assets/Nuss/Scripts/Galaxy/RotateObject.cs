using UnityEngine;
using System.Collections.Generic;

namespace Nuss
{
    public class RotateObject : MonoBehaviour
    {
        public List<GameObject> objectsToRotate; // List of additional objects to rotate

        private Vector3 mOffset;
        private float mZCoord;
        private Vector3 mInitialMousePos;
        private Quaternion mInitialRotation;

        void OnMouseDown()
        {
            // Store the object's z-coordinate (depth) when the mouse is clicked
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

            // Store the initial mouse position in world coordinates
            mInitialMousePos = GetMouseAsWorldPoint();

            // Store the initial rotation of the object
            mInitialRotation = transform.rotation;
        }

        void OnMouseDrag()
        {
            // Calculate the difference between the initial and current mouse positions in world coordinates
            Vector3 currentMousePos = GetMouseAsWorldPoint();
            Vector3 delta = currentMousePos - mInitialMousePos;

            // Calculate the rotation amounts based on the mouse movement
            float angleX = delta.y * 10; // Rotate around the x-axis
            float angleY = delta.x * 10; // Rotate around the y-axis
            float angleZ = (delta.x + delta.y) * 5; // Rotate around the z-axis

            // Apply the rotation to the object
            Quaternion newRotation = mInitialRotation * Quaternion.Euler(angleX, angleY, angleZ);
            transform.rotation = newRotation;

            // Rotate the skybox based on the object's rotation
            RotateSkybox(newRotation);

            // Rotate the additional objects
            foreach (GameObject obj in objectsToRotate)
            {
                obj.transform.rotation = newRotation;
            }
        }

        private Vector3 GetMouseAsWorldPoint()
        {
            // Get the mouse position in screen coordinates
            Vector3 mousePoint = Input.mousePosition;

            // Set the z-coordinate of the mouse's position to the object's z-coordinate
            mousePoint.z = mZCoord;

            // Convert the screen coordinates to world coordinates
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }

        private void RotateSkybox(Quaternion rotation)
        {
            // Calculate the euler angles from the rotation quaternion
            Vector3 eulerRotation = rotation.eulerAngles;

            // Update the skybox rotation
            RenderSettings.skybox.SetFloat("_Rotation", eulerRotation.y);
        }
    }
}

