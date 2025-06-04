using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy, IRangedAttacker
{
    private EnemyIdleState idleState;
    private EnemyRangeAttackState attackState;

    [SerializeField] private RangedWeapon rangedweapon;
    public int rangedweaponCount;
    [SerializeField] private float viewRange;
    [SerializeField] private LayerMask playerLayer;

    private ObjectPool<RangedWeapon> bulletPool;

    Vector3 boxSize;
    Vector3 center;

    protected override void Awake()
    {
        base.Awake();

        Dictionary<StateType, State<Enemy>> states = new Dictionary<StateType, State<Enemy>>();

        bulletPool = new ObjectPool<RangedWeapon>(10, rangedweapon);
        RangedWeapon.Pool = bulletPool;

        idleState = new EnemyIdleState(this, stateMachine, states);
        attackState = new EnemyRangeAttackState(this, stateMachine, states, bulletPool);

        states.Add(StateType.Idle, idleState);
        states.Add(StateType.Attack, attackState);
    }

    private void Start()
    {
        stateMachine.InitializeState(idleState);
        boxSize = new Vector3(viewRange, viewRange, 2f);
    }

    public override void ObserveFixedUpdate()
    {
        base.ObserveFixedUpdate();
        center = (Vector2)transform.position + new Vector2(2 * Mathf.Sign(transform.rotation.y), 1);
        SeePlayer();
    }

    protected override void SeePlayer()
    {
        Collider2D hitCollider = Physics2D.OverlapBox(center, boxSize, 0f, playerLayer);
        seePlayer = hitCollider != null;
    }

    public override Weapon GetWeapon()
    {
        return rangedweapon;
    }

    public RangedWeapon GetRangedWeaponFromPool(Vector3 position)
    {
        return bulletPool.GetObjectFromPool(position, null);
    }

    public RangedWeapon GetRangedWeaponFromPoolAndSetDirection(Vector3 position, Vector3 dir)
    {
        return bulletPool.GetObjectFromPool(position, dir);
    }

    public int GetRangedWeaponCount()
    {
        return rangedweaponCount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, boxSize);
    }
}
