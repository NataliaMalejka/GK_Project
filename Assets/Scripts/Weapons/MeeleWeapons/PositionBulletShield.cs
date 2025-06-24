using UnityEngine;

public class PositionBulletShield : MeleeWeapon, IFixedUpdateObserver
{
    [SerializeField] private GameObject parentEnemy;
    private void OnEnable()
    {
        FixedUpdateManager.AddToList(this);
    }

    private void OnDisable()
    {
        FixedUpdateManager.RemoveFromList(this);
    }

    public void ObserveFixedUpdate()
    {
        if (parentEnemy != null)
        {
            this.gameObject.transform.position = parentEnemy.transform.position;
        }
        else
            Destroy(this.gameObject);
    }

    public override void StartAttack(GameObject controller)
    {
        //base.StartAttack(controller);
    }
}
