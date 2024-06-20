using UnityEngine;

namespace Nuss
{
    public class MergeCyCaCollision : MonoBehaviour
    {
        public GameObject cylinderPrefab; // Assign a cylinder prefab in the Inspector

        private void OnCollisionEnter(Collision collision)
        {
            // Check if this object is a capsule and it collides with a sphere or vice versa
            if ((gameObject.CompareTag("Capsule") && collision.gameObject.CompareTag("Cyclinder")) ||
                 (gameObject.CompareTag("Cyclinder") && collision.gameObject.CompareTag("Capsule")))
            {
                // Ensure that only one of the colliding objects handles the creation of the new object
                if (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
                {
                    // Calculate the position for the new cylinder (midpoint between the two objects)
                    Vector3 newPosition = (transform.position + collision.transform.position) / 2;

                    // Instantiate the new cylinder at the midpoint of the two colliding objects
                    GameObject newCylinder = Instantiate(cylinderPrefab, newPosition, Quaternion.identity);

                    // Calculate the new size. x and z dimensions are taken from this object, y is the sum of y dimensions from both objects
                    Vector3 newSize = new Vector3(gameObject.transform.localScale.x,
                                                  gameObject.transform.localScale.y + collision.transform.localScale.y,
                                                  gameObject.transform.localScale.z);

                    // Apply the calculated size to the new cylinder
                    newCylinder.transform.localScale = newSize;

                    // Find or create the "objects" hierarchy in the scene
                    GameObject parentObject = GameObject.Find("objects");
                    if (parentObject == null)
                    {
                        parentObject = new GameObject("objects");
                    }
                    newCylinder.transform.SetParent(parentObject.transform);

                    // Destroy the two original objects
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
