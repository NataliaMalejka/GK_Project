using UnityEngine;

public enum StateType
{
    Idle,
    Attack
}

public abstract class Enemy : MonoBehaviour,IUpdateObserver, IFixedUpdateObserver, ILateUpdateObserver, IDamageable
{
    public HealthSystem healthSystem { get; protected set; }

    public float speed;
    public GameObject[] wayPoints;

    [HideInInspector] public bool seePlayer;
    [HideInInspector] public Rigidbody2D rb;

    [SerializeField] protected float viewRange;
    [SerializeField] protected LayerMask playerLayer;

    protected Vector3 boxSize;
    protected Vector3 center;

    protected StateMachine<Enemy> stateMachine = new StateMachine<Enemy>();

    [SerializeField] private GameObject coin;

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

    public virtual void Die()
    {
        int random = Random.Range(0, 2);

        if (random == 0 && coin!=null)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }

        Player.Instance.manaSystem.IncreaseCurrentMana(5);
        Player.Instance.hudUpdater.updateStaminaBar(Player.Instance.staminaSystem.currentStamina, Player.Instance.staminaSystem.getMaxStamina());

        Destroy(this.gameObject);
    }
}
