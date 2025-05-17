using UnityEngine;

public interface IPlayer { }
public class Player : PersistentSingleton<Player>, IPlayer
{
    public PlayerController controller { get; private set; }
    public HealthSystem healthSystem { get; private set; }
    public StaminaSystem staminaSystem { get; private set; }
    public ManaSystem manaSystem { get; private set; }
    public WeaponSwitcher weaponSwitcher { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<PlayerController>();
        healthSystem = GetComponent<HealthSystem>();
        staminaSystem = GetComponent<StaminaSystem>();
        manaSystem = GetComponent<ManaSystem>();
        weaponSwitcher = GetComponent<WeaponSwitcher>();
    }
}
