using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    private Image skillImage;

    [SerializeField] private int skillPrice;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;
    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;


    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - "+skillName;
    }
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnclockSkillSlot());
    }
    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;
        ui = GetComponentInParent<UI>();
    }

    public void UnclockSkillSlot()
    {
        if (PlayerManager.instance.HaveEnoughMoney(skillPrice) == false)
        {
            return;
        }
        for (int i=0; i< shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                return;
            }
        }
        for (int i=0; i<shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                return ;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        ui.skillToolTip.ShowToolTip(skillDescription, skillName);
        
        Vector2 mousePosotion = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;
        if (mousePosotion.x > 300)
        {
            xOffset = -40;
        }
        else
        {
            xOffset = 40;
        }

        if (mousePosotion.y > 200)
        {
            yOffset = -70;
        }
        else
        {
            yOffset = 60;
        }

        ui.skillToolTip.transform.position = new Vector2(mousePosotion.x+xOffset, mousePosotion.y+yOffset);

    }
}
