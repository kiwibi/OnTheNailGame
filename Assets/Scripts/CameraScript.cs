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
    private float zoom;

    private const float maximumZoom = 15;
    private const float minimumZoom = 1;
    private const float zoomDiv = 30;
    private float cameraPointZoom = 0;

    enum CameraState { swing, nail, transition, flying, flyingaway };
    private CameraState cameraState;

    private List<FollowCamera> cameraList;

    void Awake()
    {
        //  Sets the original z position
        posZ = transform.position.z;

        //  Sets the zoom
        zoom = maximumZoom;

        // Sets initial camera state
        cameraState = CameraState.nail;

        //  Initializes cameraList
        cameraList = new List<FollowCamera>();

        //  Initializes first camera
        SetCamera("nail", GameObject.FindGameObjectWithTag("Nail").transform.position);
    }

    void LateUpdate()
    {
        {
            // Updates camera point zoom
            cameraPointZoom = GameObject.FindGameObjectWithTag("CameraPoint").GetComponent<CameraPoint>().zoom;

            //  Updates the position variables
            UpdateCameraState();

            //  Updates the position
            transform.position = new Vector3(posX, posY, posZ);

            //  Updates the zoom 
            GetComponent<Camera>().orthographicSize = zoom;

            //  Debug
            //Debug.Log("Camera X: " + posX + " Camera Y: " + posY + " Camera zoom: " + zoom);
        }
    }

    private void UpdateCameraState()
    {
        if (cameraList.Count > 0)
        {
            if (cameraState == CameraState.flyingaway)
            {
                if (cameraList[0].UpdateCamera() == true)
                {
                    SetCamera("flying", cameraList[0].GetStartPosition(), cameraList[0].GetEndPosition());
                }
            }
            else if (cameraState == CameraState.nail)
            {
                if (cameraList[0].UpdateCamera() == true)
                {
                    SetCamera("transition", GameObject.FindGameObjectWithTag("Nail").transform.position, (GameObject.FindGameObjectWithTag("CameraPoint").transform.position), transform.position);
                }
            }
            else if (cameraState == CameraState.transition)
            {
                if (cameraList[0].UpdateCamera() == true)
                {
                    //SetCamera("swing", GameObject.FindGameObjectWithTag("Swing").transform.Find("Swingpoint").transform.position, GameObject.FindGameObjectWithTag("Hammer").transform.position, GameObject.FindGameObjectWithTag("Nail").transform.position);
                    //GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().SetStartSequence(false);
                }
            }
            else
            {
                cameraList[0].UpdateCamera();
            }
            
            Vector2 newPos = cameraList[0].GetCameraPosition();
            posX = newPos.x;
            posY = newPos.y;
            zoom = cameraList[0].GetZoom();
        }
        else
        {
            posX = 0;
            posY = 0;
        }
    }

    public void SetCamera(string cameraType, Vector2 position)
    {
        switch (cameraType)
        {
            case "nail":
                cameraState = CameraState.nail;
                cameraList.Clear();
                cameraList.Add(new NailCamera(position));
                break;
        }
    }

    public void SetCamera(string cameraType, Vector2 positionA, Vector2 positionB)
    {
        switch (cameraType)
        {
            case "flying":
                cameraState = CameraState.flying;
                cameraList.Clear();
                cameraList.Add(new FlyingCamera(positionA, positionB));
                break;
        }
    }

    public void SetCamera(string cameraType, Vector2 positionA, Vector2 positionB, Vector2 positionC)
    {
        switch (cameraType)
        {
            case "swing":
                cameraState = CameraState.swing;
                cameraList.Clear();
                cameraList.Add(new SwingCamera(positionA, positionB, positionC));
                break;

            case "flyingaway":
                if (cameraType == "flyingaway")
                {
                    cameraState = CameraState.flyingaway;
                    cameraList.Clear();
                    cameraList.Add(new FlyAwayCamera(positionA, positionB, positionC));
                }
                break;

            case "transition":
                cameraState = CameraState.transition;
                cameraList.Clear();
                cameraList.Add(new TransitionCamera(positionA, positionB, cameraPointZoom));
                break;
        }
    }

    public void SetHammerPosition(Vector2 hammerPosition)
    {
        switch (cameraState)
        {
            case CameraState.flying:
                cameraList[0].UpdateStartPosition(hammerPosition);
                break;

            case CameraState.flyingaway:
                cameraList[0].UpdateStartPosition(hammerPosition);
                break;

            case CameraState.swing:
                cameraList[0].UpdateStartPosition(hammerPosition);
                break;
        }
    }

    protected abstract class FollowCamera
    {
        public virtual Vector2 GetCameraPosition()
        {
            return Vector2.zero;
        }

        public virtual Vector2 GetStartPosition()
        {
            return Vector2.zero;
        }

        public virtual Vector2 GetEndPosition()
        {
            return Vector2.zero;
        }

        public virtual float GetZoom()
        {
            return 0;
        }

        public virtual bool UpdateCamera()
        {
            return false;
        }

        public virtual bool UpdateStartPosition(Vector2 startPos)
        {
            return false;
        }

        public virtual bool UpdateEndPosition(Vector2 endPos)
        {
            return false;
        }

        public virtual float GetTransitionRatio()
        {
            return 0;
        }
    };

    private class NailCamera:FollowCamera
    {
        private Vector2 nailPosition;
        private const float zoom = minimumZoom;
        private float timer = 2.5f;

        public NailCamera(Vector2 nailPos)
        {
            nailPosition = nailPos;
        }

        public override Vector2 GetCameraPosition()
        {
            return nailPosition;
        }

        public override float GetZoom()
        {
            return zoom;
        }

        public override bool UpdateCamera()
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    };

    private class SwingCamera:FollowCamera
    {
        private FlyingCamera flyingCamera;
        private Vector2 swingPosition;
        private Vector2 hammerPosition;
        private Vector2 nailPosition;
        private Vector2 cameraPosition;
        private float zoom;

        public SwingCamera(Vector2 swingPos, Vector2 hammerPos, Vector2 nailPos)
        {
            flyingCamera = new FlyingCamera(hammerPos, nailPos);
            swingPosition = swingPos;
            hammerPosition = hammerPos;
            nailPosition = nailPos;
            zoom = Mathf.Lerp(minimumZoom, maximumZoom, Vector2.Distance(swingPos, nailPos) / zoomDiv);
        }

        public override Vector2 GetCameraPosition()
        {
            return flyingCamera.GetCameraPosition();
        }

        public override float GetZoom()
        {
            return zoom;
        }

        public override bool UpdateCamera()
        {
            flyingCamera.UpdateCamera();
            
            return false;
        }

        public override bool UpdateStartPosition(Vector2 startPos)
        {
            flyingCamera.UpdateStartPosition(startPos);

            return true;
        }
    };

    private class FlyingCamera: FollowCamera
    {
        private Vector2 cameraPosition;
        private Vector2 positionA;
        private Vector2 positionB;
        private float zoom;
        private const float zoomDivider = 30;
        private const float maxZoom = maximumZoom;
        private const float minZoom = minimumZoom;

        public FlyingCamera(Vector2 hammerPos, Vector2 nailPos)
        {
            positionA = hammerPos;
            positionB = nailPos;
        }

        public override Vector2 GetCameraPosition()
        {
            return cameraPosition;
        }

        public override Vector2 GetStartPosition()
        {
            return positionA;
        }

        public override Vector2 GetEndPosition()
        {
            return positionB;
        }

        public override float GetZoom()
        {
            return zoom;
        }

        public override bool UpdateCamera()
        {
            cameraPosition.x = (positionA.x + positionB.x) / 2;
            cameraPosition.y = (positionA.y + positionB.y) / 2;

            zoom = Mathf.Lerp(minZoom, maxZoom, Vector2.Distance(positionA, positionB) / zoomDivider);

            return false;
        }

        public override bool UpdateStartPosition(Vector2 startPos)
        {
            positionA = startPos;

            return true;
        }
    };

    private class TransitionCamera : FollowCamera
    {
        private Vector2 positionA;
        private Vector2 positionB;
        private Vector2 cameraPosition;
        private float zoom = minimumZoom;
        private const float startZoom = minimumZoom;
        private const float endZoom = maximumZoom;
        private float pZoom;
        private float transitionState = 0;
        private float transitionLength = 0;
        private const float transitionDivider = 3;

        public TransitionCamera(Vector2 startPos, Vector2 endPos, float pointZoom)
        {
            positionA = startPos;
            positionB = endPos;
            float distance = Vector2.Distance(startPos, endPos);
            transitionLength = distance / transitionDivider;
            cameraPosition = positionA;
            zoom = startZoom;
            pZoom = pointZoom;
        }

        public override Vector2 GetCameraPosition()
        {
            return cameraPosition;
        }

        public override float GetZoom()
        {
            return zoom;
        }

        public override bool UpdateCamera()
        {
            if (transitionState >= transitionLength)
            {
                return true;
            }

            transitionState += Time.deltaTime;

            cameraPosition = positionA + ((positionB - positionA) * (transitionState / transitionLength));

            zoom = Mathf.Lerp(startZoom, pZoom, Vector2.Distance(positionA, cameraPosition) / zoomDiv);

            return false;
        }

        public override bool UpdateEndPosition(Vector2 endPos)
        {
            positionB = endPos;
            float distance = Vector2.Distance(positionA, positionB);
            transitionLength = distance / transitionDivider;

            return true;
        }

        public override float GetTransitionRatio()
        {
            return transitionState / transitionLength;
        }
    };

    private class FlyAwayCamera : FollowCamera
    {
        private FollowCamera flyingCamera;
        private FollowCamera transitionCamera;
        private Vector2 cameraPosition;
        private float zoom = maximumZoom;

        public FlyAwayCamera(Vector2 swingPos, Vector2 hammerPos, Vector2 nailPos)
        {
            flyingCamera = new FlyingCamera(hammerPos, nailPos);
            transitionCamera = new TransitionCamera(swingPos, hammerPos, 0);
            cameraPosition = swingPos;
        }

        public override Vector2 GetCameraPosition()
        {
            return cameraPosition;
        }

        public override Vector2 GetStartPosition()
        {
            return flyingCamera.GetStartPosition();
        }

        public override Vector2 GetEndPosition()
        {
            return flyingCamera.GetEndPosition();
        }

        public override float GetZoom()
        {
            return zoom;
        }

        public override bool UpdateCamera()
        {
            flyingCamera.UpdateCamera();
            transitionCamera.UpdateEndPosition(flyingCamera.GetCameraPosition());

            if (transitionCamera.UpdateCamera() == true)
            {
                return true;
            }

            zoom = flyingCamera.GetZoom() + (transitionCamera.GetZoom() * (1 - transitionCamera.GetTransitionRatio()));
            cameraPosition = transitionCamera.GetCameraPosition();

            return false;
        }

        public override bool UpdateStartPosition(Vector2 startPos)
        {
            flyingCamera.UpdateStartPosition(startPos);

            return true;
        }
    };
}
