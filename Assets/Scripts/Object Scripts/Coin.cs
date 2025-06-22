using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            Player.Instance.goldSystem.CollectGold(value);
            Player.Instance.hudUpdater.updateGold(Player.Instance.goldSystem.GetGoldAmount());

            Destroy(this.gameObject);
        }
    }
}
