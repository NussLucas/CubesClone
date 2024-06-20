using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nuss
{
    public class BallLight : MonoBehaviour
    {
        // Start is called before the first frame update
        public Material collisionMaterial; // Material to switch to on collision
        public float duration = 0.25f; // Duration to keep the material

        private Material originalMaterial;
        private new MeshRenderer renderer;
        private bool isColliding = false;

        void Start()
        {
            renderer = GetComponent<MeshRenderer>();
            originalMaterial = renderer.material; // Store the original material
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Pole") && !isColliding) // Check for collision with object tagged as "Pole"
            {
                isColliding = true;
                renderer.material = collisionMaterial; // Change the material
                Invoke("ResetMaterial", duration); // Schedule material reset
            }
        }

        void ResetMaterial()
        {
            renderer.material = originalMaterial; // Reset to the original material
            isColliding = false;
        }
    }

}