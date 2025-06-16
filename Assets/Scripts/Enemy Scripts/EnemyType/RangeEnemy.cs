using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy, IRangedAttacker
{
    protected EnemyIdleState idleState;
    protected EnemyRangeAttackState attackState;

    [SerializeField] private RangedWeapon rangedweapon;
    [SerializeField] private int rangedweaponCount;

    private ObjectPool<RangedWeapon> bulletPool;

    protected override void Awake()
    {
        base.Awake();

        Dictionary<StateType, State<Enemy>> states = new Dictionary<StateType, State<Enemy>>();

        bulletPool = new ObjectPool<RangedWeapon>(10, rangedweapon);

        idleState = new EnemyIdleState(this, stateMachine, states);
        attackState = new EnemyRangeAttackState(this, stateMachine, states);

        states.Add(StateType.Idle, idleState);
        states.Add(StateType.Attack, attackState);
    }

    protected void Start()
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

    public virtual RangedWeapon GetRangedWeaponFromPool(Vector3 position)
    {
        return bulletPool.GetObjectFromPool(position, null);
    }

    public virtual RangedWeapon GetRangedWeaponFromPoolAndSetDirection(Vector3 position, Vector3 dir)
    {
        return bulletPool.GetObjectFromPool(position, dir);
    }

    public virtual int GetRangedWeaponCount()
    {
        return rangedweaponCount;
    }

    public GameObject GetController()
    {
        return this.gameObject;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, boxSize);
    }
}
