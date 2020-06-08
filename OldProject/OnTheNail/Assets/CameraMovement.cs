using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{

    public List<Transform> targets;

    public Vector3 offset;

    public float zoomLimiter;
    public float minZoom = 40f;
    public float maxZoom = 10f;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }


    void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = newPosition;

        Zoom();
        //Zoom("y");

    }
    
    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGratestDistance() / zoomLimiter);
        cam.orthographicSize = newZoom;
    }

    float GetGratestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        
        if (bounds.size.x >= bounds.size.y * 1.8f)
            return bounds.size.x;
        else
            return bounds.size.y * 1.8f;
        
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    } 
}

