using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nuss { 
public class SimpleMotor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
     GetComponent<Rigidbody>().AddForce(new Vector3(-2,Random.Range(0,2),1)*4);   
    }
}
}