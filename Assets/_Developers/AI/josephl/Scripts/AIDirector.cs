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
    [SerializeField] internal DifficultySetting tierOne;
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

    public Transform FindClosestDeliveryZone(Vector3 car)
    {
        if (deliveryZones.Count <= 0) return null;

        Transform NearestPoint = null;

        float Distance;

        float NearestDistance = Vector3.Distance(car, deliveryZones[0].t.position);
        NearestPoint = deliveryZones[0].t;

        for (int i = 0; i < deliveryZones.Count; i++)
        {
            Distance = Vector3.Distance(car, deliveryZones[i].t.position);

            if (Distance < NearestDistance)
            {
                // Bleh
                NearestPoint = deliveryZones[i].t;
                NearestDistance = Distance;
            }

        }

        Debug.Log(NearestPoint);

        return NearestPoint;
    }

    #region Struct Definition
    private enum Difficulty { EASY, MEDIUM, HARD }

    [Serializable]
    public struct DifficultySetting
    {
        [Header("Health Settings")]
        [Range(0,100)] public float healthThreshold;

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
        public Transform t;
    }
    #endregion
}
