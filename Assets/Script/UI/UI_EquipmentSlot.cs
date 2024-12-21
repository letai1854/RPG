using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EqiupmentType slotType;

    private void Awake()
    {
        gameObject.name ="Equipment slot - " + slotType.ToString(); 
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null||item.data==null)
        {
            return;
        }
        //base.OnPointerDown(eventData);
        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
        Inventory.instance.AddItem(item.data as ItemData_Equipment);

        ui.itemToolTip.HideToolTip();
        
        CleanUpSlot();
    }
}
