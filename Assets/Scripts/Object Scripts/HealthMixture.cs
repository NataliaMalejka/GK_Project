using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HealthMixture : MonoBehaviour
{
    [SerializeField] private int health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            Player.Instance.healthSystem.Heal(health);
            Destroy(this.gameObject);
        }
    }
}
