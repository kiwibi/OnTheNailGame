using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPars : MonoBehaviour
{
    public int GetPar(string levelName)
    {
        int par = 10;

        switch (levelName)
        {
            case "Level 1":
                par = 2;
                break;

            case "Level 2":
                par = 2;
                break;

            case "Level 3":
                par = 3;
                break;

            case "Level 4":
                par = 3;
                break;

            case "Level 5":
                par = 2;
                break;

            case "Level 6":
                par = 3;
                break;

            case "Level 7":
                par = 4;
                break;

            case "Level 8":
                par = 4;
                break;

            case "Level 9":
                par = 5;
                break;

            default:
                par = 10;
                break;
        }

        return par;
    }
}
