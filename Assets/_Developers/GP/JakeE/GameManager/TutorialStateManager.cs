using Pelumi.Juicer;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;
using System.Collections.Generic;

public class TutorialStateManager : Singleton<TutorialStateManager>
{
    public enum TutorialAttribute
    {
        CutSceneComplected,
        ReachDrivingDestination,
        CollectPackage,
        DeliverPackage,
        ReachedWeaponArea,
        ReachedHat,
        HasDeliveredVolcanoPackage,
        HasTurnRight, 
        HasTurnLeft,
        HasJumped
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
    [SerializeField] private Transform _destinationArea;
    [SerializeField] private TutorialInstruction _driveControlIntroText;
    [SerializeField] private TutorialInstruction _driveForwardText;
    [SerializeField] private TutorialInstruction _breakText;
    [SerializeField] private TutorialInstruction _turnLeftText;
    [SerializeField] private TutorialInstruction _turnRightText;
    [SerializeField] private TutorialInstruction _reachDestinationText;

    [Header("Package State")]
    [SerializeField] private DeliveryZone _deliveryZone1;
    [SerializeField] private TutorialInstruction _collectPackageText;
    [SerializeField] private TutorialInstruction _deliverPackageText;

    [Header("Shooting State")]
    [SerializeField] private Transform _ShootingArea;
    [SerializeField] private Transform _dummyArea;
    [SerializeField] private TutorialInstruction _shootingIntroText;
    [SerializeField] private TutorialInstruction _shootingInputText;
    [SerializeField] private TutorialInstruction _shootingWeaponSwapText;

    [Header("Jump and Boost State")]
    [SerializeField] private TutorialInstruction _jumpText;
    [SerializeField] private TutorialInstruction _boostText;

    [Header("Hat State")]
    [SerializeField] private Transform _magicHatTrigger;
    [SerializeField] private TutorialInstruction _magicHatText;

    [Header("Volcano State")]
    [SerializeField] private GameObject[] _volcanoStateObjects;
    [SerializeField] private DeliveryZone _deliveryZone;
    [SerializeField] private List<GameObject> _packages;
    [SerializeField] private TutorialInstruction _volcanoIntroText;
    [SerializeField] private TutorialInstruction _volcanoPackageText;

    [Header("Tutorial Walls")]
    [SerializeField] private GameObject _boostStopWall;
    [SerializeField] private GameObject _breakStopWall;
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

    [Viewable] private bool _hasJumped;
    [Viewable] private bool _hasBoost;

    [Viewable] private bool _hasEnteredHat;

    [Viewable] private bool _hasDeliveredVolcanoPackage;

    [Viewable] private float _buttonPressThresshold = .025f;
    [Viewable] private float _currentThreeshold;

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
        yield return JumpAndBoostState();
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

        yield return TriggerInstrution(_breakText);
        while (!_hasBreaked)
        {
            InputConditionCheck(ref _hasBreaked, InputManager.Instance.HandleBrakeInput().IsPressed());
            yield return null;
        }
        StartCoroutine(Juicer.FadeOutMaterial(_breakStopWall.GetComponent<Renderer>().material, .25f, () => _breakStopWall.SetActive(false)));
        yield return DisplaySuccess();

        yield return TriggerInstrution(_turnLeftText);
        while (!_hasTurnLeft)
        {
            yield return null;
        }

        yield return TriggerInstrution(_turnRightText);
        while (!_hasTurnRight)
        {
            yield return null;
        }
    }

    private IEnumerator ReachDestination()
    {
        StartCoroutine(Juicer.FadeOutMaterial(_drivingStopWall.GetComponent<Renderer>().material, .1f, () => _drivingStopWall.SetActive(false)));

        WaypointMarker.Instance.SetTarget(_destinationArea.transform);

        yield return TriggerInstrution(_reachDestinationText);

        yield return new WaitUntil(() => _hasReachedDestination == true);

        WaypointMarker.Instance.SetTarget(null);

        yield return DisplaySuccess();
    }

    private IEnumerator PackageState()
    {
        yield return TriggerInstrution(_collectPackageText);

        yield return new WaitUntil(() => _hasCollectPackage == true);

        yield return DisplaySuccess();

        WaypointMarker.Instance.SetTarget(_deliveryZone1.transform);

        yield return TriggerInstrution(_deliverPackageText);

        yield return new WaitUntil(() => _hasDeliverPackage == true);

        WaypointMarker.Instance.SetTarget(null);

        yield return DisplaySuccess();

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

        yield return new WaitForSeconds(5f);

        yield return DisplaySuccess();

        yield return TriggerInstrution(_shootingWeaponSwapText);

        yield return new WaitUntil(() => Input.GetKey(KeyCode.Tab));

        StartCoroutine(Juicer.FadeOutMaterial(_shootingStopWall.GetComponent<Renderer>().material, .25f, () => _shootingStopWall.SetActive(false)));
    }

    private IEnumerator JumpAndBoostState()
    {
        yield return TriggerInstrution(_jumpText);
        yield return new WaitUntil(() => _hasJumped);

        yield return TriggerInstrution(_boostText);
        while (!_hasBoost)
        {
            InputConditionCheck(ref _hasBoost, InputManager.Instance.HandleBoostInput().IsPressed());
            yield return null;
        }
        StartCoroutine(Juicer.FadeOutMaterial(_boostStopWall.GetComponent<Renderer>().material, .05f, () => _boostStopWall.SetActive(false)));
        yield return DisplaySuccess();
    }

    private IEnumerator MagicHatState()
    {
        yield return TriggerInstrution(_magicHatText);

        WaypointMarker.Instance.SetTarget(_magicHatTrigger);

        yield return new WaitUntil(() => _hasEnteredHat);

        yield return DisplaySuccess();

        WaypointMarker.Instance.SetTarget(null);

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

        WaypointMarker.Instance.SetTarget(_deliveryZone.transform);

        yield return TriggerInstrution(_volcanoPackageText);

        yield return new WaitUntil(() => _packages.All((gameObject)=> gameObject == null));

        yield return DisplaySuccess();

        WaypointMarker.Instance.SetTarget(null);

        infoEvent.Raise(this, new InfoHUDData { Enable = false });

        StartCoroutine(Juicer.FadeOutMaterial(_volcanoStopWall.GetComponent<Renderer>().material, .25f, () => _volcanoStopWall.SetActive(false)));
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
            case TutorialAttribute.HasDeliveredVolcanoPackage:
                _hasDeliveredVolcanoPackage = true;
                return;
            case TutorialAttribute.HasTurnRight:
                _hasTurnRight = true;
                return;
            case TutorialAttribute.HasTurnLeft:
                _hasTurnLeft = true;
                return;
            case TutorialAttribute.HasJumped:
                _hasJumped = true;
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
    public float duration = .25f;
}
