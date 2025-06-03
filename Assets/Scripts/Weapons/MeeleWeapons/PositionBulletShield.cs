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
       this.gameObject.transform.position = parentEnemy.transform.position;
    }

    public override void StartAttack()
    {
        
    }

    
}
