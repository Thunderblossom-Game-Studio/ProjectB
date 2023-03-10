using System;
using System.Collections;
using UnityEngine;

public class TutorialStateManager : Singleton<TutorialStateManager>
{
    public enum TutorialAttribute
    {
        CollectPackage,
        DeliverPackage,
        ReachDestination
    }

    [SerializeField] private GameEvent infoEvent;

    public bool _hasIntroCutSceneComplected;

    public bool _hasDriveFoward;
    public bool _hasTurnLeft;
    public bool _hasTurnRight;
    public bool _hasBreaked;

    public bool _hasReachedDestination;

    public bool _hasCollectPackage;
    public bool _hasDeliverPackage;

    public float buttonPressThresshold;
    public float currentThreeshold;

    private IEnumerator Start() 
    { yield return RunTutorial(); }

    private IEnumerator RunTutorial()
    {
        yield return WelcomeState();
        yield return DrivingControlState();
        yield return PackageState();
        //  yield return BattleEnemyState();
        //  yield return GoodbyeState();
    }

    private IEnumerator IntroCutSceneState()
    {
        yield return new WaitUntil(() => _hasIntroCutSceneComplected);
    }

    private IEnumerator WelcomeState()
    {
        yield return new WaitForSeconds(5f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Welcome State" });

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Welcome State Complete" });
    }

    private IEnumerator DrivingControlState()
    {
        currentThreeshold = 0;

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Learn Control State " });

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Drive Foward start " });

        while (!_hasDriveFoward)
        {
            InputConditionCheck(ref _hasDriveFoward, InputManager.Instance.HandleAccelerateInput().ReadValue<float>() > 0);

            yield return null;
        }

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Drive Foward Complete " });

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Turn Left Start " });

        while (!_hasTurnLeft)
        {
            InputConditionCheck(ref _hasTurnLeft, InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x < 0);
            yield return null;
        }

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Turn Left Complete " });

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Turn Right Start" });

        while (!_hasTurnRight)
        {
            InputConditionCheck(ref _hasTurnRight, InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x > 0);
            yield return null;
        }

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Turn Right Complete" });

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Break Start" });

        while (!_hasBreaked)
        {
            InputConditionCheck(ref _hasBreaked, InputManager.Instance.HandleBrakeInput().IsPressed());
            yield return null;
        }

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Break Complected" });

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = false});
    }

    private IEnumerator PackageState()
    {
        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Pick up the Package" });

        yield return new WaitUntil(() => _hasCollectPackage == true);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Drop off the Package" });

        yield return new WaitUntil(() => _hasDeliverPackage == true);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Successful" });
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
            currentThreeshold += Time.deltaTime;
            if (currentThreeshold >= buttonPressThresshold)
            {
                conditionToReset = true;
                currentThreeshold = 0;
            }
        }
    }
}
