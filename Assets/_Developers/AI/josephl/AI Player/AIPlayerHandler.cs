using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AICarController), typeof(AIDecisionHandler), typeof(AICombatHandler))]
public class AIPlayerHandler : MonoBehaviour
{
    public enum CurrentState { IDLE, PURSUE, FLEE, PICKUP, DELIVERY }

    [Viewable] private CurrentState state;

    private AICarController carHandler;
    private AIDecisionHandler decisionHandler;
    private AICombatHandler combatHandler;

    private bool newState = false;
    private CurrentState previousState;

    // Start is called before the first frame update
    void Start()
    {
        carHandler = GetComponent<AICarController>();
        decisionHandler = GetComponent<AIDecisionHandler>();
        combatHandler = GetComponent<AICombatHandler>();

        if (AIDirector.Instance) AIDirector.Instance.bots.Add(this);
        else Debug.LogWarning("No AI Director found in scene.");
    }

    // Update is called once per frame
    private void Update()
    {
        Evaluations();

        StateController();
    }

    private void Evaluations()
    {
        newState = false;
        previousState = state;
        state = decisionHandler.Evaluate(state);
        state = combatHandler.Evaluate(state);
        state = carHandler.Evaluate(state);
        if (state != previousState || carHandler.StalePath()) newState = true;
    }

    private void StateController()
    {
        switch (state)
        {
            case CurrentState.IDLE:
                Idle();
                break;
            case CurrentState.PURSUE:
                combatHandler.Pursue();
                break;
            case CurrentState.FLEE:
                combatHandler.Flee();
                break;
            case CurrentState.PICKUP:
                state = decisionHandler.Pickup(newState,state);
                break;
            case CurrentState.DELIVERY:
                decisionHandler.Delivery();
                break;
        }
    }

    private void Idle()
    {
        carHandler.RecallAgent();
    }

    private void OnDestroy()
    {
        if (AIDirector.Instance)
        {
            AIDirector.Instance.bots.Remove(this);
        }
    }
}
