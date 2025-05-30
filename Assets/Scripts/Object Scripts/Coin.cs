using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.Instance.goldSystem.CollectGold(value);
        Destroy(this.gameObject);
    }
}
