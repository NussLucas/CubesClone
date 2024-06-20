using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Nuss
{
    public class MonitorAndReset : MonoBehaviour
    {
        public Material skyboxMaterial; // Assign this in the Inspector
        public float maxExposure = 1.3f; // Maximum exposure value for the skybox
        public float minExposure = 0.3f; // Minimum exposure value for the skybox
        public float startExposure = 0.0f; // Exposure at the start of the game
        public float targetExposure = 1.0f; // Target exposure to reach over time
        public float fadeInDuration = 5.0f; // Duration to fade in the exposure

        private List<ChangeMaterialAndFadeOnCollision> cubes;
        private bool allFadedOut = false;
        private float originalExposure;
        private bool fadeInCompleted = false; // Flag to track if fade-in completed

        void Start()
        {
            cubes = new List<ChangeMaterialAndFadeOnCollision>();

            // Get all ChangeMaterialAndFadeEmissionOnCollision components in children
            foreach (Transform child in transform)
            {
                ChangeMaterialAndFadeOnCollision cubeScript = child.GetComponent<ChangeMaterialAndFadeOnCollision>();
                if (cubeScript != null)
                {
                    cubes.Add(cubeScript);
                }
            }

            // Store the original exposure value of the skybox
            if (skyboxMaterial.HasProperty("_Exposure"))
            {
                originalExposure = skyboxMaterial.GetFloat("_Exposure");
            }
            else
            {
                originalExposure = maxExposure; // Default value if not set
            }

            // Set initial exposure
            SetSkyboxExposure(startExposure);

            // Start the fade-in coroutine
            StartCoroutine(FadeInSkyboxExposure());

            // Start the monitoring coroutine only after the fade-in is complete
            StartCoroutine(MonitorCubes());
        }

        private IEnumerator FadeInSkyboxExposure()
        {
            float startTime = Time.time;

            while (Time.time - startTime < fadeInDuration)
            {
                float t = (Time.time - startTime) / fadeInDuration;
                float newExposure = Mathf.Lerp(startExposure, targetExposure, t);
                SetSkyboxExposure(newExposure);
                yield return null;
            }

            // Ensure the final exposure is set
            SetSkyboxExposure(targetExposure);

            fadeInCompleted = true; // Set flag to true when fade-in completes
        }

        private IEnumerator MonitorCubes()
        {
            // Wait until fade-in is completed
            while (!fadeInCompleted)
            {
                yield return null;
            }

            // Start monitoring cubes after fade-in
            while (!allFadedOut)
            {
                allFadedOut = true;
                int activeCount = 0;

                foreach (var cube in cubes)
                {
                    if (cube.gameObject.activeSelf)
                    {
                        allFadedOut = false;
                        activeCount++;
                    }
                }

                // Adjust skybox exposure based on the number of active cubes
                AdjustSkyboxExposure(activeCount);

                if (allFadedOut)
                {
                    Invoke("ResetScene" , 7);
                }

                yield return new WaitForSeconds(1); // Check every 1 second
            }
        }

        private void AdjustSkyboxExposure(int activeCount)
        {
            float exposureRange = maxExposure - minExposure;
            float normalizedCount = (float)activeCount / cubes.Count;
            float newExposure = minExposure + (normalizedCount * exposureRange);

            if (skyboxMaterial.HasProperty("_Exposure"))
            {
                skyboxMaterial.SetFloat("_Exposure", newExposure);
            }
        }

        private void SetSkyboxExposure(float exposure)
        {
            if (skyboxMaterial.HasProperty("_Exposure"))
            {
                skyboxMaterial.SetFloat("_Exposure", exposure);
            }
        }

        private void ResetScene()
        {
            
            // Reset the scene by reloading it
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
