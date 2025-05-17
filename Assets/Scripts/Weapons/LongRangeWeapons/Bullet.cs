using Unity.VisualScripting;
using UnityEngine;

public class Bullet : Weapon, IPoolable, IFixedUpdateObserver
{
    [SerializeField] private float velocity;
    [SerializeField] private float maxLifeTime;
    private float currentLifeTime = 0;

    protected Rigidbody2D rb;

    private Vector3 direction;

    public static ObjectPool<Bullet> Pool;

    private void ReturnToPool()
    {
        if (Pool != null)
        {
            Pool.ReturnToPool(this);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
           rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    protected void OnEnable()
    {
        FixedUpdateManager.AddToList(this);
    }

    protected void OnDisable()
    {
        FixedUpdateManager.RemoveFromList(this);
    }

    public override void AttackBehavior()
    {
        
    }

    public void ObserveFixedUpdate()
    {
        currentLifeTime += Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * velocity;


        if (currentLifeTime > maxLifeTime)
        {
            ReturnToPool();
        }

    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
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
                Player.Instance.healthSystem.GetDmg(dmg);
            
            }

            ReturnToPool();
        }
    }

    public void OnGetFromPool()
    {   
        direction = Player.Instance.transform.position - transform.position;
        currentLifeTime = 0f;
    }

    public void OnRetunToPool()
    {
        rb.linearVelocity = Vector2.zero;
    }
}
