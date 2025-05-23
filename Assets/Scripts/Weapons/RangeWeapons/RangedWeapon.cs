using UnityEngine;

public abstract class RangedWeapon : Weapon, IPoolable
{
    public static ObjectPool<RangedWeapon> Pool;

    public virtual void OnGetFromPool() { }
    public virtual void OnRetunToPool() { }
}
