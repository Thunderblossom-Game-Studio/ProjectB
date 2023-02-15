using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<GameEvent> gameEvents = new List<GameEvent>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        CleartAllGameEvents();
    }

    public void CleartAllGameEvents()
    {
        gameEvents.ForEach((gameEvent) => gameEvent.Clear());
    }
}
