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
                par = 10;
                break;

            case "Level 2":
                par = 10;
                break;

            case "Level 3":
                par = 10;
                break;

            case "Level 4":
                par = 10;
                break;

            case "Level 5":
                par = 10;
                break;

            case "Level 6":
                par = 10;
                break;

            case "Level 7":
                par = 10;
                break;

            case "Level 8":
                par = 10;
                break;

            case "Level 9":
                par = 10;
                break;

            case "Level 10":
                par = 10;
                break;

            case "Level 11":
                par = 10;
                break;

            case "Level 12":
                par = 10;
                break;

            case "Level 13":
                par = 10;
                break;

            default:
                par = 10;
                break;
        }

        return par;
    }
}
