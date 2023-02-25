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
}
