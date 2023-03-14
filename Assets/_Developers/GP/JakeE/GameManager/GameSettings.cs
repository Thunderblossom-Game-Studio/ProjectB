using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Game Settings")]
    public float _gameDuration;
    public float _winCondition;
    
    [Header("Sequence Settings")]
    public string[] _beginSequenceText;
    public string[] _completeSequenceText;
}