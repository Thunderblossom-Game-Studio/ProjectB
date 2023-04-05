using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pelumi.Juicer;

public class GameMenu : BaseMenu<GameMenu>
{
    [SerializeField] private CanvasGroup _gameUI;
    [SerializeField] private CanvasGroup _buttonHolder;

    [Header("Car UI")]
    [SerializeField] private TextMeshProUGUI _carSpeedText;

    [Header( "Package UI")]
    [SerializeField] private JuicerVector3Properties _packageUIProperties;
    [SerializeField] private TextMeshProUGUI _currentPackageText;
    [SerializeField] private TextMeshProUGUI _maxPackageText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Combat UI")]
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _maxAmmoText;
    [SerializeField] private Image _reloadImage;

    [Header("Gameplay UI")] 
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _centreScreenText;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private TextMeshProUGUI _winningTeam;

    [Header("Team UI")] 
    [SerializeField] private TextMeshProUGUI _bluePackages;
    [SerializeField] private TextMeshProUGUI _redPackages;
    [SerializeField] private TextMeshProUGUI _blueScore;
    [SerializeField] private TextMeshProUGUI _redScore;
    
    [Header("Package Events")]
    [SerializeField] private GameEvent _onPickUp;
    [SerializeField] private GameEvent _onDeliver;
    [SerializeField] private GameEvent _onBlueTeamScore;
    [SerializeField] private GameEvent _onRedTeamScore;

    [Header("Combat Events")]
    [SerializeField] private List<JuicerVector3Properties> _reloadingEffect;
    [SerializeField] private GameEvent _onOnAmmoChanged;
    [SerializeField] private GameEvent _onReloadStart;
    [SerializeField] private GameEvent _onReloading;
    [SerializeField] private GameEvent _onReloadEnd;

    [Header("Health")]
    [SerializeField] private CanvasGroup _healthView;

    [Header("Gameplay Events")]
    [SerializeField] private GameEvent _onTimerUpdate;
    [SerializeField] private GameEvent _onCountDown;
    [SerializeField] private GameEvent _onGameOverScreen;

    private Coroutine _reloadRoutine;

    private void OnEnable()
    {
        _onPickUp.Register(OnPickUp);
        _onDeliver.Register(OnDeliver);
        _onBlueTeamScore.Register(OnUpdateBlueTeam);
        _onRedTeamScore.Register(OnUpdateRedTeam);

        _onOnAmmoChanged.Register(OnAmmoChanged);
        _onReloading.Register(OnReload);
        _onReloadStart.Register(OnReloadStart);
        _onReloadEnd.Register(OnReloadEnd);
        
        _onTimerUpdate.Register(OnTimerUpdate);
        _onCountDown.Register(OnCentreTextUpdate);
        _onGameOverScreen.Register(GameOver);
    }
    
    public override IEnumerator OpenMenuRoutine(Action onCompleted = null)
    {
        yield return Juicer.DoFloat(null, 0, (pos) => _buttonHolder.alpha = pos, new JuicerFloatProperties(1, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);
        InputManager.Instance.SwithControlMode(InputManager.ControlMode.Gameplay);     
        InputManager.Instance.OnPauseAction += Instance_OnPauseAction;
        yield return base.OpenMenuRoutine(onCompleted);
    }

    public override IEnumerator CloseMenuRoutine(Action onCompleted = null)
    {
        yield return Juicer.DoFloat(null, _buttonHolder.alpha, (pos) => _buttonHolder.alpha = pos, new JuicerFloatProperties(0, .2f, animationCurveType: AnimationCurveType.EaseInOut), null);
        InputManager.Instance.OnPauseAction -= Instance_OnPauseAction;
        yield return base.CloseMenuRoutine(onCompleted);
    }

    public void MenuButtonPressed()
    {
        CloseMenu(() => (LoadingMenu.Instance as LoadingMenu)?.LoadScene(0));
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
        _currentPackageText.text = ((int[])arg2)[0].ToString();
        _maxPackageText.text = ((int[])arg2)[1].ToString();
        StartCoroutine(Juicer.DoVector3(null, Vector3.zero, (pos) => _currentPackageText.transform.localScale = pos, _packageUIProperties, null));
    }
    
    private void OnDeliver(Component arg1, object arg2)
    {
        _currentPackageText.text = "0";
        _scoreText.text = ((int)arg2) + " Points";
        StartCoroutine(Juicer.DoVector3(null, Vector3.zero, (pos) => _scoreText.transform.localScale = pos, _packageUIProperties, null));
    }

    private void OnAmmoChanged(Component arg1, object value)
    {
        _ammoText.text = ((int[])value)[0].ToString();
        _maxAmmoText.text = ((int[])value)[1].ToString();
        StartCoroutine(Juicer.DoVector3(null, Vector3.zero, (pos) => _ammoText.transform.localScale = pos, _packageUIProperties, null));
    }

    private void OnReload(Component arg1, object value)
    {
        _reloadImage.fillAmount = (float)value;
    }

    private void OnReloadStart(Component arg1, object value)
    {
        _reloadRoutine = StartCoroutine(Juicer.DoMultipleVector3(null, Vector3.zero, (pos) => _reloadImage.transform.localScale = pos, _reloadingEffect, 0, true));
    }

    private void OnReloadEnd(Component arg1, object value)
    {
        StopCoroutine(_reloadRoutine);
    }
    
    private void OnTimerUpdate(Component arg1, object value)
    {
        _timerText.text = (string)value;
    }

    public void SetCarSpeed(string speed)
    {
        _carSpeedText.text = speed;
    }

    private void OnCentreTextUpdate(Component arg1, object value)
    {
        string displayText = ((object[])value)[0].ToString();
        object current = ((object[])value)[1];
        object maximum = ((object[])value)[2];

        _centreScreenText.enabled = (int)current != (int)maximum;
        StartCoroutine(Juicer.DoVector3(null, Vector3.zero, (pos) => _centreScreenText.transform.localScale = pos, _packageUIProperties, null));
        _centreScreenText.text = displayText;
    }

    private void OnUpdateBlueTeam(Component arg1, object value)
    {
        _blueScore.text = ((int[])value)[0].ToString();
        _bluePackages.text = ((int[])value)[1].ToString();
    }

    private void OnUpdateRedTeam(Component arg1, object value)
    {
        _redScore.text = ((int[])value)[0].ToString();
        _redPackages.text = ((int[])value)[1].ToString();
    }

    private void GameOver(Component arg1, object value)
    {
        if ((bool)value)
        {
            _endGamePanel.SetActive(true);
            _gamePanel.SetActive(false);
        }
        else
        {
            _endGamePanel.SetActive(false);
            _gamePanel.SetActive(true);
        }

        _winningTeam.text = GameTeamManager.Instance.GetWinningTeam().TeamName;
    }

    public void SetHealthView(float amount)
    {
        _healthView.alpha =  1 - amount;
    }

    public void ToggleVisibility(bool status )
    {
        _gameUI.alpha = status  ? 1 : 0;
    }

    private void OnDisable()
    {
        _onPickUp.Unregister(OnPickUp);
        _onDeliver.Unregister(OnDeliver);
        _onBlueTeamScore.Unregister(OnUpdateBlueTeam);
        _onRedTeamScore.Unregister(OnUpdateRedTeam);

        _onOnAmmoChanged.Unregister(OnAmmoChanged);
        _onReloading.Unregister(OnReload);
        _onReloadStart.Unregister(OnReloadStart);
        _onReloadEnd.Unregister(OnReloadEnd);
        
        _onTimerUpdate.Unregister(OnTimerUpdate);
        _onCountDown.Unregister(OnCentreTextUpdate);
        
    }
}
