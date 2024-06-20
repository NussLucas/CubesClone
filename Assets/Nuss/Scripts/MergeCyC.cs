using UnityEngine;

namespace Nuss
{

    public class MergeCyCCollision : MonoBehaviour
    {
        public GameObject cylinderPrefab; // Assign a cylinder prefab in the Inspector

        private void OnCollisionEnter(Collision collision)
        {
            // Check if this object is a cube and it collides with a sphere
            if ((gameObject.CompareTag("Cyclinder") && collision.gameObject.CompareTag("Cube")) ||
                 (gameObject.CompareTag("Cube") && collision.gameObject.CompareTag("Cyclinder")))
            {
                // Ensure that only one of the colliding cubes handles the creation of the new cube
                if (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
                {
                    // Calculate the position for the new, larger cube (midpoint between the two cubes)
                    Vector3 newPosition = (transform.position + collision.transform.position) / 2;

                    // Instantiate the larger cube at the midpoint of the two colliding cubes
                    GameObject newCube = Instantiate(cylinderPrefab, newPosition, Quaternion.identity);

                    // Calculate the new size as the sum of the sizes of the two colliding cubes
                    Vector3 newSize = gameObject.transform.localScale;

                    // Apply the calculated size to the new cube
                    newCube.transform.localScale = newSize;

                    // Find or create the "Objects" hierarchy in the scene
                    GameObject parentObject = GameObject.Find("objects");
                    if (parentObject == null)
                    {
                        parentObject = new GameObject("objects");
                    }
                    newCube.transform.SetParent(parentObject.transform);

                    // Destroy the two original cubes
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}