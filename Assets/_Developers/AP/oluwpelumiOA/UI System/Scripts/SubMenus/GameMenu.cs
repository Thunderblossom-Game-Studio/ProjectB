using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Pelumi.Juicer;

public class GameMenu : BaseMenu<GameMenu>
{
    [SerializeField] private CanvasGroup buttonHolder;

    [Header( "Package UI")]
    [SerializeField] private JuicerVector3Properties _packageUIProperties;
    [SerializeField] private TextMeshProUGUI currentPackageText;
    [SerializeField] private TextMeshProUGUI maxPackageText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Combat UI")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI maxAmmoText;
    [SerializeField] private Image reloadImage;

    [Header("Package Events")]
    [SerializeField] private GameEvent _onPickUp;
    [SerializeField] private GameEvent _onDeliver;

    [Header("Combat Events")]
    [SerializeField] private List<JuicerVector3Properties> _reloadingEffect;
    [SerializeField] private GameEvent _onOnAmmoChanged;
    [SerializeField] private GameEvent _onReloadStart;
    [SerializeField] private GameEvent _onReloading;
    [SerializeField] private GameEvent _onReloadEnd;

    private Coroutine reloadRoutine;

    private void OnEnable()
    {
        _onPickUp.Register(OnPickUp);
        _onDeliver.Register(OnDeliver);

        _onOnAmmoChanged.Register(OnAmmoChanged);
        _onReloading.Register(OnReload);
        _onReloadStart.Register(OnReloadStart);
        _onReloadEnd.Register(OnReloadEnd);
    }
    
    public override IEnumerator OpenMenuRoutine(Action OnComplected = null)
    {
        yield return Juicer.DoFloat(null, 0, (pos) => buttonHolder.alpha = pos, new JuicerFloatProperties(1, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);
        InputManager.Instance.SwithControlMode(InputManager.ControlMode.Gameplay);     
        InputManager.Instance.OnPauseAction += Instance_OnPauseAction;
        yield return base.OpenMenuRoutine(OnComplected);
    }

    public override IEnumerator CloseMenuRoutine(Action OnComplected = null)
    {
        yield return Juicer.DoFloat(null, buttonHolder.alpha, (pos) => buttonHolder.alpha = pos, new JuicerFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);
        InputManager.Instance.OnPauseAction -= Instance_OnPauseAction;
        yield return base.CloseMenuRoutine(OnComplected);
    }

    public void MenuButtonPressed()
    {
        CloseMenu(() => (LoadingMenu.Instance as LoadingMenu).LoadScene(0));
    }

    public void OptionButtonPressed()
    {
        InputManager.Instance.SwithControlMode(InputManager.ControlMode.UI);
        PauseMenu.Open();
    }

    private void Instance_OnPauseAction(object sender, EventArgs e)
    {
        if (PauseMenu.Instance && PauseMenu.IsOpened) return;
        PauseMenu.Open();
    }
    private void OnPickUp(Component arg1, object arg2)
    {
        currentPackageText.text = (arg2 as int[])[0].ToString();
        maxPackageText.text = (arg2 as int[])[1].ToString();
        StartCoroutine(Juicer.DoVector3(null, Vector3.zero, (pos) => currentPackageText.transform.localScale = pos, _packageUIProperties, null));
    }
    
    private void OnDeliver(Component arg1, object arg2)
    {
        currentPackageText.text = "0";
        scoreText.text = ((int)arg2) + " Points";
        StartCoroutine(Juicer.DoVector3(null, Vector3.zero, (pos) => scoreText.transform.localScale = pos, _packageUIProperties, null));
    }

    private void OnAmmoChanged(Component arg1, object value)
    {
        ammoText.text = (value as int[])[0].ToString();
        maxAmmoText.text = (value as int[])[1].ToString();
        StartCoroutine(Juicer.DoVector3(null, Vector3.zero, (pos) => ammoText.transform.localScale = pos, _packageUIProperties, null));
    }

    private void OnReload(Component arg1, object value)
    {
        reloadImage.fillAmount = (float)value;
    }

    private void OnReloadStart(Component arg1, object value)
    {
        reloadRoutine = StartCoroutine(Juicer.DoMultipleVector3(null, Vector3.zero, (pos) => reloadImage.transform.localScale = pos, _reloadingEffect, 0, true));
    }

    private void OnReloadEnd(Component arg1, object value)
    {
        StopCoroutine(reloadRoutine);
    }

    private void OnDisable()
    {
        _onPickUp.Unregister(OnPickUp);
        _onDeliver.Unregister(OnDeliver);

        _onOnAmmoChanged.Unregister(OnAmmoChanged);
        _onReloading.Unregister(OnReload);
        _onReloadStart.Unregister(OnReloadStart);
        _onReloadEnd.Unregister(OnReloadEnd);
    }
}
