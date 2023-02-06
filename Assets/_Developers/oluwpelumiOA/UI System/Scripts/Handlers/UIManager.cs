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
        InputManager.Instance.OnBackAction += Instance_OnBackAction;
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

    private void Instance_OnBackAction(object sender, EventArgs e)
    {
        Menu.currentMenu?.OnBackPressed();
    }

    private void Update()
    {
        //if (InputManager.Instance.HandleMoveInput().ReadValue<Vector2>() != Vector2.zero)
        //{
        //    Debug.Log(InputManager.Instance.HandleMoveInput().ReadValue<Vector2>());
        //}

        //if (InputManager.Instance.HandleFireInput().WasPressedThisFrame())
        //{
        //    Debug.Log("Fire");
        //}

        //if (InputManager.Instance.HandleInteractInput().WasPressedThisFrame())
        //{
        //    Debug.Log("Interact");
        //}
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
        switch (scene.buildIndex)
        {
            case 0: 
                MainMenu.Open();
                InputManager.Instance.SwithControlMode(InputManager.ControlMode.UI);
                break;
            case 1: GameMenu.Open(); break;
            default: break;
        }
    }

    public void InstantiateMenu<T>()
    {
        Menu menu = menuPrefabs.Find(x => x.GetType() == typeof(T));
        if (menu != null) SpawnMenu(menu);
    }

    public void InstantiateMenu(Type type)
    {
        Menu menu = menuPrefabs.Find(x => x.GetType() == type);
        if (menu != null) SpawnMenu(menu);
    }

    public void SpawnMenu(Menu menu)
    {
        Menu newMenu = Instantiate(menu, transform);
        newMenu.OpenMenu();
    }
}
