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
        //  yield return CollectPackageState();
        //  yield return DeliverPackageState();
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
            if (InputManager.Instance.HandleMoveInput().IsPressed())
            {
                currentThreeshold += Time.deltaTime;
                if (currentThreeshold >= buttonPressThresshold)
                {
                    _hasDriveFoward = true;
                    currentThreeshold = 0;
                }
            }

            yield return null;
        }

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Drive Foward Complete " });

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Turn Left Start " });

        while (!_hasTurnLeft)
        {
            if (InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x < 0)
            {
                currentThreeshold += Time.deltaTime;
                if (currentThreeshold >= buttonPressThresshold)
                {
                    _hasTurnLeft = true;
                    currentThreeshold = 0;
                }
            }
            yield return null;
        }

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Turn Left Complete " });

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Turn Right Start" });

        while (!_hasTurnRight)
        {
            if (InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x > 0)
            {
                currentThreeshold += Time.deltaTime;
                if (currentThreeshold >= buttonPressThresshold)
                {
                    _hasTurnRight = true;
                    currentThreeshold = 0;
                }
            }
            yield return null;
        }

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Turn Right Complete" });

        yield return new WaitForSeconds(2f);

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Break Start" });

        while (!_hasBreaked)
        {
            if (InputManager.Instance.HandleBrakeInput().IsPressed())
            {
                currentThreeshold += Time.deltaTime;
                if (currentThreeshold >= buttonPressThresshold)
                {
                    _hasBreaked = true;
                    currentThreeshold = 0;
                }
            }
            yield return null;
        }

        infoEvent.Raise(this, new InfoHUDData { Enable = true, Message = "Break Complected" });
    }

    private IEnumerator PackageState()
    {
        yield return new WaitUntil(() => _hasCollectPackage == true);
        Debug.Log("Collect Package State");
    }

    private IEnumerator CollectPackageState()
    {
        yield return new WaitUntil(() => _hasCollectPackage == true);
        Debug.Log("Collect Package State");
    }

    private IEnumerator DeliverPackageState()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Deliver Package State");
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
}
