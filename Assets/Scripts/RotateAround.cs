using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject OrbitPoint_;
    private Rigidbody2D HammerBody_;
    private Transform HandleFlash_;
    private GameObject Sling_;
    private GameObject[] walls_;

    public float orbitDistance_;
    [Header("Speed variables")]
    [Tooltip("speed that increases every frame its held")]
    public float speedIncrease_;
    [Tooltip("start speed")]
    public float orbitSpeed_;
    [Tooltip("orbit speed cap")]
    public float orbitSpeedCap_;
    public float yeetSpeedCap_;
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
    [Tooltip("area to check for wall closeness")]
    public float wallArea_;
    [Tooltip("how far the swing gets pushed if inside wallArea_")]
    public float wallPushback_;
    public float amountOfBounces_;

    [Header("VFX files")]
    public GameObject[] VFXFiles_;

    private bool sent_;
    private float orbit_; //radian degree toward the center object
    private float speedReset_;
    private float throttleReset_;
    private Vector3 tempPos_;
    private Vector3 releaseDirection_;
    private float gravityScale_;
    private bool swinging_;
    private bool introScene_;
    private Quaternion startRotation_;
    private float amountOfBounce_;
    private float bounceAcumelator_;
    private float yeetNumber_;

    void Start()
    {
        HammerBody_ = GetComponent<Rigidbody2D>();
        HandleFlash_ = gameObject.transform.GetChild(0);
        Sling_ = GameObject.FindGameObjectWithTag("Swing");
        tempPos_ = new Vector3(0, 0, 0);
        speedReset_ = orbitSpeed_;
        orbit_ = (Mathf.PI / 2) * 3;
        gravityScale_ = HammerBody_.gravityScale;
        HammerBody_.gravityScale = 0;
        HammerBody_.angularVelocity = 0;
        swinging_ = true;
        introScene_ = true;
        startRotation_ = HammerBody_.transform.rotation;
        throttleReset_ = spinThrottle_;
        amountOfBounce_ = amountOfBounces_;
        bounceAcumelator_ = 0;
        sent_ = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (introScene_)
        {
  
            Rotate();

            HammerState(swinging_);

            if (Input.GetKey(KeyCode.Space))
            {
                bounceAcumelator_ += 1 * Time.deltaTime;
                resetSwing();
            }
        }
    }

    void Rotate()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && swinging_ == true)
        {
            orbit_ -= orbitSpeed_ * Time.deltaTime / 10;
            tempPos_.x = OrbitPoint_.transform.position.x + Mathf.Cos(orbit_) * orbitDistance_;
            tempPos_.y = OrbitPoint_.transform.position.y + Mathf.Sin(orbit_) * orbitDistance_;
            tempPos_.z = transform.position.z;
            transform.position = tempPos_;
            if(transform.rotation.eulerAngles.z < 180 && transform.rotation.eulerAngles.z > 175)
                FindObjectOfType<AudioManager>().Play("The swosh");

            if (orbitSpeed_ < orbitSpeedCap_)
                orbitSpeed_ += speedIncrease_;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && swinging_ == true)
        {
            orbit_ += orbitSpeed_ * Time.deltaTime / 10;
            tempPos_.x = OrbitPoint_.transform.position.x + Mathf.Cos(orbit_) * orbitDistance_;
            tempPos_.y = OrbitPoint_.transform.position.y + Mathf.Sin(orbit_) * orbitDistance_;
            tempPos_.z = transform.position.z;
            transform.position = tempPos_;
            if (transform.rotation.eulerAngles.z < 180 && transform.rotation.eulerAngles.z > 175)
                FindObjectOfType<AudioManager>().Play("The swosh");
            if (orbitSpeed_ < orbitSpeedCap_)
                orbitSpeed_ += speedIncrease_;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))//Yeet that hammer
        {
            if (HammerBody_.velocity == Vector2.zero)
            {
                yeetNumber_ = Random.Range(0, 100);

                if(yeetSpeedCap_ < orbitSpeed_ && yeetNumber_ <= 10)
                    FindObjectOfType<AudioManager>().Play("ThrowYeet");
                else
                    FindObjectOfType<AudioManager>().Play("Throw");
                //releaseDirection_ = calculateTan(OrbitPoint_.transform.position, HammerBody_.transform.position);
                releaseDirection_ = Vector2.Perpendicular(OrbitPoint_.transform.position - HammerBody_.transform.position);
                float magnitude = releaseDirection_.magnitude;
                releaseDirection_ = releaseDirection_ / magnitude;
                releaseDirection_ *= -1;
                HammerBody_.AddForce(releaseDirection_ * forceMultiplier_ * orbitSpeed_);
                swinging_ = false;
                GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().AddSwing();
            }
           // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().SetCamera("flyingaway", GameObject.FindGameObjectWithTag("Swing").transform.position, GameObject.FindGameObjectWithTag("Hammer").transform.position, GameObject.FindGameObjectWithTag("Nail").transform.position);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (HammerBody_.velocity == Vector2.zero)
            {
                yeetNumber_ = Random.Range(0, 100);

                if (yeetSpeedCap_ < orbitSpeed_ && yeetNumber_ <= 10)
                    FindObjectOfType<AudioManager>().Play("ThrowYeet");
                else
                    FindObjectOfType<AudioManager>().Play("Throw");
                //releaseDirection_ = calculateTan(OrbitPoint_.transform.position, HammerBody_.transform.position);
                releaseDirection_ = Vector2.Perpendicular(OrbitPoint_.transform.position - HammerBody_.transform.position);
                float magnitude = releaseDirection_.magnitude;
                releaseDirection_ = releaseDirection_ / magnitude;
                HammerBody_.AddForce(releaseDirection_ * forceMultiplier_ * orbitSpeed_);
                swinging_ = false;
                GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().AddSwing();
            }
           // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().SetCamera("flyingaway", GameObject.FindGameObjectWithTag("Swing").transform.position, GameObject.FindGameObjectWithTag("Hammer").transform.position, GameObject.FindGameObjectWithTag("Nail").transform.position);
        }
    }
    void resetSwing()
    {
        amountOfBounces_ = amountOfBounce_;
        swinging_ = true;
        transform.position = new Vector3(OrbitPoint_.transform.position.x, OrbitPoint_.transform.position.y - orbitDistance_, transform.position.z);
        HammerBody_.velocity = Vector3.zero;
        HammerBody_.transform.rotation = startRotation_;
        HammerBody_.angularVelocity = 0;
        orbit_ = (Mathf.PI / 2) * 3;
        orbitSpeed_ = speedReset_;
        spinThrottle_ = throttleReset_;
       // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().SetCamera("swing", GameObject.FindGameObjectWithTag("Swing").transform.position);
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (bounceAcumelator_ < 2)
            bounceAcumelator_ += 1*Time.deltaTime;
        else
        {
            resetSwing();
            bounceAcumelator_ = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D col_)
    {
        if (col_.gameObject.tag == "NoSpawn")
            return;
        if (col_.otherCollider.bounciness == 0.5f)
        {
            //Instantiate(VFXFiles_[2], HandleFlash_.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("Bounce");
        }
        else
        {
            if (col_.gameObject.transform.tag != "Nail")
            {
                Instantiate(VFXFiles_[2], HandleFlash_.position, Quaternion.identity);
                FindObjectOfType<AudioManager>().Play("Bounce");
                if (amountOfBounces_ > 0 && swinging_ == false)
                {
                    amountOfBounces_--;
       
                }
                if (amountOfBounces_ == 0)
                {
                    
                    HammerBody_.velocity = new Vector2(0, 0);
                    float dist = Vector3.Distance(transform.position, Sling_.transform.position);
                    if (dist > 1.5f /*&& dist > 0*/|| dist < -1.5f/* && dist < 0*/)
                    {
                        float yValue = col_.transform.position.y + 0.95f;
                        if (col_.transform.tag == "Geometry")
                        {
                            yValue = col_.collider.bounds.min.y + 0.75f;
                        }
                           
                        Vector3 distance = CheckWallDistance(new Vector3(col_.otherCollider.transform.position.x, yValue, Sling_.transform.position.z));
                        Sling_.transform.position = distance;
                    }
                    resetSwing();
                }
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("NailHit");
                switch (col_.transform.rotation.eulerAngles.z)
                {
                    case 0:     //top
                        if (col_.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                        {
                            Instantiate(VFXFiles_[0], col_.transform);
                            col_.transform.position = new Vector3(col_.transform.position.x, col_.transform.position.y - 0.26f, col_.transform.position.z);
                            
                            col_.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                            FindObjectOfType<AudioManager>().Play("Goodjob");
                        }
                        break;
                    case 90:     //right
                        if (col_.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                        {
                            Instantiate(VFXFiles_[0], col_.transform);
                            col_.transform.position = new Vector3(col_.transform.position.x + 0.26f, col_.transform.position.y, col_.transform.position.z);
                            
                            col_.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                            FindObjectOfType<AudioManager>().Play("Goodjob");
                        }
                        break;
                    case 180:     //bottom
                        if (col_.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                        {
                            Instantiate(VFXFiles_[0], col_.transform);
                            col_.transform.position = new Vector3(col_.transform.position.x, col_.transform.position.y + 0.26f, col_.transform.position.z);
                            
                            col_.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                            FindObjectOfType<AudioManager>().Play("Goodjob");
                        }
                        break;
                    case 270:     //left
                        if (col_.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                        {
                            Instantiate(VFXFiles_[0], col_.transform);
                            col_.transform.position = new Vector3(col_.transform.position.x - 0.26f, col_.transform.position.y, col_.transform.position.z);
                           
                            col_.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                            FindObjectOfType<AudioManager>().Play("Goodjob");
                        }
                        break;
                }
                if(sent_ == false)
                {
                    GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().GoToScene("NextLevel");
                    sent_ = true;
                }
                
            }
        }      
    }

    private Vector3 CheckWallDistance(Vector3 newPos)
    {
        float distance;
        walls_ = GameObject.FindGameObjectsWithTag("Geometry");
        foreach(GameObject wall in walls_)
        {
            Collider2D wallColl_ = wall.GetComponent<Collider2D>();
            if (wallColl_.bounds.max.y > newPos.y && wallColl_.bounds.min.y < newPos.y)
            {
                
                distance = newPos.x - wall.transform.position.x;
                if (distance > -wallArea_ && distance < 0)
                {
                    newPos.x = wall.transform.position.x - wallPushback_;
                }
                else if (distance < wallArea_ && distance > 0)
                {
                    newPos.x = wall.transform.position.x + wallPushback_;
                }
            }
            
        }
        return newPos;
    }

    public bool isSwinging()
    {
        return swinging_;
    }

    public void setIntroSwing(bool isItPlaying)
    {
        introScene_ = isItPlaying;
    }
}
