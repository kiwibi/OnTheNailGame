using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenScript : MonoBehaviour
{
    public void EndEndScreen()
    {
        GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>().GoToScene("Menu");
    }
}
