using JE.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.AI;


public class AIPlayerTutorialController : AICarController
{
    protected enum TutorialState { PURSUE, PATROL, ATTACK, FLEE, PICKUP, DELIVERY }
    [SerializeField] protected TutorialState NextState;


    protected override void Evaluate()
    {

    }

    protected override void SwapState()
    {

    }

    protected override void Act()
    {

    }
}

