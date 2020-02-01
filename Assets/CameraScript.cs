using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject objectFollowing;
    private Transform transformFollowing;
    private float posX;
    private float posY;
    private float posZ;

    public float originX;
    public float originY;
    public float minObjX;
    public float maxObjX;
    public float minObjY;
    public float maxObjY;
    public float adjustmentX;
    public float adjustmentY;

    private float minCameraX;
    private float maxCameraX;
    private float minCameraY;
    private float maxCameraY;


    void Start()
    {
        posZ = transform.position.z;

        SetCameraPositions();
    }

    void Update()
    {
        if (objectFollowing != null)
        {
            UpdatePosition();
            transform.position = new Vector3(posX, posY, posZ);
            Debug.Log("Camera X: " + posX + " Camera Y: " + posY);
        }
    }

    private void SetOrigin()
    {

    }

    private void SetCameraPositions()
    {
        minCameraX = minObjX + adjustmentX;
        maxCameraX = maxObjX - adjustmentX;
        minCameraY = minObjY + adjustmentY;
        maxCameraY = maxObjY - adjustmentY;
    }

    private void UpdatePosition()
    {
        if (transformFollowing.position.x < minObjX)
        {
            posX = minCameraX;
        }
        else if (transformFollowing.position.x > maxObjX)
        {
            posX = maxCameraX;
        }
        else
        {
            if (transformFollowing.position.x < 0)
            {
                posX = minCameraX * Mathf.Abs((transformFollowing.position.x / minObjX));
            }
            else if (transformFollowing.position.x > 0)
            {
                posX = maxCameraX * Mathf.Abs((transformFollowing.position.x / maxObjX));
            }
            else
            {
                posX = 0;
            }
        }

        if (transformFollowing.position.y < minObjY)
        {
            posY = minCameraY;
        }
        else if (transformFollowing.position.y > maxObjY)
        {
            posY = maxCameraY;
        }
        else
        {
            if (transformFollowing.position.y < 0)
            {
                posY = minCameraY * Mathf.Abs((transformFollowing.position.y / minObjY));
            }
            else if (transformFollowing.position.y > 0)
            {
                posY = maxCameraY * Mathf.Abs((transformFollowing.position.y / maxObjY));
            }
            else
            {
                posY = 0;
            }
        }
    }
    /*
    private void SetCameraMinMaxPosition(float minx, float maxx, float miny, float maxy)
    {
        minX = minx;
        maxX = maxx;
        minY = miny;
        maxX = maxy;
    }
    */
    public void SetObjectFollowing(GameObject obj)
    {
        objectFollowing = obj;
        transformFollowing = objectFollowing.transform;
        Debug.Log("Object Following Set");
    }

    public GameObject GetObjectFollowing()
    {
        return objectFollowing;
    }
}
