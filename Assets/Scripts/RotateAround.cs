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
    private float orbit_; //radian degree toward the center object
    private float speedReset_;
    private Vector3 tempPos_;
    private Vector3 releaseDirection_;

    void Start()
    {
        HammerBody_ = GetComponent<Rigidbody2D>();
        tempPos_ = new Vector3(0, 0, 0);
        speedReset_ = orbitSpeed_;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
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
            resetSwing();

            releaseDirection_ = calculateTan(OrbitPoint_.transform.position, transform.position);
            float magnitude = releaseDirection_.magnitude;
            releaseDirection_ = releaseDirection_ / magnitude;
            releaseDirection_ *= -1;
            HammerBody_.AddForce(releaseDirection_ * forceMultiplier_);
            
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            resetSwing();

            releaseDirection_ = calculateTan(OrbitPoint_.transform.position, HammerBody_.transform.position);
            float magnitude = releaseDirection_.magnitude;
            releaseDirection_ = releaseDirection_ / magnitude;
            HammerBody_.AddForce(releaseDirection_ * forceMultiplier_);
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
        orbitSpeed_ = speedReset_;
    }
}
