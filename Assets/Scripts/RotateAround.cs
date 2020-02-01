using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject OrbitPoint_;
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
    [Header("spin variables")]
    [Tooltip("big number that decreases spin speed       multiplication used")]
    public float spinThrottle_;
    [Tooltip("small number that decreses the spin throttle over time    division used")]
    public float spinSpeedDecrease_;
    [Header("collision variables")]
    [Tooltip("Amount of speed decresed by every collision   subtraction used")]
    public float collisionSpeedDecrease_;

    private float orbit_; //radian degree toward the center object
    private float speedReset_;
    private float throttleReset_;
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
        orbit_ = (Mathf.PI / 2) * 3;
        gravityScale_ = HammerBody_.gravityScale;
        HammerBody_.gravityScale = 0;
        HammerBody_.angularVelocity = 0;
        swinging_ = true;
        startRotation_ = HammerBody_.transform.rotation;
        throttleReset_ = spinThrottle_;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();

        HammerState(swinging_);

        if(Input.GetKey(KeyCode.Space))
        {
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
            tempPos_.z = transform.position.z;
            transform.position = tempPos_;
            if (orbitSpeed_ < orbitSpeedCap_)
                orbitSpeed_ += speedIncrease_;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            orbit_ += orbitSpeed_ * Time.deltaTime / 10;
            tempPos_.x = OrbitPoint_.transform.position.x + Mathf.Cos(orbit_) * orbitDistance_;
            tempPos_.y = OrbitPoint_.transform.position.y + Mathf.Sin(orbit_) * orbitDistance_;
            tempPos_.z = transform.position.z;
            transform.position = tempPos_;
            if (orbitSpeed_ < orbitSpeedCap_)
                orbitSpeed_ += speedIncrease_;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))//Yeet that hammer
        {
            //releaseDirection_ = calculateTan(OrbitPoint_.transform.position, HammerBody_.transform.position);
            releaseDirection_ = Vector2.Perpendicular(OrbitPoint_.transform.position - HammerBody_.transform.position);
            float magnitude = releaseDirection_.magnitude;
            releaseDirection_ = releaseDirection_ / magnitude;
            releaseDirection_ *= -1;
            HammerBody_.AddForce(releaseDirection_ * forceMultiplier_ * orbitSpeed_);
            swinging_ = false;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            //releaseDirection_ = calculateTan(OrbitPoint_.transform.position, HammerBody_.transform.position);
            releaseDirection_ = Vector2.Perpendicular(OrbitPoint_.transform.position - HammerBody_.transform.position);
            float magnitude = releaseDirection_.magnitude;
            releaseDirection_ = releaseDirection_ / magnitude;
            HammerBody_.AddForce(releaseDirection_ * forceMultiplier_ * orbitSpeed_);
            swinging_ = false;
        }
    }
    //Vector3 calculateTan(Vector3 lhs, Vector3 rhs)
    //{
    //    Vector3 normal_ = (rhs - lhs);
    //    Vector3 tangent;
    //    Vector3 t1 = Vector3.Cross(normal_, Vector3.forward);
    //    Vector3 t2 = Vector3.Cross(normal_, Vector3.up);
    //    if (t1.magnitude > t2.magnitude)
    //    {
    //        tangent = t1;
    //    }
    //    else
    //    {
    //        tangent = t2;
    //    }
    //    return tangent;
    //}

    void resetSwing()
    {
        swinging_ = true;
        transform.position = new Vector3(OrbitPoint_.transform.position.x, OrbitPoint_.transform.position.y - orbitDistance_, transform.position.z);
        HammerBody_.velocity = Vector3.zero;
        HammerBody_.transform.rotation = startRotation_;
        HammerBody_.angularVelocity = 0;
        orbit_ = (Mathf.PI / 2) * 3;
        orbitSpeed_ = speedReset_;
        spinThrottle_ = throttleReset_;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().SetCamera("swing", GameObject.FindGameObjectWithTag("Swing").transform.position);
    }

    void RotateSprite(bool free)
    {
        if (free)
        {

            transform.Rotate(new Vector3(0, 0, 1), orbit_* orbitSpeed_ / spinThrottle_);
            spinThrottle_ -= spinSpeedDecrease_;
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

    private void OnCollisionEnter2D(Collision2D col_)
    {
        if(col_.otherCollider.bounciness == 0.5f)
        {
            if (col_.gameObject.transform.tag != "Nail")
            {
               
                if (orbitSpeed_ > 0)
                    orbitSpeed_ -= collisionSpeedDecrease_;
                else
                    orbitSpeed_ = 0;
            }
        }
        else
        {
            if (col_.gameObject.transform.tag != "Nail")
            {
                if (orbitSpeed_ > 0)
                    orbitSpeed_ -= collisionSpeedDecrease_;
                else
                    orbitSpeed_ = 0;
            }
            else
            {
                col_.transform.position = new Vector3(col_.transform.position.x + 0.26f, col_.transform.position.y, col_.transform.position.z);
            }
        }      
    }
}
