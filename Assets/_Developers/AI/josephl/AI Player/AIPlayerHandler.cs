
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Networking changes:
/// - Added NetworkBehaviour inheritance.
/// - Added [SyncVar] to _state.
/// </summary>

[RequireComponent(typeof(AICarController), typeof(AIDecisionHandler), typeof(AICombatHandler))]
public class AIPlayerHandler : MonoBehaviour
{
    public enum CurrentState { IDLE, PURSUE, FLEE, PICKUP, DELIVERY}

    [Viewable] private CurrentState _state;

    private AICarController _carHandler;
    private AIDecisionHandler _decisionHandler;
    private AICombatHandler _combatHandler;

    private bool _newState = false;
    private CurrentState _previousState;

    private void Awake()
    {
        if (AIDirector.Instance) AIDirector.Instance.bots.Add(this);
        else Debug.LogWarning("No AI Director found in scene.");
    }

    void Start()
    {
        _carHandler = GetComponent<AICarController>();
        _decisionHandler = GetComponent<AIDecisionHandler>();
        _combatHandler = GetComponent<AICombatHandler>();

    }

    private void Update()
    {
        Evaluations();

        StateController();
    }

    private void Evaluations()
    {
        _newState = false;
        _previousState = _state;
        _state = _decisionHandler.Evaluate(_state);
        _state = _combatHandler.Evaluate(_state);
        _state = _carHandler.Evaluate(_state);
        if (_state != _previousState || _carHandler.StalePath()) _newState = true;
    }

    private void StateController()
    {
        switch (_state)
        {
            case CurrentState.IDLE:
                Idle();
                break;
            case CurrentState.PURSUE:
                _combatHandler.Pursue();
                break;
            case CurrentState.FLEE:
                _combatHandler.Flee();
                break;
            case CurrentState.PICKUP:
                _state = _decisionHandler.Pickup(_newState,_state);
                break;
            case CurrentState.DELIVERY:
                _decisionHandler.Delivery();
                break;
        }
    }

    private void Idle()
    {
        _state = CurrentState.PICKUP;
        _carHandler.RecallAgent();
    }

    private void OnDestroy()
    {
        if (AIDirector.Instance)
        {
            AIDirector.Instance.bots.Remove(this);
        }
    }
}
