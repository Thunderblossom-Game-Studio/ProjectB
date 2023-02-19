using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CYLevelStreamingPvP : MonoBehaviour
{
    void Start()
    {

        Scene currentScenePvP = SceneManager.GetActiveScene();

        string sceneName = currentScenePvP.name;

        if (sceneName == "PvPLevel1Part1")
        {
            SceneManager.LoadScene("PvPLevel1Part2", LoadSceneMode.Additive);
            SceneManager.LoadScene("PvPLevel1Part3", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("PvPLevel1Part1");
        }
    }
}