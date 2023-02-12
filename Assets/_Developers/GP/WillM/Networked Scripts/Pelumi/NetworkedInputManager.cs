using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class NetworkedInputManager : MonoBehaviour
{
    public static NetworkedInputManager Instance;

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public enum DeviceType { KeyboardAndMouse, Gamepad, }

    public enum ControlMode { UI, Gameplay, UIandGameplay}

    [SerializeField] private DeviceType currentDeviceType;

    [SerializeField] private ControlMode controlMode;

    public event EventHandler<DeviceType> OnDeviceChanged;

    public event EventHandler OnTabLeftAction;
    public event EventHandler OnTabRightAction;
    public event EventHandler OnBackAction;

    public event EventHandler OnPauseAction;

    private PlayerInputActions playerInputActions;
    [SerializeField] PlayerInput _playerInput;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public bool EnableInput()
    {
        try
        {
            _playerInput.enabled = true;
            playerInputActions = new PlayerInputActions();        
        }
        catch
        {
            Debug.LogError("Unable to enable playerInput");
            return false;
        }

        try
        {
            UpdateDeviceType(_playerInput.currentControlScheme);

            LoadBinding();

            playerInputActions.Player.Enable();

            playerInputActions.UI.TabLeft.performed += TabLeft_performed;

            playerInputActions.UI.TabRight.performed += TabRight_performed;

            playerInputActions.UI.Back.performed += Back_performed;

            playerInputActions.General.Pause.performed += Pause_performed;

            playerInputActions.Player.Enable();
        }
        catch (Exception e)
        {
            Debug.LogError("Unable to set device type and load bindings: " + e);
            return false;
        }
        
        try
        {
            InputUser.onChange += OnInputDeviceChange;
        }
        catch
        {
            Debug.LogError("Unable to update input device");
            return false;
        }

        return true;

    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Back_performed(InputAction.CallbackContext obj)
    {
        OnBackAction?.Invoke(this, EventArgs.Empty);
    }

    private void TabLeft_performed(InputAction.CallbackContext obj)
    {
        OnTabLeftAction?.Invoke(this, EventArgs.Empty);
    }

    private void TabRight_performed(InputAction.CallbackContext obj)
    {
        OnTabRightAction?.Invoke(this, EventArgs.Empty);
    }
    
    void OnInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if (_playerInput.inputIsActive == false) return;
        if (user.controlScheme.Value.name.Equals(currentDeviceType.ToString())) return;
        UpdateDeviceType(user.controlScheme.Value.name);
    }

    void UpdateDeviceType(string schemeName)
    {
        currentDeviceType = schemeName.Equals("Gamepad") ? DeviceType.Gamepad : DeviceType.KeyboardAndMouse;
        OnDeviceChanged?.Invoke(this, currentDeviceType);
    }

    public InputAction HandleMoveInput() => playerInputActions.Player.Move;

    public InputAction HandleLookInput() => playerInputActions.Player.Look;

    public InputAction HandleFireInput() => playerInputActions.Player.Fire;

    public InputAction HandleInteractInput() => playerInputActions.Player.Interact;

    public InputAction HandleAccelerateInput() => playerInputActions.Player.Accelerate;

    public InputAction HandleDecelerateInput() => playerInputActions.Player.Decelerate;
    
    public InputAction HandleBrakeInput() => playerInputActions.Player.Brake;

    public string GetCurrentDevice() => currentDeviceType == DeviceType.KeyboardAndMouse ? "Keyboard" : "Gamepad";

    public enum Binding
    { 
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Fire, 
        Interact,
        Gamepad_Fire,
        Gamepad_Interact,
    }

    public string GetBindingName(Binding binding)
    {
        switch (binding)
        {
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[5].ToDisplayString();
                
            case Binding.Fire:
                return playerInputActions.Player.Fire.bindings[0].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();

            case Binding.Gamepad_Fire:
                return playerInputActions.Player.Fire.bindings[1].ToDisplayString();
            case Binding.Gamepad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            default:  return "Unknown";
        }
    }

    public void RebindBinding(Binding binding, Action OnActionRebind)
    {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex = 0;

        switch (binding)
        {
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 5;
                break;
            case Binding.Fire:
                inputAction = playerInputActions.Player.Fire;
                bindingIndex = 0;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;

            case Binding.Gamepad_Fire:
                inputAction = playerInputActions.Player.Fire;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            default: return;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            callback.Dispose();
            playerInputActions.Player.Enable();
            OnActionRebind?.Invoke();

            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

        }).Start();
    }

    private void LoadBinding()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
    }

    
    public void SwithControlMode(ControlMode newControlMode)
    {
        switch (newControlMode)
        {
            case ControlMode.UI:
                playerInputActions.Player.Disable();
                playerInputActions.UI.Enable();
                break;
            case ControlMode.Gameplay:
                playerInputActions.Player.Enable();
                playerInputActions.UI.Disable();
                break;
            case ControlMode.UIandGameplay:
                playerInputActions.Player.Enable();
                playerInputActions.UI.Enable();
                break;
            default:
                break;
        }
        
        //switch (newControlMode)
        //{
        //    case ControlMode.UI:
        //        _playerInput.SwitchCurrentActionMap("UI");
        //        break;
        //    case ControlMode.Gameplay:
        //        _playerInput.SwitchCurrentActionMap("Player");
        //        break;
        //}
    }

    public DeviceType GetCurrentDeviceType() => currentDeviceType;

    private void OnDisable()
    {
        InputUser.onChange -= OnInputDeviceChange;
    }
}
