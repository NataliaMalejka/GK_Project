using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //public InventorySlot[] inventorySlots;
    //public GameObject inventoryItemPrefab;

    //public bool addItem(Item item)
    //{
    //    for (int i = 0; i < inventorySlots.Length; i++)
    //    {
    //        InventorySlot slot = inventorySlots[i];
    //        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

    //        if (itemInSlot != null)
    //        {
    //            itemInSlot.RefreshCount();
    //            return true;
    //        }
    //    }


    //    for (int i = 0; i < inventorySlots.Length; i++)
    //    {
    //        InventorySlot slot = inventorySlots[i];
    //        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

    //        if (itemInSlot == null)
    //        {
    //            SpawnNewItem(item, slot);
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //void SpawnNewItem(Item item, InventorySlot slot)
    //{
    //    GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
    //    InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
    //    inventoryItem.InitialiseItem(item);
    //}
}
