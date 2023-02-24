using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPLevelStreaming : MonoBehaviour
{
    void Start()
    {

        Scene currentScenePvP = SceneManager.GetActiveScene();

        string sceneName = currentScenePvP.name;

        if (sceneName == "PvPLevel2Part1")
        {
            SceneManager.LoadScene("PvPLevel2Part2", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("PvPLevel2Part1");
        }
    }
}
