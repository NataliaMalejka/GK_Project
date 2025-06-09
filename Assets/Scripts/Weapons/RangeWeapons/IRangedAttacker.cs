using UnityEngine;

public interface IRangedAttacker
{
    public RangedWeapon GetRangedWeaponFromPool(Vector3 position);
    public RangedWeapon GetRangedWeaponFromPoolAndSetDirection(Vector3 position, Vector3 dir);
    public int GetRangedWeaponCount();
    public GameObject GetController();
}
