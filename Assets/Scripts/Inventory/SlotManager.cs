using System.Linq;
using UnityEngine;

public class SlotManager : StaticInstance<SlotManager>
{
    [SerializeField] private InventoryItem playerWeaponItem;
    [SerializeField] private InventoryItem[] itemsList = new InventoryItem[15];

    public void NewPlayerWeapon(Item item)
    {
        playerWeaponItem.SetItem(item);
    }

    public Item GetPlayerWeapon()
    {
        return playerWeaponItem.item;
    }       

    //public void LoadItemsCost(int[] costList)
    //{
    //    for (int i = 0; i < itemsList.Count(); i++)
    //    {
    //        itemsList[i].SetCost(costList[i]);
    //    }
    //}

    //public int[] GetCostLIst()
    //{
    //    int[] costList = new int[itemsList.Count()];

    //    for (int i = 0; i < itemsList.Count(); i++)
    //    {
    //        costList[i] = itemsList[i].GetCost();
    //    }

    //    return costList;
    //}

    public void SetPlayerWeapon(Item item)
    {
        PlayerWeapon weapon = Player.Instance.controller.GetWeapon();

        weapon.SetSprite(item.sprite);
        weapon.SetDmg(item.dmg);
        weapon.SetDamageDuration(item.damageDuration);
        weapon.setNeededMana(item.neededMana);
        weapon.SetNeededStamina(item.neededStamina);

        NewPlayerWeapon(item);
    }
}
