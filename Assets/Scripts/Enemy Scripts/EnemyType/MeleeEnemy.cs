using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy, IMeleeAttack
{
    private EnemyIdleState idleState;
    private EnemyFollowState attackState;

    [SerializeField] private MeleeWeapon meeleWeapon;
    [SerializeField] private float viewRange;
    [SerializeField] private LayerMask playerLayer;

    Vector3 boxSize;
    Vector3 center;

    protected override void Awake()
    {
        base.Awake();

        Dictionary<StateType, State<Enemy>> states = new Dictionary<StateType, State<Enemy>>();

        idleState = new EnemyIdleState(this, stateMachine, states);
        attackState = new EnemyFollowState(this, stateMachine, states);

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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, boxSize);
    }

    public override Weapon GetWeapon()
    {
        return meeleWeapon;
    }

    public void StartAttack()
    {
        meeleWeapon.StartAttack(this.gameObject);
    }

}
