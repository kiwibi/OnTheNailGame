using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{
    public Text swingText;
    public Text totalSwingText;
    public Text stageNumber;

    void Start()
    {
        swingText.text = 0.ToString();
        totalSwingText.text = 0.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
