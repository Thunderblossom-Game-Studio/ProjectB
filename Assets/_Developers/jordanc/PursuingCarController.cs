using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuingCarController : AICarController
{
    protected enum State {PURSUE, TURNLEFT, TURNRIGHT, BRAKE}
    [SerializeField] protected State NextState;
    private AITestCar CurrentCar;
    public CollisionPrevention PreventionCollision;

    protected override void Start()
    {
        base.Start();
        CurrentCar = GetComponent<AITestCar>();
    }

    protected override void Evaluate()
    {
      if (PreventionCollision.TurnLeftBoolPass == true)
        {
           NextState = State.TURNLEFT;
        }
      if (PreventionCollision.TurnRightBoolPass == true) 
        {
            NextState= State.TURNRIGHT;
        }
      if (PreventionCollision.BrakeBoolPass == true) 
        {
            NextState= State.BRAKE;
        }

      if ((PreventionCollision.TurnLeftBoolPass == false) && (PreventionCollision.TurnRightBoolPass == false) && (PreventionCollision.BrakeBoolPass == false))
        {
            NextState = State.PURSUE;
        }

    }



    protected override void SwapState()
    {
      
        switch(NextState)
        {
            case State.PURSUE:
                Pursue();
                break;
            case State.TURNLEFT:
                TurnLeft();
                break;
            case State.TURNRIGHT:
                TurnRight();
                break;
            case State.BRAKE:
                Brake(); 
                break;
        }
    }



    protected override void Act()
    {
        if (NextState == State.PURSUE) 
        {  
            FollowAgent();
        }
       

        State c = NextState;

        Evaluate();

        if (Input.GetKeyDown(KeyCode.I) && agentDebug)
        {
            agentDebug.SetActive(!agentDebug.activeInHierarchy);
        }

        if (c != NextState) newState = true;

        SwapState();
    }


    private void Pursue()
    {

    }

    private void TurnLeft()
    {
        CurrentCar.Turn(-1);
    }

    private void TurnRight()
    {
        CurrentCar.Turn(1);
    }

    private void Brake()
    {
        CurrentCar.Accelerate(-1);
    }








}
