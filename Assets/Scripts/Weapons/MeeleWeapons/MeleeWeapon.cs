using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    private Collider2D weaponCollider;
    private Animator animator;

    protected virtual void Awake()
    {
        weaponCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        weaponCollider.enabled = false;
    }

    public virtual void StartAttack(GameObject controller) 
    {
        this.controller = controller;
        animator.SetTrigger("Attack");
    }

    public void EnableCollider()
    {
        weaponCollider.enabled = true;
    }

    public void DisenableCollider()
    {
        weaponCollider.enabled = false;
    }
}
