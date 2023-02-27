using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : Singleton<AIDirector>
{
    #region Inspector
    [Header("Bot Difficulty Settings")]
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
    public List<DeliveryPoint> deliveryZones;
    [Tooltip("All bots currently in the scene.")]
    public List<PursuingCarController> bots;
    #endregion

    #region Struct Definition
    private enum Difficulty { EASY, MEDIUM, HARD }

    [Serializable]
    private struct DifficultySetting
    {
        [Header("Health Settings")]
        public float healthThreshold;

        [Header("Package Collection/Delivery Settings")]
        public float packageThreshold;

        [Header("Attack Settings")]
        public float aggroRange;
        public float attackRange;
    }


    [Serializable]
    public struct DeliveryPoint
    {
        public enum Team { RED, BLUE, BOTH }

        [Tooltip("The team that delivers to this point.")]
        public Team team;

        [Tooltip("The transform position of the delivery point.")]
        public Transform position;
    }
    #endregion
}
