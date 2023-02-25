using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings")]
public class GameSettings : ScriptableObject
{
    public float _gameDuration;
    public float _winCondition;
    
    [Header("Begin Sequence Settings")]
    public float _beginSequenceCount;
}