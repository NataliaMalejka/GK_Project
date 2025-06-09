using UnityEngine;

public enum StateType
{
    Idle,
    Attack
}

public abstract class Enemy : MonoBehaviour,IUpdateObserver, IFixedUpdateObserver, ILateUpdateObserver, IDamageable
{
    public HealthSystem healthSystem { get; private set; }

    public float speed;
    public GameObject[] wayPoints;

    [HideInInspector] public bool seePlayer;
    [HideInInspector] public Rigidbody2D rb;

    protected StateMachine<Enemy> stateMachine = new StateMachine<Enemy>();

    protected void OnEnable()
    {
        UpdateManager.AddToList(this);
        FixedUpdateManager.AddToList(this);
        LateUpdateManager.AddToList(this);
    }

    protected void OnDisable()
    {
        UpdateManager.RemoveFromList(this);
        FixedUpdateManager.RemoveFromList(this);
        LateUpdateManager.RemoveFromList(this);
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        healthSystem = GetComponent<HealthSystem>();
    }

    protected abstract void SeePlayer();
    public abstract Weapon GetWeapon();

    public virtual void ObserveUpdate() => stateMachine.UpdateCurrentState();
    public virtual void ObserveFixedUpdate() => stateMachine.FixedUpdateCurrentState();
    public virtual void ObserveLateUpdate() => stateMachine.LateUpdateCurrentState();

    public void ReduceHP(int dmg, int duration)
    {
        healthSystem.GetDmg(dmg, duration);
    }

    public void Die()
    {
        RangeEnemy rangeEnemy = this.gameObject.GetComponent<RangeEnemy>();

        if (rangeEnemy != null)
        {
            
        }

        Destroy(this.gameObject);
    }
}
