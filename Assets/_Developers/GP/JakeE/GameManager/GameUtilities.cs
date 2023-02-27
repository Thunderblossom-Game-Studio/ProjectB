using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtilities : MonoBehaviour
{
    public static bool GamePaused { get; set; }
    
    public static void PauseGame()
    {
        GamePaused = true;
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        GamePaused = false;
        Time.timeScale = 1;
    }

    public static void SlowMotion(bool status)
    {
        if (status) Time.timeScale = 0.5f;
        else Time.timeScale = 1;
    }
}
