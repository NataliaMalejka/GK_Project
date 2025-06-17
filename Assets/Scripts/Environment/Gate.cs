using UnityEngine;

/**
 * Gate that opens when the player presses E nearby and has a key.
 * 
 * @author Krzysztof Gach
 * @version 1.0
 */
[RequireComponent(typeof(Collider2D))]
public class Gate : MonoBehaviour
{
    public bool IsOpen { get; private set; } = false;

    [SerializeField] private float interactRange = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private Transform player;
    private KeySystem keySystem;

    private Animator animator;
    [SerializeField] private GameObject textPanel;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            keySystem = playerObj.GetComponent<KeySystem>();
        }
        else
        {
            Debug.LogWarning("Gate: Player object with tag 'Player' not found.");
        }

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsOpen || player == null || keySystem == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= interactRange && Input.GetKeyDown(interactKey))
        {
            TryUseKey();
        }
    }

    private void TryUseKey()
    {
        if (!keySystem.UseKey(this))
        {
            Debug.Log("Gate is locked. No key available.");
        }
    }

    public void Open()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            Debug.Log("Gate opened!");
            animator?.SetTrigger("OpenGate");


            // Optional: Play open animation or sound
        }
    }

    public void DisenableCollider()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            col.enabled = false; // Disable the collider to let the player walk through
            circleCollider2D.enabled = false; // Disenable text view
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            textPanel.SetActive(true); // Show interaction prompt when player is near
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            textPanel.SetActive(false); // Hide interaction prompt when player leaves
        }  
    }
}
