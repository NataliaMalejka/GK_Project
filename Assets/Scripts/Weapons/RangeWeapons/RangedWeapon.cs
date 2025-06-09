using UnityEngine;

public abstract class RangedWeapon : Weapon, IPoolable, IFixedUpdateObserver
{
    public static ObjectPool<RangedWeapon> Pool;

    [SerializeField] protected float velocity;
    [SerializeField] protected float maxLifeTime;

    protected float currentLifeTime = 0f;
    protected Rigidbody2D rb;
    protected Vector3 direction;

    protected void OnEnable()
    {
        FixedUpdateManager.AddToList(this);
    }

    protected void OnDisable()
    {
        FixedUpdateManager.RemoveFromList(this);
    }

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>();
    }

    public virtual void ObserveFixedUpdate() { }


    public void Setcontroller(GameObject controller)
    {
        this.controller = controller;
    }

    public void OnGetFromPool()
    {
        direction = Player.Instance.transform.position - transform.position;
        currentLifeTime = 0f;
    }

    public void OnGetFromPool(Vector3 dir)
    {
        direction = dir;
        currentLifeTime = 0f;
    }

    public void OnRetunToPool()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void ReturnToPool()
    {
        Pool.ReturnToPool(this);
    }
}
