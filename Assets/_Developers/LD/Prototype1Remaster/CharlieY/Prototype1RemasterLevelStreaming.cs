using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prototype1RemasterLevelStreaming : MonoBehaviour
{
    void Start()
    {
        Scene currentScenePvE = SceneManager.GetActiveScene();

        string sceneName = currentScenePvE.name;

        if (sceneName == "PrototypeLevel1Remake1")
        {
            SceneManager.LoadScene("PrototypeLevel1Remake2", LoadSceneMode.Additive);
;
        }
        else
        {
            SceneManager.LoadScene("PrototypeLevel1Remake1");
        }
    }
}
