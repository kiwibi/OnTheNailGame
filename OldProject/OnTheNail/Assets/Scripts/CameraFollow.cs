using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject camera;
    public GameObject objectA;
    public GameObject objectB;

    void Update()
    {
        camera.GetComponent<CameraScript>().SetHammerPosition(transform.position);

        
    }
}
