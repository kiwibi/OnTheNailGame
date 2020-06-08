using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanksScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().Play("Transition");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
