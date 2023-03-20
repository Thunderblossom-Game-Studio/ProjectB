using Pelumi.Juicer;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialStateManager : Singleton<TutorialStateManager>
{
    public enum TutorialAttribute
    {
        CutSceneComplected,
        ReachDrivingDestination,
        CollectPackage,
        DeliverPackage,
        ReachedWeaponArea,
        ReachedHat
    }

    [SerializeField] private GameEvent infoEvent;

    [Header("CutSceneTriggers")]
    [SerializeField] private CutSceneTrigger cutSceneTrigger;
    [SerializeField] private CutSceneTrigger area2CutSceneTrigger;

    [Header("Welcome Intructions")]
    [SerializeField] private TutorialInstruction _welcomeText;

    [Header("Success Notifications")]
    [SerializeField] private TutorialInstruction[] _successTexts;

    [Header("Driving State")]
    [SerializeField] private TutorialInstruction _driveControlIntroText;
    [SerializeField] private TutorialInstruction _driveForwardText;
    [SerializeField] private TutorialInstruction _turnLeftText;
    [SerializeField] private TutorialInstruction _turnRightText;
    [SerializeField] private TutorialInstruction _breakText;
    [SerializeField] private TutorialInstruction _reachDestinationText;

    [Header("Package State")]
    [SerializeField] private TutorialInstruction _collectPackageText;
    [SerializeField] private TutorialInstruction _deliverPackageText;

    [Header("Shooting State")]
    [SerializeField] private Transform _ShootingArea;
    [SerializeField] private Transform _dummyArea;
    [SerializeField] private TutorialInstruction _shootingIntroText;
    [SerializeField] private TutorialInstruction _shootingInputText;
    [SerializeField] private TutorialInstruction _shootingWeaponSwapText;

    [Header("Hat State")]
    [SerializeField] private Transform _magicHatTrigger;
    [SerializeField] private TutorialInstruction _magicHatText;

    [Header("Volcano State")]
    [SerializeField] private GameObject[] _volcanoStateObjects;
    [SerializeField] private TutorialInstruction _volcanoIntroText;
    [SerializeField] private TutorialInstruction _volcanoPackageText;

    [Header("Tutorial Walls")] 
    [SerializeField] private GameObject _drivingStopWall;
    [SerializeField] private GameObject _shootingStopWall;
    [SerializeField] private GameObject _volcanoStopWall;

    [Viewable] private bool _hasCutSceneComplected;

    [Viewable] private bool _hasDriveFoward;
    [Viewable] private bool _hasTurnLeft;
    [Viewable] private bool _hasTurnRight;
    [Viewable] private bool _hasBreaked;

    [Viewable] private bool _hasReachedDestination;

    [Viewable] private bool _hasCollectPackage;
    [Viewable] private bool _hasDeliverPackage;

    [Viewable] private bool _hasReachedWeaponArea;
    [Viewable] private bool _hasShootTargets;

    [Viewable] private bool _hasEnteredHat;

    [Viewable] private bool _hasCollectAllVolcanoPackages;

    [Viewable] private float _buttonPressThresshold = 1;
    [Viewable] private float _currentThreeshold;

    private readonly WaitForSeconds _defaultDelay = new WaitForSeconds(2);

    private IEnumerator Start() 
    {
        Time.timeScale = 1;
        yield return RunTutorial();
    }

    private IEnumerator RunTutorial()
    {
        yield return IntroCutSceneState();
        yield return WelcomeState();
        yield return DrivingControlState();
        yield return ReachDestination();
        yield return PackageState();
        yield return ShootingState();
        yield return MagicHatState();

        yield return Area2CutSceneState();
        yield return VolcanoState();
        //  yield return BattleEnemyState();
        //  yield return GoodbyeState();
    }

    private IEnumerator IntroCutSceneState()
    {
        cutSceneTrigger.StartCutScene();
        yield return new WaitUntil(() => _hasCutSceneComplected);
        _hasCutSceneComplected = false;
    }

    private IEnumerator WelcomeState()
    {
        yield return new WaitForSeconds(2f);
        yield return TriggerInstrution(_welcomeText);
    }

    private IEnumerator DrivingControlState()
    {
        yield return TriggerInstrution(_driveControlIntroText);

        yield return TriggerInstrution(_driveForwardText);

        while (!_hasDriveFoward)
        {
            InputConditionCheck(ref _hasDriveFoward, InputManager.Instance.HandleAccelerateInput().ReadValue<float>() > 0);
            yield return null;
        }

        yield return DisplaySuccess();

        yield return TriggerInstrution(_turnLeftText);

        while (!_hasTurnLeft)
        {
            InputConditionCheck(ref _hasTurnLeft, InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x < 0);
            yield return null;
        }

        yield return DisplaySuccess();

        yield return TriggerInstrution(_turnRightText);

        while (!_hasTurnRight)
        {
            InputConditionCheck(ref _hasTurnRight, InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x > 0);
            yield return null;
        }

        yield return DisplaySuccess();

        yield return TriggerInstrution(_breakText);

        while (!_hasBreaked)
        {
            InputConditionCheck(ref _hasBreaked, InputManager.Instance.HandleBrakeInput().IsPressed());
            yield return null;
        }

        yield return DisplaySuccess();
    }

    private IEnumerator ReachDestination()
    {
        StartCoroutine(Juicer.FadeOutMaterial(_drivingStopWall.GetComponent<Renderer>().material, 1f, () => _drivingStopWall.SetActive(false)));

        yield return TriggerInstrution(_reachDestinationText);

        yield return new WaitUntil(() => _hasReachedDestination == true);

        yield return DisplaySuccess();
    }

    private IEnumerator PackageState()
    {
        yield return TriggerInstrution(_collectPackageText);

        yield return new WaitUntil(() => _hasCollectPackage == true);
        yield return DisplaySuccess();

        yield return TriggerInstrution(_deliverPackageText);

        yield return new WaitUntil(() => _hasDeliverPackage == true);
        yield return DisplaySuccess();

        yield return new WaitForSeconds(2);
        infoEvent.Raise(this, new InfoHUDData { Enable = false});
    }

    private IEnumerator ShootingState()
    {
        WaypointMarker.Instance.SetTarget(_ShootingArea);

        yield return TriggerInstrution(_shootingIntroText);

        yield return new WaitUntil(() => _hasReachedWeaponArea);
        WaypointMarker.Instance.SetTarget(_dummyArea);

        yield return TriggerInstrution(_shootingInputText);

        yield return new WaitUntil(() => InputManager.Instance.HandleFireInput().IsPressed());
        yield return DisplaySuccess();

        yield return new WaitForSeconds(2f);

        yield return TriggerInstrution(_shootingWeaponSwapText);

        yield return new WaitUntil(() => Input.GetKey(KeyCode.Tab));

        StartCoroutine(Juicer.FadeOutMaterial(_shootingStopWall.GetComponent<Renderer>().material, 1f, () => _shootingStopWall.SetActive(false)));
    }

    private IEnumerator MagicHatState()
    {
        yield return TriggerInstrution(_magicHatText);
        WaypointMarker.Instance.SetTarget(_magicHatTrigger);
        yield return new WaitUntil(() => _hasEnteredHat);
        yield return DisplaySuccess();
        WaypointMarker.Instance.SetTarget(null);
        yield return new WaitForSeconds(2);
        infoEvent.Raise(this, new InfoHUDData { Enable = false });
    }

    private IEnumerator Area2CutSceneState()
    {
        area2CutSceneTrigger.StartCutScene();
        yield return new WaitUntil(() => _hasCutSceneComplected);
    }

    private IEnumerator VolcanoState()
    {
        yield return TriggerInstrution(_volcanoIntroText);

        foreach (var gameObject in _volcanoStateObjects) gameObject.SetActive(true);

        yield return TriggerInstrution(_volcanoPackageText);

        yield return new WaitUntil(() => _hasCollectAllVolcanoPackages);
        yield return DisplaySuccess();
        WaypointMarker.Instance.SetTarget(null);
        yield return new WaitForSeconds(2);
        infoEvent.Raise(this, new InfoHUDData { Enable = false });

        StartCoroutine(Juicer.FadeOutMaterial(_volcanoStopWall.GetComponent<Renderer>().material, 1f, () => _volcanoStopWall.SetActive(false)));
    }

    private IEnumerator BattleEnemyState()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Battle Enemy State");
    }

    private IEnumerator GoodbyeState()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Goodbye State");
    }

    public void CompleteAttribute(TutorialAttribute tutorialAttribute)
    {
        switch (tutorialAttribute)
        {
            case TutorialAttribute.CutSceneComplected:
                _hasCutSceneComplected = true;
                return;
            case TutorialAttribute.CollectPackage:
                _hasCollectPackage = true;
                return;
            case TutorialAttribute.DeliverPackage:
                _hasDeliverPackage = true;
                return;
            case TutorialAttribute.ReachDrivingDestination:
                _hasReachedDestination = true;
                return;
            case TutorialAttribute.ReachedWeaponArea:
                _hasReachedWeaponArea = true;
                return;
            case TutorialAttribute.ReachedHat:
                _hasEnteredHat = true;
                return;
        }
    }

    public void InputConditionCheck(ref bool conditionToReset, bool conditionToCheck)
    {
        if (conditionToCheck)
        {
            _currentThreeshold += Time.deltaTime;
            if (_currentThreeshold >= _buttonPressThresshold)
            {
                conditionToReset = true;
                _currentThreeshold = 0;
            }
        }
    }

    public IEnumerator DisplaySuccess()
    {
        TutorialInstruction tutorialInstruction = _successTexts[UnityEngine.Random.Range(0, _successTexts.Length - 1)];
        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = tutorialInstruction.Message });
        AudioManager.PlaySoundEffect(tutorialInstruction.AudioID);
        yield return new WaitForSeconds(tutorialInstruction.duration);
    }

    public IEnumerator TriggerInstrution(TutorialInstruction tutorialInstruction)
    {
        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = tutorialInstruction.Message });
        AudioManager.PlaySoundEffect(tutorialInstruction.AudioID);
        yield return new WaitForSeconds(tutorialInstruction.duration);
    }
}

[System.Serializable]
public class TutorialInstruction
{
    public string Message;
    public string AudioID;
    public float duration = 2;
}
