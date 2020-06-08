using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void StartGame()
    {
        GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().GoToScene("StartGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
