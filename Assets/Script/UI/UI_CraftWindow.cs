using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescripton;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;
    [SerializeField] private Image[] materialImage;
    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        craftButton.onClick.RemoveAllListeners();

        for(int i=0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i=0; i< _data.craftingMatterials.Count;i++)
        {
            if(_data.craftingMatterials.Count > materialImage.Length)
            {
                Debug.Log("sdadjfakdljjkl");
            }

            materialImage[i].sprite = _data.craftingMatterials[i].data.itemIcon;
            materialImage[i].color = Color.white;
            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();
            materialSlotText.text = _data.craftingMatterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;
        }

        itemIcon.sprite = _data.itemIcon;
        itemName.text = _data.itemName;
        itemDescripton.text = _data.GetDescription();

        craftButton.onClick.AddListener(()=> Inventory.instance.CanCraft(_data,_data.craftingMatterials));
    }
}
