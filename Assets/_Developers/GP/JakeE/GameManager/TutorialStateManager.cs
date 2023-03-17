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
        IntroCutScene,
        CollectPackage,
        DeliverPackage,
        ReachDestination
    }

    [SerializeField] private PlayableAsset introCutScene;
    [SerializeField] private CutSceneTrigger cutSceneTrigger;
    [SerializeField] private GameEvent infoEvent;

    [Header("Welcome Intructions")]
    [SerializeField] private TutorialInstruction _welcomeText;

    [Header("Success Notifications")]
    [SerializeField] private TutorialInstruction[] _successTexts;

    [Header("Driving Intructions")]
    [SerializeField] private TutorialInstruction _driveControlIntroText;
    [SerializeField] private TutorialInstruction _driveForwardText;
    [SerializeField] private TutorialInstruction _turnLeftText;
    [SerializeField] private TutorialInstruction _turnRightText;
    [SerializeField] private TutorialInstruction _breakText;
    [SerializeField] private TutorialInstruction _reachDestinationText;

    [Header("Package Intructions")]
    [SerializeField] private TutorialInstruction _collectPackageText;
    [SerializeField] private TutorialInstruction _deliverPackageText;

    [Header("Tutorial Walls")] 
    [SerializeField] private GameObject _firstWall;

    [Viewable] private bool _hasIntroCutSceneComplected;

    [Viewable] private bool _hasDriveFoward;
    [Viewable] private bool _hasTurnLeft;
    [Viewable] private bool _hasTurnRight;
    [Viewable] private bool _hasBreaked;

    [Viewable] private bool _hasReachedDestination;
    [Viewable] private bool _hasCollectPackage;
    [Viewable] private bool _hasDeliverPackage;
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
        //  yield return BattleEnemyState();
        //  yield return GoodbyeState();
    }

    private IEnumerator IntroCutSceneState()
    {
        cutSceneTrigger.StartCutScene(introCutScene);
        yield return new WaitUntil(() => _hasIntroCutSceneComplected);
    }

    private IEnumerator WelcomeState()
    {
        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = _welcomeText.Message });
        AudioManager.PlaySoundEffect(_welcomeText.AudioID);
    }

    private IEnumerator DrivingControlState()
    {
        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = _driveControlIntroText.Message});
        AudioManager.PlaySoundEffect( _driveControlIntroText.AudioID);

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = _driveForwardText.Message });
        AudioManager.PlaySoundEffect(_driveForwardText.AudioID);

        while (!_hasDriveFoward)
        {
            InputConditionCheck(ref _hasDriveFoward, InputManager.Instance.HandleAccelerateInput().ReadValue<float>() > 0);
            yield return null;
        }

        DisplaySuccess();

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = _turnLeftText.Message });

        while (!_hasTurnLeft)
        {
            InputConditionCheck(ref _hasTurnLeft, InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x < 0);
            yield return null;
        }

        DisplaySuccess();

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = _turnRightText.Message });
        AudioManager.PlaySoundEffect(_turnRightText.AudioID);

        while (!_hasTurnRight)
        {
            InputConditionCheck(ref _hasTurnRight, InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x > 0);
            yield return null;
        }

        DisplaySuccess();

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = _breakText.Message });

        while (!_hasBreaked)
        {
            InputConditionCheck(ref _hasBreaked, InputManager.Instance.HandleBrakeInput().IsPressed());
            yield return null;
        }

        DisplaySuccess();

        yield return new WaitForSeconds(2f);
    }

    private IEnumerator ReachDestination()
    {
        StartCoroutine(Juicer.FadeOutMaterial(_firstWall.GetComponent<Renderer>().material, 2f, () => _firstWall.SetActive(false)));

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = _reachDestinationText.Message });
        AudioManager.PlaySoundEffect(_reachDestinationText.AudioID);

        yield return new WaitUntil(() => _hasReachedDestination == true);
        DisplaySuccess();

        yield return _defaultDelay;
    }

    private IEnumerator PackageState()
    {
        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = _collectPackageText.Message });
        AudioManager.PlaySoundEffect(_collectPackageText.AudioID);

        yield return new WaitUntil(() => _hasCollectPackage == true);
        DisplaySuccess();

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = _deliverPackageText.Message});
        AudioManager.PlaySoundEffect(_deliverPackageText.AudioID);

        yield return new WaitUntil(() => _hasDeliverPackage == true);
        DisplaySuccess();

        yield return new WaitForSeconds(2);
        infoEvent.Raise(this, new InfoHUDData { Enable = false});
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
            case TutorialAttribute.IntroCutScene:
                _hasIntroCutSceneComplected = true;
                return;
            case TutorialAttribute.CollectPackage:
                _hasCollectPackage = true;
                return;
            case TutorialAttribute.DeliverPackage:
                _hasDeliverPackage = true;
                return;
            case TutorialAttribute.ReachDestination:
                _hasReachedDestination = true;
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

    public void DisplaySuccess()
    {
        TutorialInstruction tutorialInstruction = _successTexts[UnityEngine.Random.Range(0, _successTexts.Length - 1)];
        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = tutorialInstruction.Message });
        AudioManager.PlaySoundEffect(tutorialInstruction.AudioID);
    }
}

[System.Serializable]
public class TutorialInstruction
{
    public string Message;
    public string AudioID;
}
