using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levlum {
    public enum Interaction_types
    {
        OBJECT_UP, VACUUM_CLEANER, PUSH_OBJECT, PUSH_OBJECT_UP
    }

    public class camera_ray : MonoBehaviour
    {
        public Interaction_types initeraction_type;
        public LayerMask ignoreMe;
        public Transform crosshair_t;
        public Transform push_object;
        public float pushing_strength = 1f;

        public GameObject[] jumpy_objects;
        public GameObject[] away_objects;

        private int gameplay_layer = 3;
        private int push_object_layer = 7;

        // Start is called before the first frame update
        void Start()
        {
            // jumpy_objects = GameObject.FindGameObjectsWithTag("jumpy");
            // away_objects = GameObject.FindGameObjectsWithTag("away");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            RaycastHit hit;
            var ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            crosshair_t.position = new Vector3( crosshair_t.position.x, -1f, crosshair_t.position.z); //below floor

            switch (initeraction_type)
            {
                case Interaction_types.OBJECT_UP:
                    ignoreMe &= ~(1 << gameplay_layer); //remove gameplay_layer
                    ignoreMe &= ~(1 << push_object_layer); //remove  push_object_layer

                    if (Physics.Raycast(ray, out hit, 500f, ~ignoreMe) && hit.rigidbody != null) {
                        hit.rigidbody.AddForce(Vector3.up, ForceMode.Impulse);
                    }
                break;


                case Interaction_types.VACUUM_CLEANER:
                    ignoreMe |= (1 << gameplay_layer); //add gameplay_layer
                    ignoreMe |= (1 << push_object_layer); //add push_object_layer
                    if (Physics.Raycast(ray, out hit, 500f, ~ignoreMe) && hit.collider.name == "Floor") {
                        
                        crosshair_t.position = new Vector3( hit.point.x , 0.01f, hit.point.z); //set crosshair position
                        //use all objects with tag "jumpy"
                        foreach (var one_jumpy in jumpy_objects)
                        {
                            Vector3 power_direction = hit.point - one_jumpy.GetComponent<Transform>().position;
                            float length = power_direction.magnitude;
                            one_jumpy.GetComponent<Rigidbody>().AddForce(power_direction.normalized * 50f/(length) * pushing_strength, ForceMode.Force);
                        }
                        foreach (var one_o in away_objects)
                        {
                            Vector3 power_direction = hit.point - one_o.GetComponent<Transform>().position;
                            float length = power_direction.magnitude;
                            one_o.GetComponent<Rigidbody>().AddForce(-power_direction.normalized * 50f/(length) * pushing_strength, ForceMode.Force);
                        }
                    }
                break;


                case Interaction_types.PUSH_OBJECT_UP:
                    ignoreMe |= (1 << gameplay_layer); //add gameplay_layer
                    ignoreMe |= (1 << push_object_layer); //add push_object_layer
                    if (!Input.GetMouseButton(0)){
                        push_object.GetComponent<Rigidbody>().isKinematic = true; //make push object not physical
                        push_object.position = new Vector3( push_object.position.x , -1f, push_object.position.z);
                    }
                    if (Physics.Raycast(ray, out hit, 500f, ~ignoreMe) && hit.collider.name == "Floor") {
                        crosshair_t.position = new Vector3( hit.point.x , 0.01f, hit.point.z); //above floor
                        if (Input.GetMouseButton(0)){
                            if (push_object.GetComponent<Rigidbody>().isKinematic){
                                push_object.position = new Vector3( hit.point.x , Mathf.Max(0f,push_object.position.y), hit.point.z);
                                push_object.GetComponent<Rigidbody>().isKinematic = false;

                            }
                        }
                    } 
                     if (Input.GetMouseButton(0)){
                        push_object.GetComponent<Rigidbody>().AddForce(Vector3.up * pushing_strength * 0.01f, ForceMode.Impulse);
                    }
                break;


                case Interaction_types.PUSH_OBJECT:
                    ignoreMe |= (1 << gameplay_layer); //add gameplay_layer
                    ignoreMe |= (1 << push_object_layer); //add push_object_layer
                    
                    if (Physics.Raycast(ray, out hit, 500f, ~ignoreMe) && hit.collider.name == "Floor") {
                        Vector3 power_direction = hit.point - push_object.GetComponent<Transform>().position;
                        float length = power_direction.magnitude;
                        push_object.GetComponent<Rigidbody>().AddForce(power_direction * 1000f * pushing_strength + power_direction.normalized * 5, ForceMode.Force);
                    }
                break;
            }
        }
    }
}
//his