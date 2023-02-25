using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : Singleton<AIDirector>
{
    #region Inspector
    [Header("Difficulty Settings")]
    [SerializeField] private Difficulty botDifficulty;
    
    [Tooltip("The lowest difficult tier of the multiplayer bot.")]
    [SerializeField] private DifficultySetting tierOne;
    [Tooltip("The mid difficult tier of the multiplayer bot.")]
    [SerializeField] private DifficultySetting tierTwo;
    [Tooltip("The highest difficult tier of the multiplayer bot.")]
    [SerializeField] private DifficultySetting tierThree;

    [Header("Scene Settings")]
    [Tooltip("All package spawners in the scene.")]
    public List<EntitySpawner> packageSpawners;
    [Tooltip("All delivery zones in the scene.")]
    public List<Transform> deliveryZones;
    [Tooltip("All bots currently in the scene.")]
    public List<PursuingCarController> bots;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Struct Definition
    private enum Difficulty { EASY, MEDIUM, HARD }

    [Serializable]
    private struct DifficultySetting
    {
        public float packageThreshold;

        public float healthThreshold;

        public float aggroRange;
        public float attackRange;
    }
    #endregion
}
