using UnityEngine;

namespace Nuss
{
    public class MaterialChanger : MonoBehaviour
    {
        public Material collisionMaterial; // Material to switch to on collision
        public float duration = 5f; // Duration to keep the material

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
            if (collision.collider.CompareTag("Sphere") && !isColliding) // Check for collision with object tagged as "Sphere"
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