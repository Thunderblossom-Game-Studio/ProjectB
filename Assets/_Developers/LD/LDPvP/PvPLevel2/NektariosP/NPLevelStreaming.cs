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
            SceneManager.LoadScene("PvPLevel2ND", LoadSceneMode.Additive);
            SceneManager.LoadScene("PvPLevel2UX", LoadSceneMode.Additive);
            SceneManager.LoadScene("PvPLevel2AI", LoadSceneMode.Additive);
            SceneManager.LoadScene("PvPLevel2GP", LoadSceneMode.Additive);
            SceneManager.LoadScene("PvPLevel2GD", LoadSceneMode.Additive);
            SceneManager.LoadScene("PvPLevel2AD", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("PvPLevel2Part1");
        }
    }
}
