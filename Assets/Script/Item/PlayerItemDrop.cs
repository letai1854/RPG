using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLooseItems;
    [SerializeField] private float chanceToLooseMaterials;
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;
        List<InventoryItem> currentStash = inventory.GetStashList();
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();   
        List<InventoryItem> itemsToUnequip = new List<InventoryItem> ();
        List<InventoryItem> materulsToLoose = new List<InventoryItem>();
        foreach(InventoryItem item in currentEquipment)
        {
            if(Random.Range(0,100) <= chanceToLooseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }
        for(int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        foreach(InventoryItem item in currentStash)
        {
            if(Random.Range(0,100)<= chanceToLooseMaterials)
            {
                DropItem(item.data);
                materulsToLoose.Add(item);
            }
        }

        for (int i = 0; i < materulsToLoose.Count; i++)
        {
            inventory.RemoveItem(materulsToLoose[i].data);
        }
    }
}
