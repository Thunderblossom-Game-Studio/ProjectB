using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JE.DamageSystem;

public class AICombatHandler : MonoBehaviour
{
    private PackageSystem packageSystem;
    private AICarController carHandler;
    private AIDecisionHandler decisionHandler;
    private GameObject ShootTarget;

    [SerializeField] private Weapon weaponHandler;
    [SerializeField] private HealthSystem Health;
    private GamePlayer gamePlayer;
    private Vector3 SpawnPoint;

    [SerializeField] private float AttackRange;
    [Viewable] private float fleeThreshold;

    [SerializeField] float AggroRange;
    [SerializeField] LayerMask Car;

    // Start is called before the first frame update
    private void Start()
    {
        carHandler = GetComponent<AICarController>();
        decisionHandler = GetComponent<AIDecisionHandler>();
        packageSystem = GetComponent<PackageSystem>();
        gamePlayer = GetComponent<GamePlayer>();

        if (AIDirector.Instance)
            fleeThreshold = AIDirector.Instance.tierOne.healthThreshold / 100;
        else
            Debug.LogWarning("No AI Director Instance Detected.");

        SpawnPoint = transform.position;
    }

    private void Update()
    {
        LookForTargets();

        TryShoot();
    }

    public AIPlayerHandler.CurrentState Evaluate(AIPlayerHandler.CurrentState state)
    {
        if (state == AIPlayerHandler.CurrentState.DELIVERY) return state;

        if (ShootTarget)
        {
            // Pursue
            if (Vector3.Distance(transform.position, ShootTarget.transform.position) <= AggroRange)
            {
                state = AIPlayerHandler.CurrentState.PURSUE;
            }
        }

        if (Health.HealthPercentage <= fleeThreshold)
        {
            state = AIPlayerHandler.CurrentState.FLEE;
        }

        return state;
    }

    public void Pursue()
    {
        if (ShootTarget)
            carHandler.SetAgentTarget(ShootTarget.transform.position);
    }

    /// <summary>
    /// This method is the code for the flee state. If the AIBot has packages on them it will go to a delivery zone, if not it will flee to a spawn zone.
    /// </summary>
    public void Flee()
    {
        if (packageSystem.PackageAmount >= 1)
        {
            decisionHandler.Delivery();
        }
        else if (packageSystem.PackageAmount == 0)
        {
            carHandler.SetAgentTarget(SpawnPoint);
        }
    }

    private void LookForTargets()
    {
        RaycastHit[] Hits = Physics.SphereCastAll(transform.position, AggroRange, Vector3.forward, 0, Car);

        if (Hits.Length > 0)
        {
            foreach (RaycastHit hit in Hits)
            {
                if (hit.transform.gameObject != gameObject && hit.transform.CompareTag("Player"))
                {

                    ShootTarget = hit.transform.gameObject;

                }
            }
        }
    }

    private void TryShoot()
    {
        if (ShootTarget)
        {
            if (ShootTarget.TryGetComponent<GamePlayer>(out GamePlayer gp) && gamePlayer.PlayerTeamData != null)
            {
                if (gamePlayer.PlayerTeamData.TeamName == gp.PlayerTeamData.TeamName)
                {
                    ShootTarget = null;
                }
            }
        }

        if (ShootTarget)
        {
            // Attack
            if (Vector3.Distance(transform.position, ShootTarget.transform.position) <= AttackRange)
            {
                if (ShootTarget.TryGetComponent<HealthSystem>(out HealthSystem hs))
                {
                    Shoot();
                }
            }

            if (Vector3.Distance(transform.position, ShootTarget.transform.position) <= 15)
            {
                ShootTarget = null;
            }

        }
    }

    private void Shoot()
    {
        if (!ShootTarget) return;
        weaponHandler.SetAim((ShootTarget.transform.position - transform.position).normalized);
        weaponHandler.Shoot(ShootTarget.transform.position);

    }
}