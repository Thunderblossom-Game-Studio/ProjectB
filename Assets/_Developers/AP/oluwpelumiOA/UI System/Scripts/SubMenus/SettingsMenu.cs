using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class SettingsMenu : BaseMenu <SettingsMenu>, IDataPersistance
{
    //Test variable to save to settings
    public int testInt = 0;

    [SerializeField] private Transform title;
    [SerializeField] private RectTransform view2;
    [SerializeField] private CanvasGroup buttonHolder;
    [SerializeField] private CanvasGroup view1;
    [SerializeField] private TabUI tabUI;

    [Header("Effect")]
    [SerializeField] private FeelVector3Properties textPopTransition;

    [Header("Control Settings")]

    [Header("Key Binding")]
    [SerializeField] private Transform pressToRebindKey;
    [SerializeField] private BindUI moveUpBind;
    [SerializeField] private BindUI moveDownBind;
    [SerializeField] private BindUI moveLeftBind;
    [SerializeField] private BindUI moveRightBind;
    [SerializeField] private BindUI fireBind;
    [SerializeField] private BindUI interactBind;

    [Header("Gamepad")]
    [SerializeField] private BindUI gamepadFireText;
    [SerializeField] private BindUI gamepadInteractText;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Video Settings")]
    [SerializeField] private HorizontalSelector screenResolutionSelector;
    [SerializeField] private Toggle fullScreenToggle;

    private Resolution[] resolutions;

    private bool inBindingMode;

    public void Start()
    {
        TogglePressToRebind(false);
        SetBindingButtons();
        UpdateBindingVisuals();
        
        PopulateScreenResolution();

        fullScreenToggle.onValueChanged.AddListener((x) => Screen.fullScreen = x);
    }

    


    protected override void InputManager_OnDeviceChanged(object sender, InputManager.DeviceType deviceType)
    {
        if (!IsOpened) return;    
        if (deviceType == InputManager.DeviceType.Gamepad) firstSelectedButton = tabUI.GetCurrentTabTogleSelectable();
        base.InputManager_OnDeviceChanged(sender, deviceType);
    }

    protected override void Instance_OnTabLeftAction(object sender, EventArgs e)
    {
        if (inBindingMode) return;
        tabUI.SelectLeft();
    }

    protected override void Instance_OnTabRightAction(object sender, EventArgs e)
    {
        if (inBindingMode) return;
        tabUI.SelectRight();
    }
    
    public override IEnumerator OpenMenuRoutine(Action OnComplected = null)
    {
        StartCoroutine(FeelUtility.FadeVector3(null, Vector3.zero, (pos) => title.transform.localScale = pos, 
            new FeelVector3Properties(new Vector3(1, 1, 1), .1f, animationCurveType: AnimationCurveType.EaseInOut), null));
        
        yield return FeelUtility.FadeFloat(null, -1177, (v) => view2.anchoredPosition =  new Vector2(v, view2.anchoredPosition.y), new FeelFloatProperties(0, .25f, animationCurveType: AnimationCurveType.EaseInOut));
        yield return FeelUtility.FadeFloat(null, buttonHolder.alpha,(v) => buttonHolder.alpha = v, new FeelFloatProperties(1, .5f, animationCurveType: AnimationCurveType.EaseInOut));

        yield return base.OpenMenuRoutine(OnComplected);
    }

    public override IEnumerator CloseMenuRoutine(Action OnComplected = null)
    {
        yield return FeelUtility.FadeFloat(null, buttonHolder.alpha, (v) => buttonHolder.alpha = v, new FeelFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut));
        yield return FeelUtility.FadeFloat(null, 0, (v) => view2.anchoredPosition = new Vector2(v, view2.anchoredPosition.y), new FeelFloatProperties(-1177, .25f, animationCurveType: AnimationCurveType.EaseInOut));
    
        StartCoroutine(FeelUtility.FadeVector3(null, title.transform.localScale, (pos) => title.transform.localScale = pos,
    new FeelVector3Properties(Vector3.zero, .1f, animationCurveType: AnimationCurveType.EaseInOut), null));

        yield return base.CloseMenuRoutine(OnComplected);
    }
    
    protected override void ResetUI()
    {
        title.localScale = Vector3.zero;
        buttonHolder.alpha = 0;
    }

    protected override void Instance_OnBackAction(object sender, EventArgs e)
    {
        if (inBindingMode) return;
        CloseButton();
    }

    public void CloseButton()
    {
        Close(() => lastMenu.OpenMenu());
    }

    private void PopulateScreenResolution()
    {
        resolutions = Screen.resolutions;

        screenResolutionSelector.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        screenResolutionSelector.SetOption(options);
        screenResolutionSelector.SetIndex(currentResolutionIndex);
        screenResolutionSelector.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        StartCoroutine(FeelUtility.FadeVector3(null, Vector3.zero, (pos) => screenResolutionSelector.GetText().transform.localScale = pos,  textPopTransition, null));
        
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("Set Resolution: " + resolution.width + "x" + resolution.height);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetVSync(bool isVSync)
    {
        QualitySettings.vSyncCount = isVSync ? 1 : 0;
    }

    public void SetAntiAliasing(int antiAliasing)
    {
        QualitySettings.antiAliasing = antiAliasing;
    }

    public void SetTextureQuality(int textureQuality)
    {
        QualitySettings.masterTextureLimit = textureQuality;
    }

    public void SetShadows(bool isShadows)
    {
        QualitySettings.shadows = isShadows ? ShadowQuality.All : ShadowQuality.Disable;
    }

    public void SetSoftParticles(bool isSoftParticles)
    {
        QualitySettings.softParticles = isSoftParticles;
    }

    public void SetRealtimeReflections(bool isRealtimeReflections)
    {
        QualitySettings.realtimeReflectionProbes = isRealtimeReflections;
    }

    public void SetBrightness(float brightness)
    {
        Screen.brightness = brightness;
    }
    
    public void UpdateBindingVisuals()
    {
        moveUpBind.SetBindText (InputManager.Instance.GetBindingName(InputManager.Binding.Move_Up));
        moveDownBind.SetBindText(InputManager.Instance.GetBindingName(InputManager.Binding.Move_Down));
        moveLeftBind.SetBindText(InputManager.Instance.GetBindingName(InputManager.Binding.Move_Left));
        moveRightBind.SetBindText(InputManager.Instance.GetBindingName(InputManager.Binding.Move_Right));
        
        fireBind.SetBindText(InputManager.Instance.GetBindingName(InputManager.Binding.Fire));
        interactBind.SetBindText(InputManager.Instance.GetBindingName(InputManager.Binding.Interact));

        gamepadFireText.SetBindText(InputManager.Instance.GetBindingName(InputManager.Binding.Gamepad_Fire));
        gamepadInteractText.SetBindText(InputManager.Instance.GetBindingName(InputManager.Binding.Gamepad_Interact));
    }

    public void SetBindingButtons()
    {
        moveUpBind.SetBindButton(() => RebingBinding(InputManager.Binding.Move_Up));
        moveDownBind.SetBindButton(() => RebingBinding(InputManager.Binding.Move_Down));
        moveLeftBind.SetBindButton(() => RebingBinding(InputManager.Binding.Move_Left));
        moveRightBind.SetBindButton(() => RebingBinding(InputManager.Binding.Move_Right));

        fireBind.SetBindButton(() => RebingBinding(InputManager.Binding.Fire));
        interactBind.SetBindButton(() => RebingBinding(InputManager.Binding.Interact));

        gamepadFireText.SetBindButton(() => RebingBinding(InputManager.Binding.Gamepad_Fire));
        gamepadInteractText.SetBindButton(() => RebingBinding(InputManager.Binding.Gamepad_Interact));
    }

    private void TogglePressToRebind(bool isOn)
    {
        pressToRebindKey.gameObject.SetActive(isOn);
    }

    private void RebingBinding(InputManager.Binding binding)
    {
        inBindingMode = true;
        TogglePressToRebind(true);
        InputManager.Instance.RebindBinding(binding, ()=> 
        {
            TogglePressToRebind(false);
            UpdateBindingVisuals();
            if (InputManager.Instance.GetCurrentDeviceType() == InputManager.DeviceType.KeyboardAndMouse) EventSystem.current.SetSelectedGameObject(null);
            StartCoroutine(EnableControl());
        });
    }

    IEnumerator EnableControl()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        inBindingMode = false;
    }

    public void SetMasterVolume(float value)
    {
        //SetVolume("Master", value, masterDisplay);
    }

    public void SetMusicVolume(float value)
    {
       // SetVolume("Music", value, musicDisplay);
    }

    public void SetSfxVolume(float value)
    {
      //  SetVolume("Sfx", value, sfxDisplay);
    }

    void SetVolume(string parameter, float value, Text display)
    {
        audioMixer.SetFloat(parameter, Mathf.Log10(value) * 80);

        display.text = (value * 100).ToString("0") + "%";
    }

    void SaveVolume(string parameter, float value)
    {
        PlayerPrefs.SetFloat(parameter, value);
    }
    
    void LoadVolume(string parameter, Slider slider, Text display)
    {
        if (PlayerPrefs.HasKey(parameter))
        {
            float volume = PlayerPrefs.GetFloat(parameter);
            slider.value = volume;
            SetVolume(parameter, volume, display);
        }
    }

    public void ApplyChanges()
    {
        CloseButton();
    }

    public void Save()
    {
        
    }

    public void LoadSettingsData(SettingsData data)
    {
        data.testInt = this.testInt;
        //Add values you want to save here
    }

    public void SaveSettingsData(ref SettingsData data)
    {
        //Add values you want to save here
        this.testInt = data.testInt;
    }


    #region GameData 
    public void LoadGameData(GameData data)
    {
        throw new NotImplementedException();
    }

    public void SaveGameData(ref GameData data)
    {
        throw new NotImplementedException();
    }
    #endregion //NOT IMPLEMENTED
}
