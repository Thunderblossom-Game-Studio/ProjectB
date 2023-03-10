using JE.DamageSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayerTutorialController : PursuingCarController
{
    public enum TutorialState { FOLLOWROUTE, WAIT }


    [Space(25)]
    [Header("Tutorial Settings")]
    public TutorialState tutorialState;
    public Dictionary<TutorialState, bool> TutorialStateAccess;
    [SerializeField] private bool canShoot = false;

    [SerializeField] private TutorialAIDirector.Route currentRoute;

    protected override void Start()
    {
        if (TutorialAIDirector.Instance)
        {
            TutorialAIDirector.Instance.bots.Add(this);
            gameObject.SetActive(false);
        }
        else Debug.LogWarning("No Tutorial AI Director found in scene.");

        base.Start();
    }

    private void OnDestroy()
    {
        if (TutorialAIDirector.Instance)
        {
            TutorialAIDirector.Instance.bots.Remove(this);
        }

    }

    protected override void Evaluate()
    {
        base.Evaluate();

        tutorialState = TutorialState.FOLLOWROUTE;
    }

    protected override void SwapState()
    {

        switch (tutorialState)
        {
            case TutorialState.FOLLOWROUTE:
                FollowRoute();
                break;
            case TutorialState.WAIT:
                break;

        }
    }

    protected override void Shoot()
    {
        if (canShoot) base.Shoot();
    }

    public void SetNextRoute(TutorialAIDirector.Route next)
    {
        if (currentRoute.points.Count > 0) transform.position = currentRoute.points[currentRoute.points.Count - 1].position;

        currentRoute = next;

        if (currentRoute.points.Count > 0) transform.LookAt(currentRoute.points[0].position);
    }

    public void SetStateAccess(TutorialState state, bool access)
    {
        if (TutorialStateAccess.ContainsKey(state))
        {
            TutorialStateAccess[state] = access;
        }
    }

    private void FollowRoute()
    {
        if (currentRoute.IsFinished() || currentRoute.points.Count == 0)
        {
            Wait();
            return;
        }

        agent.SetDestination(currentRoute.points[currentRoute.pointIndex].position);

        if (Vector3.Distance(transform.position, currentRoute.points[currentRoute.pointIndex].position) <= stopDistance)
        {
            currentRoute.pointIndex++;
        }

    }

    private void Wait()
    {

    }

    [Serializable]
    public struct TogglableState
    {
        public TutorialState state;
        public bool canUse;
    }
}

