using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStateManager : MonoBehaviour
{
    private IEnumerator Start() 
    { yield return RunTutorial(); }

    private IEnumerator RunTutorial()
    {
        yield return WelcomeState();
        yield return LearnControlState();
        yield return CollectPackageState();
        yield return DeliverPackageState();
        yield return BattleEnemyState();
        yield return GoodbyeState();
    }

    private IEnumerator WelcomeState()
    {
        Debug.Log("Welcome State");
        bool testValue = false;
        yield return new WaitUntil((() => testValue == false));
    }
    
    private IEnumerator LearnControlState()
    {
        Debug.Log("Learn Control State");
        yield return null;
    }

    private IEnumerator CollectPackageState()
    {
        Debug.Log("Collect Package State");
        yield return null;
    }

    private IEnumerator DeliverPackageState()
    {
        Debug.Log("Deliver Package State");
        yield return null;
    }

    private IEnumerator BattleEnemyState()
    {
        Debug.Log("Battle Enemy State");
        yield return null;
    }

    private IEnumerator GoodbyeState()
    {
        Debug.Log("Goodbye State");
        yield return null;
    }
}
