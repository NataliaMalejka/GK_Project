using UnityEngine;

public class ManaMixture : MonoBehaviour
{
    [SerializeField] private int mana;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            Player.Instance.manaSystem.IncreaseCurrentMana(mana);
            Destroy(this.gameObject);
        }
    }
}
