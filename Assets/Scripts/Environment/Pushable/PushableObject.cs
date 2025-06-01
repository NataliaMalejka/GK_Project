using UnityEngine;

/** 
 * 
 * @author Krzysztof Gach
 * @version 1.0
 */
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PushableObject : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        Lock(); // Start locked
    }

    public void Lock()
    {
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void Unlock()
    {
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
