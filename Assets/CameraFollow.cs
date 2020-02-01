using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject camera;
    public GameObject objectA;
    public GameObject objectB;
    
    // Start is called before the first frame update
    void Start()
    {
        // camera.GetComponent<CameraScript>().SetCamera("flyingaway",objectA.transform.position,transform.position,objectB.transform.position);
        camera.GetComponent<CameraScript>().SetCamera("swing", objectA.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        camera.GetComponent<CameraScript>().SetHammerPosition(transform.position);
    }
}
