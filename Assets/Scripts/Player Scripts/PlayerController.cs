using UnityEngine;

public class PlayerController : MonoBehaviour, IUpdateObserver, IFixedUpdateObserver, ILateUpdateObserver
{
    [HideInInspector] public Rigidbody2D rb;

    [Header("Basic Movement")]
    [HideInInspector] public float xVelocity;
    [HideInInspector] public float yVelocity;   
    [HideInInspector] public float direction = 1;
    public float runSpeed;

    [Header("Dash")]
    public int dashTime;
    public float dashForce;
    public float dashNeededStamina;

    [HideInInspector] public StateMachine<PlayerController> stateMachine = new StateMachine<PlayerController>();
    [HideInInspector] public PlayerIdleState playerIdleState;
    [HideInInspector] public PlayerDashState playerDashState;

    public Weapon weapon;

    private void OnEnable()
    {
        UpdateManager.AddToList(this);
        FixedUpdateManager.AddToList(this);
        LateUpdateManager.AddToList(this);
    }

    private void OnDisable()
    {
        UpdateManager.RemoveFromList(this);
        FixedUpdateManager.RemoveFromList(this);
        LateUpdateManager.RemoveFromList(this);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerIdleState = new PlayerIdleState(this, stateMachine);
        playerDashState = new PlayerDashState(this, stateMachine);
    }

    private void Start()
    {
        stateMachine.InitializeState(playerIdleState);
    }

    public void ObserveUpdate()
    {
        stateMachine.UpdateCurrentState();
        Player.Instance.weaponSwitcher.Switcher_Update();
    }

    public void ObserveFixedUpdate() 
    {
        stateMachine.FixedUpdateCurrentState();   
    }

    public void ObserveLateUpdate()
    {
        stateMachine.LateUpdateCurrentState();
        animate();
    }

    private void animate()
    {
        float we = 90 - 90 * Mathf.Sign(direction);
        Vector3 rotator = new Vector3(transform.rotation.x, we, transform.rotation.z);
        transform.rotation = Quaternion.Euler(rotator);

    }
}
