using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : Enemy, IRangedAttacker
{
    [SerializeField] private List<RangedWeapon> rangedweapons;
    [SerializeField] private int minRangedWeaponCount;
    [SerializeField] private int maxRangedWeaponCount;

    private List<ObjectPool<RangedWeapon>> bulletPools;
    private int currentWeapon = 0;

    protected EnemyIdleState idleState;
    protected EnemyRangeAttackState attackState;

    protected override void Awake()
    {
        base.Awake();

        bulletPools = new List<ObjectPool<RangedWeapon>>();

        for (int i = 0; i < rangedweapons.Count; i++) 
        {
            bulletPools.Add(new ObjectPool<RangedWeapon>(16, rangedweapons[i]));
        }

        Dictionary<StateType, State<Enemy>> states = new Dictionary<StateType, State<Enemy>>();

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
        center = (Vector2)transform.position;
        SeePlayer();
    }

    protected override void SeePlayer()
    {
        Collider2D hitCollider = Physics2D.OverlapBox(center, boxSize, 0f, playerLayer);
        seePlayer = hitCollider != null;
    }

    public override Weapon GetWeapon()
    {
        currentWeapon = Random.Range(0, rangedweapons.Count());
        return rangedweapons[currentWeapon];
    }

    public GameObject GetController()
    {
        return this.gameObject;
    }

    public int GetRangedWeaponCount()
    {
        return Random.Range(minRangedWeaponCount, maxRangedWeaponCount);
    }

    public RangedWeapon GetRangedWeaponFromPool(Vector3 position)
    {
        return bulletPools[currentWeapon].GetObjectFromPool(position, null);
    }

    public RangedWeapon GetRangedWeaponFromPoolAndSetDirection(Vector3 position, Vector3 dir)
    {
        return bulletPools[currentWeapon].GetObjectFromPool(position, dir);
    }

    public override void Die()
    {
        base.Die();
        _=GameManager.Instance.AfterPlayerDead();
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, boxSize);
    }
}
