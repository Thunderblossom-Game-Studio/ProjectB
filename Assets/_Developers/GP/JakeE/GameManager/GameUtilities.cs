using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtilities : MonoBehaviour
{
    public static bool GamePaused { get; set; }
    
    public static void PauseGame()
    {
        GamePaused = true;
    }

    public static void ResumeGame()
    {
        GamePaused = false;
    }

    public static void SlowMotion(bool status)
    {
        //TODO Game SlowMotion
    }
}
