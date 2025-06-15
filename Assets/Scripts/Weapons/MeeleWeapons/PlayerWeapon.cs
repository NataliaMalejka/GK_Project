using UnityEngine;

public class PlayerWeapon : MeleeWeapon
{
    public float neededStamina;
    public int neededMana;
    protected SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetDmg(int dmg)
    {
        this.dmg = dmg;
    }

    public void SetDamageDuration(int damageDuration)
    {
        this.damageDuration = damageDuration;
    }

    public void SetNeededStamina(float stamina)
    {
        this.neededStamina = stamina;
    }

    public void setNeededMana(int mana)
    {
        this.neededMana = mana;
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
