using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLevelStreaming : MonoBehaviour
{
    void Start()
    {

        Scene currentSceneTutorial = SceneManager.GetActiveScene();

        string sceneName = currentSceneTutorial.name;

        if (sceneName == "TutorialLevelPart1")
        {
            SceneManager.LoadScene("TutorialLevelPart2", LoadSceneMode.Additive);
            SceneManager.LoadScene("TutorialLevelAI", LoadSceneMode.Additive);
            SceneManager.LoadScene("TutorialLevelND", LoadSceneMode.Additive);
            SceneManager.LoadScene("TutorialLevelAP", LoadSceneMode.Additive);
            SceneManager.LoadScene("TutorialLevelGP", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("TutorialLevelPart1");
        }
    }
}
