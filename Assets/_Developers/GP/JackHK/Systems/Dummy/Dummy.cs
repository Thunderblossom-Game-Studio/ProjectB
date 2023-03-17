using JE.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dummy : MonoBehaviour
{
    public bool _isMoving = true;
    public float _flippedTime = 3f;
    public UnityEvent _onFlippedStartEvent;
    public UnityEvent _onFlippedEndEvent;

    FSM _fsm;
    FSM.State _flippedState;
    FSM.State _movingState;
    FSM.State _freezeState;
    RouteUser _routeUser;
    HealthSystem _healthSystem;
    float _currentTime;

    void Start()
    {
        _routeUser = GetComponent<RouteUser>();
        _healthSystem = GetComponent<HealthSystem>();

        _flippedState = FSM_Flipped;
        _movingState = FSM_Moving;
        _freezeState = FSM_Freeze;

        _fsm = new FSM();
        _fsm.Start(_movingState);
        _currentTime = _flippedTime;
    }

    void Update()
    {
        _fsm.OnUpdate();

        if (Input.GetKeyDown(KeyCode.L))
        {
            _healthSystem.ReduceHealth(50);
        }
    }

    public void Heal(float amount)
    {
        _healthSystem.RestoreHealth(amount);
    }

    public void OnDeath()
    {
        _fsm.TransitionTo(_flippedState);
    }

    void FSM_Moving(FSM fsm, FSM.Step step, FSM.State state)
    {
        if (step == FSM.Step.Enter)
        {
            if (!_isMoving) fsm.TransitionTo(_freezeState);
            _routeUser.ToggleMovement(true);
        }
    }

    void FSM_Freeze(FSM fsm, FSM.Step step, FSM.State state)
    {
        if (step == FSM.Step.Enter)
        {
            _routeUser.ToggleMovement(false);
        }
    }

    void FSM_Flipped(FSM fsm, FSM.Step step, FSM.State state)
    {
        if (step == FSM.Step.Enter)
        {
            _onFlippedStartEvent.Invoke();
        }
        else if (step == FSM.Step.Update)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime < 0)
            {
                if (_isMoving) fsm.TransitionTo(_movingState);
                else fsm.TransitionTo(_freezeState);
            }
        }
        else if (step == FSM.Step.Exit)
        {
            _onFlippedEndEvent.Invoke();
            Heal(_healthSystem.MaximumHealth);
            _currentTime = _flippedTime;
        }
    }


}