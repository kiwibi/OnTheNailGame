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

    public GameObject transition;
    private Animator transitionAnimator;

    private void Awake()
    {
        transitionAnimator = Instantiate(transition, transform.Find("Canvas").transform).GetComponent<Animator>();
    }

    void Start()
    {
        swingText.text = 0.ToString();
        parText.text = 0.ToString();
        stageNumber.text = 0.ToString();
    }

    void Update()
    {
        swingText.text = gameHandler.GetComponent<GameHandler>().GetSwings().ToString();
        parText.text = "Par " + gameHandler.GetComponent<GameHandler>().GetPar().ToString();
        stageNumber.text = "Stage " + gameHandler.GetComponent<GameHandler>().GetStage().ToString();
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
