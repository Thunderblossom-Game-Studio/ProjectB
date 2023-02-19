using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CYLevelStreaming : MonoBehaviour
{
    void Start()
    {

        Scene currentScenePvE = SceneManager.GetActiveScene();

        string sceneName = currentScenePvE.name;

        if (sceneName == "PvELevel1Part3")
        {
            SceneManager.LoadScene("PvELevel1Part1", LoadSceneMode.Additive);
            SceneManager.LoadScene("PvELevel1Part2", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("PvELevel1Part3");
        }
    }
}
