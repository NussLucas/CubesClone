using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nuss{
public class camera_ray_simple : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            RaycastHit hit;
            
            var ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50f) && hit.rigidbody != null){
                hit.rigidbody.AddForce(Vector3.up, ForceMode.Impulse);
            }
        }
    }
}
