
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public abstract class BaseMenu<T> : Menu
{
    public static BaseMenu<T> Instance { get; private set; }

    public  static bool IsOpened { get; private set; }

    public static T GetInstance() => (T)Convert.ChangeType(Instance, typeof(T));

    [Header("Gamepad Specifics")]
    [SerializeField] protected List<GameObject> gamepadSpecifics;

    [Header("Keyboard Specifics")]
    [SerializeField] protected List<GameObject> keypadSpecifics;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null) Destroy(gameObject); else  Instance = this;
    }

    private void Start()
    {
        
    }

    protected virtual void Instance_OnBackAction(object sender, EventArgs e)
    {

    }

    protected virtual void Instance_OnTabRightAction(object sender, EventArgs e)
    {
        
    }

    protected virtual void Instance_OnTabLeftAction(object sender, EventArgs e)
    {

    }

    public static void Open(Action OnOpened = null)
    {
        if (Instance != null)
        {
            Instance.OpenMenu(OnOpened);
        } 
        else
        {
            UIManager.Instance.InstantiateMenu<T>(OnOpened);
        }
    }

    public override void OnMenuOpened()
    {
        base.OnMenuOpened();
        IsOpened = true;

        InputManager_OnDeviceChanged(null, InputManager.Instance.GetCurrentDeviceType());

        InputManager.Instance.OnDeviceChanged += InputManager_OnDeviceChanged;

        InputManager.Instance.OnBackAction += Instance_OnBackAction;
        InputManager.Instance.OnTabLeftAction += Instance_OnTabLeftAction;
        InputManager.Instance.OnTabRightAction += Instance_OnTabRightAction;
    }

    protected virtual void InputManager_OnDeviceChanged(object sender, InputManager.DeviceType deviceType)
    {
        if (!IsOpened || !navigatable) return;

        switch (deviceType)
        {
            case InputManager.DeviceType.KeyboardAndMouse:  EventSystem.current.SetSelectedGameObject(null);   break;
            case InputManager.DeviceType.Gamepad: firstSelectedButton?.Select();  break;
            default: break;
        }

        gamepadSpecifics.ForEach((x) => x.SetActive(deviceType == InputManager.DeviceType.Gamepad));
        keypadSpecifics.ForEach((x) => x.SetActive(deviceType == InputManager.DeviceType.KeyboardAndMouse));
    }

    public static void Close(Action OnClosed = null)
    {
        if (Instance == null) return;
        Instance.CloseMenu(OnClosed);
    }

    public override void OnMenuClosed()
    {
        InputManager.Instance.OnDeviceChanged -= InputManager_OnDeviceChanged;

        InputManager.Instance.OnBackAction -= Instance_OnBackAction;
        InputManager.Instance.OnTabLeftAction -= Instance_OnTabLeftAction;
        InputManager.Instance.OnTabRightAction -= Instance_OnTabRightAction;

        IsOpened = false;
        base.OnMenuClosed();
    }
}
