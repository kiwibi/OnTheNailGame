using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    public float zoom;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
