using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Sword : MeleeWeapon
{
    private Collider2D weaponCollider;
    private Animator animator;
    
    protected override void Awake()
    {
        weaponCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        weaponCollider.enabled = false;
    }

    public override void StartAttack(GameObject controller)
    {
        base.StartAttack(controller);

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
