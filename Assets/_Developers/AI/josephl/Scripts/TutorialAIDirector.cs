using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAIDirector : Singleton<TutorialAIDirector>
{
    [SerializeField] private List<Route> routes;
    [Viewable] private int currentRoute = 0;

    public List<AIPlayerTutorialController> bots;

    public bool t = false;

    private void Update()
    {
        if (t)
        {
            StartNextRoute();

            t = false;
        }
    }

    public void StartNextRoute()
    {
        if (currentRoute >= routes.Count) return;

        if (currentRoute == 0) bots[0].gameObject.SetActive(true);

        bots[0].SetNextRoute(routes[currentRoute]);

        Debug.Log("saasdasd");

        currentRoute++;
    }

    [Serializable]
    public struct Route
    {
        public List<Transform> points;
        [Viewable] public int pointIndex;

        public bool IsFinished()
        {
            if (pointIndex >= points.Count) return true;
            return false;
        }
    }
}
