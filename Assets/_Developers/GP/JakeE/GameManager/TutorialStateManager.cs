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

    public bool _hasCollectPackage;
    public bool _hasDeliverPackage;
    public bool _hasReachedDestination;

    private IEnumerator Start() 
    { yield return RunTutorial(); }

    private IEnumerator RunTutorial()
    {
        yield return WelcomeState();
        yield return DrivingState();
        yield return CollectPackageState();
        yield return DeliverPackageState();
        yield return BattleEnemyState();
        yield return GoodbyeState();
    }

    private IEnumerator WelcomeState()
    {
        yield return new WaitForSeconds(5f);
        
        Debug.Log("Welcome State");
        
        yield return HintManager.Instance.HintRoutine
            (HintManager.Instance.WelcomeHint);
        
        Debug.Log("Welcome State Complete");
        
    }
    
    private IEnumerator DrivingState()
    {
        yield return new WaitForSeconds(5f);
        
        Debug.Log("Learn Control State");

        yield return HintManager.Instance.HintRoutine
            (HintManager.Instance.DrivingHint);

        yield return new WaitUntil(() => _hasCollectPackage == true);
        
        Debug.Log("Driving Hint Complete");
    }

    private IEnumerator CollectPackageState()
    {
        yield return new WaitForSeconds(5f);
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
