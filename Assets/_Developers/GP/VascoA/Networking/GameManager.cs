using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    
    [SerializeField] private List<GameEvent> _gameEvents = new List<GameEvent>();

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

        ClearAllGameEvents();
    }

    public void ClearAllGameEvents()
    {
        _gameEvents.ForEach((gameEvent) => gameEvent.Clear());
    }
}
