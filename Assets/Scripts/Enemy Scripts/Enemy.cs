using UnityEngine;

public enum StateType
{
    Idle,
    Attack
}

public abstract class Enemy : MonoBehaviour, IUpdateObserver, IFixedUpdateObserver, ILateUpdateObserver
{
    public HealthSystem healthSystem { get; private set; }

    public float speed;
    public GameObject[] wayPoints;

    [HideInInspector] public bool seePlayer;

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
        healthSystem = GetComponent<HealthSystem>();
    }

    public virtual void ObserveUpdate()
    {
        stateMachine.UpdateCurrentState();
    }

    public virtual void ObserveFixedUpdate()
    {
        stateMachine.FixedUpdateCurrentState();
        
    }

    public virtual void ObserveLateUpdate()
    {
        stateMachine.LateUpdateCurrentState();
    }

    protected abstract void SeePlayer();

}
