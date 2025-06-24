using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value=5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            Player.Instance.goldSystem.CollectGold(value);
            SoundsManager.Instance.PlayAudioClip(Sounds.CollectEnemyCoin);
            Destroy(this.gameObject);
        }
    }
}
