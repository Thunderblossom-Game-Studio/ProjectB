using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JE.DamageSystem;

public class AICombatHandler : MonoBehaviour
{
    private PackageSystem _packageSystem;
    private AICarController _carHandler;
    private AIDecisionHandler _decisionHandler;
    private GameObject _shootTarget;

    [SerializeField] private Weapon _weaponHandler;
    [SerializeField] private HealthSystem _health;
    private GamePlayer _gamePlayer;
    private Vector3 _spawnPoint;

    [SerializeField] private float _attackRange;
    [Viewable] private float _fleeThreshold;

    [SerializeField] private float _aggroRange;
    [SerializeField] private LayerMask _car;

    private int _closeThreshold = 15;

    // Start is called before the first frame update
    private void Start()
    {
        _carHandler = GetComponent<AICarController>();
        _decisionHandler = GetComponent<AIDecisionHandler>();
        _packageSystem = GetComponent<PackageSystem>();
        _gamePlayer = GetComponent<GamePlayer>();

        if (AIDirector.Instance)
            _fleeThreshold = AIDirector.Instance.tierOne.healthThreshold / 100;
        else
            Debug.LogWarning("No AI Director Instance Detected.");

        _spawnPoint = transform.position;
    }

    private void Update()
    {
        LookForTargets();

        TryShoot();
    }

    public AIPlayerHandler.CurrentState Evaluate(AIPlayerHandler.CurrentState state)
    {
        if (state == AIPlayerHandler.CurrentState.DELIVERY) return state;

        if (_shootTarget)
        {
            // Pursue
            if (Vector3.Distance(transform.position, _shootTarget.transform.position) <= _aggroRange)
            {
                state = AIPlayerHandler.CurrentState.PURSUE;
            }
        }

        if (_health.HealthPercentage <= _fleeThreshold)
        {
            state = AIPlayerHandler.CurrentState.FLEE;
        }

        return state;
    }

    public void Pursue()
    {
        if (_shootTarget)
            _carHandler.SetAgentTarget(_shootTarget.transform.position);
    }

    /// <summary>
    /// This method is the code for the flee state. If the AIBot has packages on them it will go to a delivery zone, if not it will flee to a spawn zone.
    /// </summary>
    public void Flee()
    {
        if (_packageSystem.PackageAmount >= 1)
        {
            _decisionHandler.Delivery();
        }
        else if (_packageSystem.PackageAmount == 0)
        {
            _carHandler.SetAgentTarget(_spawnPoint);
        }
    }

    private void LookForTargets()
    {
        RaycastHit[] Hits = Physics.SphereCastAll(transform.position, _aggroRange, Vector3.forward, 0, _car);

        if (Hits.Length > 0)
        {
            foreach (RaycastHit hit in Hits)
            {
                if (hit.transform.gameObject != gameObject /*&& hit.transform.CompareTag("Player")*/ && hit.transform.TryGetComponent<HealthSystem>(out HealthSystem hs))
                {
                    _shootTarget = hit.transform.gameObject;
                }
            }
        }
    }

    private void TryShoot()
    {
        if (_shootTarget)
        {
            if (_shootTarget.TryGetComponent<GamePlayer>(out GamePlayer gp) && _gamePlayer.PlayerTeamData != null)
            {
                if (gp.PlayerTeamData != null)
                {
                    if (_gamePlayer.PlayerTeamData.TeamName == gp.PlayerTeamData.TeamName)
                    {
                        _shootTarget = null;
                    }
                }
            }
        }

        if (_shootTarget)
        {
            // Attack
            if (Vector3.Distance(transform.position, _shootTarget.transform.position) <= _attackRange)
            {
                if (_shootTarget.TryGetComponent<HealthSystem>(out HealthSystem hs))
                {
                    Shoot();
                }
            }

            if (Vector3.Distance(transform.position, _shootTarget.transform.position) <= _closeThreshold)
            {
                _shootTarget = null;
            }

        }
    }

    private void Shoot()
    {
        if (!_shootTarget) return;
        _weaponHandler.SetAim((_shootTarget.transform.position - transform.position).normalized);
        _weaponHandler.Shoot(_shootTarget.transform.position);

    }
}