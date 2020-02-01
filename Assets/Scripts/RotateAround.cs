using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject OrbitPoint_;
    public GameObject Anchor_;
    private Rigidbody2D HammerBody_;

    public float orbitDistance_;
    [Header("Speed variables")]
    [Tooltip("speed that increases every frame its held")]
    public float speedIncrease_;
    [Tooltip("start speed")]
    public float orbitSpeed_;
    [Tooltip("orbit speed cap")]
    public float orbitSpeedCap_;
    [Tooltip("Force multiplier when releasing hammer")]
    public float forceMultiplier_;
    private float orbit_; //radian degree toward the center object
    private float speedReset_;
    private Vector3 tempPos_;
    private Vector3 releaseDirection_;
    private float gravityScale_;
    private bool swinging_;
    private Quaternion startRotation_;

    void Start()
    {
        HammerBody_ = GetComponent<Rigidbody2D>();
        tempPos_ = new Vector3(0, 0, 0);
        speedReset_ = orbitSpeed_;
        gravityScale_ = HammerBody_.gravityScale;
        HammerBody_.gravityScale = 0;
        swinging_ = true;
        startRotation_ = HammerBody_.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();

        HammerState(swinging_);

        if(Input.GetKey(KeyCode.Space))
        {
            swinging_ = true;
            transform.position = new Vector3(Anchor_.transform.position.x + orbitDistance_, Anchor_.transform.position.y, Anchor_.transform.position.z);
            
            resetSwing();
        }
    }

    void Rotate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            orbit_ -= orbitSpeed_ * Time.deltaTime / 10;
            tempPos_.x = OrbitPoint_.transform.position.x + Mathf.Cos(orbit_) * orbitDistance_;
            tempPos_.y = OrbitPoint_.transform.position.y + Mathf.Sin(orbit_) * orbitDistance_;
            transform.position = tempPos_;
            if (orbitSpeed_ < orbitSpeedCap_)
                orbitSpeed_ += speedIncrease_;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            orbit_ += orbitSpeed_ * Time.deltaTime / 10;
            tempPos_.x = OrbitPoint_.transform.position.x + Mathf.Cos(orbit_) * orbitDistance_;
            tempPos_.y = OrbitPoint_.transform.position.y + Mathf.Sin(orbit_) * orbitDistance_;
            transform.position = tempPos_;
            if (orbitSpeed_ < orbitSpeedCap_)
                orbitSpeed_ += speedIncrease_;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))//Yeet that hammer
        {
            releaseDirection_ = calculateTan(OrbitPoint_.transform.position, HammerBody_.transform.position);
            float magnitude = releaseDirection_.magnitude;
            releaseDirection_ = releaseDirection_ / magnitude;
            releaseDirection_ *= -1;
            HammerBody_.AddForce(releaseDirection_ * forceMultiplier_ * orbitSpeed_);
            swinging_ = false;

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().SetCamera("flyingaway", GameObject.FindGameObjectWithTag("Swing").transform.position, GameObject.FindGameObjectWithTag("Hammer").transform.position, GameObject.FindGameObjectWithTag("Nail").transform.position);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            releaseDirection_ = calculateTan(OrbitPoint_.transform.position, HammerBody_.transform.position);
            float magnitude = releaseDirection_.magnitude;
            releaseDirection_ = releaseDirection_ / magnitude;
            HammerBody_.AddForce(releaseDirection_ * forceMultiplier_ * orbitSpeed_);
            swinging_ = false;

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().SetCamera("flyingaway", GameObject.FindGameObjectWithTag("Swing").transform.position, GameObject.FindGameObjectWithTag("Hammer").transform.position, GameObject.FindGameObjectWithTag("Nail").transform.position);
        }
    }
    Vector3 calculateTan(Vector3 lhs, Vector3 rhs)
    {
        Vector3 normal_ = rhs - lhs;
        Vector3 tangent;
        Vector3 t1 = Vector3.Cross(normal_, Vector3.forward);
        Vector3 t2 = Vector3.Cross(normal_, Vector3.up);
        if (t1.magnitude > t2.magnitude)
        {
            tangent = t1;
        }
        else
        {
            tangent = t2;
        }
        return tangent;
    }

    void resetSwing()
    {
        HammerBody_.velocity = Vector3.zero;
        HammerBody_.transform.rotation = startRotation_;
        HammerBody_.angularVelocity = 0;
        orbit_ = 0;
        orbitSpeed_ = speedReset_;

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().SetCamera("swing", GameObject.FindGameObjectWithTag("Swing").transform.position);
    }

    void RotateSprite(bool free)
    {
        if (free)
        {
            //transform.Rotate(new Vector3(0, 0, 1), orbit_);
        }
        else
        {
            Vector3 dir = (OrbitPoint_.transform.position - HammerBody_.transform.position);
            Vector3 up = new Vector3(0, 0, 1);
            var rotation = Quaternion.LookRotation(dir, up);
            rotation.x = 0;
            rotation.y = 0;
            transform.rotation = rotation;
        }
    }

    void HammerState(bool swingin)
    {
        
        switch (swingin)
        {
            case true:
                HammerBody_.gravityScale = 0;
                RotateSprite(false);
                break;
            case false:
                HammerBody_.gravityScale = gravityScale_;
                RotateSprite(true);
                break;
        };
    }
}
