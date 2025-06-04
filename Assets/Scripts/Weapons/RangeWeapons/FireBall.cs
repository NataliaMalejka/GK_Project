using UnityEngine;

public class FireBall : RangedWeapon, IFixedUpdateObserver
{
    [SerializeField] private int damageDuration;
    [SerializeField] private float velocity;
    [SerializeField] private float maxLifeTime;

    private float currentLifeTime = 0f;
    private Rigidbody2D rb;
    private Vector3 direction;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>();
    }

    protected void OnEnable()
    {
        FixedUpdateManager.AddToList(this);
    }

    protected void OnDisable()
    {
        FixedUpdateManager.RemoveFromList(this);
    }

    public void ObserveFixedUpdate()
    {
        currentLifeTime += Time.fixedDeltaTime;
        rb.linearVelocity = direction.normalized * velocity;

        if (currentLifeTime > maxLifeTime)
        {
            RetunToPool();
        }
    }
    public override void OnGetFromPool()
    {
        direction = Player.Instance.transform.position - transform.position;
        currentLifeTime = 0f;
    }

    public override void OnGetFromPool(Vector3 dir)
    {
        direction = dir;
        currentLifeTime = 0f;
    }

    public void RetunToPool()
    {
        rb.linearVelocity = Vector2.zero;
        Pool.ReturnToPool(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (other.gameObject == shooter) return;

        //IDamageable enemy = other.GetComponent<IDamageable>();
        IPlayer player = collision.GetComponent<IPlayer>();

        //if (enemy != null)
        //{
        //    enemy.TakeDamage(damage);

        //}

        if (player != null)
        {
            if (Player.Instance.controller.stateMachine.currentState != Player.Instance.controller.playerDashState)
            {
                Player.Instance.healthSystem.ApplyDamageOverTime(dmg, damageDuration);

            }

            RetunToPool();
        }
    }
}
