using UnityEngine;
using UnityEngine.SceneManagement;


namespace Nuss
{
    public class Counter : MonoBehaviour
    {
        public string hierarchyObjectName = "objects"; // The name of the hierarchy object to monitor

        void Update()
        {
            // Find the hierarchy object
            GameObject hierarchyObject = GameObject.Find(hierarchyObjectName);
            if (hierarchyObject != null)
            {
                // Count the number of child objects
                int childCount = hierarchyObject.transform.childCount;
                // Check if there are only 2 child objects left
                if (childCount == 2)
                {
                    // Reset the scene if only 2 objects are left
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }
}