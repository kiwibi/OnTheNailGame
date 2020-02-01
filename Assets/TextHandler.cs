using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{
    public Text swingText;
    public Text parText;
    public Text stageNumber;

    private GameObject gameHandler;

    void Start()
    {
        swingText.text = 0.ToString();
        parText.text = 0.ToString();
        stageNumber.text = 0.ToString();
    }

    void Update()
    {
        swingText.text = gameHandler.GetComponent<GameHandler>().GetSwings().ToString();
        parText.text = gameHandler.GetComponent<GameHandler>().GetPar().ToString();
        stageNumber.text = gameHandler.GetComponent<GameHandler>().GetStage().ToString();
    }

    public void SetGameHandler(GameObject GH)
    {
        gameHandler = GH;
    }
}
