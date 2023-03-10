using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    private event Action<Component, object> response;

    public void Raise(Component sender, object data) => response?.Invoke(sender, data);

    public void Register(Action<Component, object> action) => response += action;

    internal void Register(GameEvent infoEvent)
    {
        throw new NotImplementedException();
    }

    public void Unregister(Action<Component, object> action) => response -= action;

    public void Clear() => response = new Action<Component, object>((sender, data) => { });
}