using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3LevelStreaming : MonoBehaviour
{
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;

        if (sceneName == "LD_Level3Blockout")
        {
            SceneManager.LoadScene("AD_Level3Blockout", LoadSceneMode.Additive);
            SceneManager.LoadScene("AI_Level3Blockout", LoadSceneMode.Additive);
            SceneManager.LoadScene("GP_Level3Blockout", LoadSceneMode.Additive);
            SceneManager.LoadScene("ND_Level3Blockout", LoadSceneMode.Additive);
            SceneManager.LoadScene("UX_Level3Blockout", LoadSceneMode.Additive);
            SceneManager.LoadScene("GD_Level3Blockout", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("LD_Level3Blockout");
        }
    }
}
