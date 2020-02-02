using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{
    public Text swingText;
    public Text parText;
    public Text stageNumber;
    public Text endPar;

    private GameObject gameHandler;

    public GameObject transition;
    private Animator transitionAnimator;

    bool endText;

    private void Awake()
    {
        transitionAnimator = Instantiate(transition, transform.Find("Canvas").transform).GetComponent<Animator>();
        endText = false;
    }

    void Start()
    {
        swingText.text = 0.ToString();
        parText.text = 0.ToString();
        stageNumber.text = 0.ToString();
        endPar.text = "";
        endPar.enabled = false;
    }

    void Update()
    {
        if (!endText)
        {
            swingText.text = gameHandler.GetComponent<GameHandler>().GetSwings().ToString();
            parText.text = "Par " + gameHandler.GetComponent<GameHandler>().GetPar().ToString();
            stageNumber.text = "Stage " + gameHandler.GetComponent<GameHandler>().GetStage().ToString();
        }
    }

    public void SetEndScreen(int par)
    {
        endText = true;
        endPar.enabled = true;

        swingText.enabled = false;
        parText.enabled = false;
        stageNumber.enabled = false;

        if (par == 0)
        {
            parText.text = "You were on par! Well done!";
        }
        else if (par < 0)
        {
            parText.text = "You were " + par + " under par! You're amazing!";
        }
        else
        {
            parText.text = "You were " + par + " over par! Better luck next time!";
        }
    }

    public void SetGameHandler(GameObject GH)
    {
        gameHandler = GH;
    }

    public void PlayTransition()
    {
        transitionAnimator.SetTrigger("Start");
    }
}
