using UnityEngine;

public interface IDamageable 
{
    public void Die();

    public void ReduceHP(int dmg, int duration);
}
