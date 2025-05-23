using UnityEngine;

public interface IRangedAttacker
{
    public RangedWeapon GetRangedWeaponFromPool(Vector3 position);
}
