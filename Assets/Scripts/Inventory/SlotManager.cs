using UnityEngine;

public class SlotManager : StaticInstance<SlotManager>
{
    [SerializeField] private InventoryItem playerWeaponItem;

    public void NewPlayerWeapon(Item item)
    {
        playerWeaponItem.SetItem(item);
    }
}
