using UnityEngine;


namespace Nuss
{

    public class DragObject : MonoBehaviour
    {
        private Vector3 mOffset;
        private float mZCoord;

        void OnMouseDown()
        {
            // Store the object's z-coordinate (depth) when the mouse is clicked
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

            // Calculate the offset between the object's position and the mouse position in world coordinates
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        }

        void OnMouseDrag()
        {
            // Update the object's position based on the mouse's position
            transform.position = GetMouseAsWorldPoint() + mOffset;
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
    }
}
