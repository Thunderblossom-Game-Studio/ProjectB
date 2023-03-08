using FishNet;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugExit : MonoBehaviour
{

    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    public void OnSceneChanged(Scene current, Scene next)
    {
        Debug.Log("Scene changed: " + current.name + "->" + next.name);

        if (next.name == "MultiplayerScene")
        {
            _exitButton = GameObject.Find("Exit Button").GetComponent<Button>();

            _exitButton.onClick.AddListener(() => InstanceFinder.ClientManager.StopConnection());

            _exitButton.onClick.AddListener(() => SceneManager.LoadScene("MultiplayerMenu"));

        }
    }
}
