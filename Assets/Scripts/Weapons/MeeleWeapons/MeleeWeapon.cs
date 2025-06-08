using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    public virtual void StartAttack(GameObject controller) 
    {
        this.controller = controller;
    }
}
