using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Menu Prefabs")]
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private AudioSource menuAudio;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private List<Menu> menuPrefabs;
 
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            eventSystem.SetActive(true);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        Menu.OnAnyUIOpened += Menu_OnAnyUIOpened;
        Menu.OnAnyUIClosed += Menu_OnAnyUIClosed;
        Menu.OnAnyUIDestroyed += Menu_OnAnyUIDestroyed;

        AdvanceButton.OnAnyButtonHovered += AdvanceButton_OnAnyButtonHovered;
        AdvanceButton.OnAnyButtonClicked += AdvanceButton_OnAnyButtonClicked;
    }

    private void Start()
    {
        LoadingMenu.Open();
    }

    private void AdvanceButton_OnAnyButtonHovered(object sender, EventArgs e)
    {
        menuAudio.PlayOneShot(hoverSound);
    }

    private void AdvanceButton_OnAnyButtonClicked(object sender, EventArgs e)
    {
        menuAudio.PlayOneShot(clickSound);
    }

    private void Menu_OnAnyUIOpened(object sender, EventArgs e)
    {

    }

    private void Menu_OnAnyUIClosed(object sender, EventArgs e)
    {

    }

    private void Menu_OnAnyUIDestroyed(object sender, EventArgs e)
    {

    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneType loadedScene = (SceneType)scene.buildIndex;
        switch (loadedScene)
        {
            case SceneType.MainMenu:
                MainMenu.Open();
                InputManager.Instance.SwithControlMode(InputManager.ControlMode.UI);
                break;
            case SceneType.Level1: GameMenu.Open(); break;
            case SceneType.Level2: GameMenu.Open(); break;
            case SceneType.Multiplayer: GameMenu.Open(); break;
            default: break;
        }
    }

    public void InstantiateMenu<T>(Action OnOpen = null)
    {
        Menu menu = menuPrefabs.Find(x => x.GetType() == typeof(T));
        if (menu != null) SpawnMenu(menu, OnOpen);
    }

    public void InstantiateMenu(Type type, Action OnOpen = null)
    {
        Menu menu = menuPrefabs.Find(x => x.GetType() == type);
        if (menu != null) SpawnMenu(menu, OnOpen);
    }

    public void SpawnMenu(Menu menu, Action OnOpen= null)
    {
        Menu newMenu = Instantiate(menu, transform);
        newMenu.OpenMenu(OnOpen);
    }
}
