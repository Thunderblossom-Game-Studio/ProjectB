using FishNet;
using FishNet.Object;
using Pelumi.Juicer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Weapon
{
    public enum State { Reload, Fire }

    [Space(5)]

    [Header("Ref")]
    [SerializeField] private Transform thrower;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Properties")]
    [SerializeField] private List<JuicerVector3Properties> throwProperties;
    [SerializeField] private float throwerResetDelay;
    [SerializeField] private float range;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    [SerializeField] private int vertextCount;

    [Header("Test")]
    [Viewable] [SerializeField] private float angle;
    [Viewable] [SerializeField] private State state;
    [Viewable] [SerializeField] private LayerMask layerMask;
    [Viewable] [SerializeField] public CatapultProjectile loadedProjectile;
    [Viewable] [SerializeField] private bool inAction;
    [Viewable] [SerializeField] private Vector3 targetPos;

    protected override void Start()
    {
        base.Start();
        SwitchState(State.Reload);
    }

    public override void ShootProjectile(Vector3 targetPos, Action onFireSuccess = null )
    {
        if (inAction) return;
        base.ShootProjectile(targetPos);
        this.targetPos = targetPos;
        inAction = true;
        StartCoroutine(Juicer.DoMultipleVector3(null, Vector3.zero, (rotation) => thrower.localEulerAngles = rotation, throwProperties, throwerResetDelay, false, HandleState, null));
    }

    public void DrawPath(Vector3 _targetPos)
    {
        targetPos = _targetPos;
        angle = PathUtil.CalculateAngle(firePoint[0].position, targetPos, minAngle, maxAngle);
        PathUtil.DrawPath(lineRenderer, firePoint[0].position, targetPos, angle, vertextCount);
    }

    public void HandleState()
    {
        switch (state)
        {
            case State.Reload: SwitchState(State.Fire); break;
            case State.Fire: SwitchState(State.Reload); break;
        }
    }

    public void SwitchState(State _state)
    {
        state = _state;
        switch (state)
        {
            case State.Reload:
                SpawnProjectile(firePoint[0].position);
                inAction = false;
                break;
            case State.Fire:
                ModifyAmmo(currentAmmo - 1);
                FireProjectile(targetPos, weaponSO.projectileSpeed, angle);
                AudioManager.PlaySoundEffect(FireSoundID, true);
                break;
        }
    }

    public void SpawnProjectile(Vector3 position)
    {
        GameObject projectile = Instantiate(weaponSO.projectile, position, Quaternion.identity).gameObject;
        InstanceFinder.ServerManager.Spawn(projectile);
        SetSpawnedProjectile(projectile);
    }

    [ObserversRpc]
    public void SetSpawnedProjectile(GameObject projectile)
    {
        loadedProjectile = projectile.GetComponent<CatapultProjectile>();
        loadedProjectile.transform.SetParent(firePoint[0]);
    }

    [ObserversRpc]
    public void FireProjectile(Vector3 target, float speed, float angle)
    {
        if (loadedProjectile == null) return;
        loadedProjectile.transform.SetParent(null);
        loadedProjectile.SetUp(target, speed, angle);
    }
}
