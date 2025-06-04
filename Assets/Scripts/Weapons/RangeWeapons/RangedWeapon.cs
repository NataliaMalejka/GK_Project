using UnityEngine;

public abstract class RangedWeapon : Weapon, IPoolable
{
    public static ObjectPool<RangedWeapon> Pool;

    public virtual void OnGetFromPool() { }

    public virtual void OnGetFromPool(Vector3 dir) { }

    public virtual void OnRetunToPool() { }
}
