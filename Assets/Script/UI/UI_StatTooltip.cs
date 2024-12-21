using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;
    
    public void ShowStatToolTip(string _Text)
    {
        description.text = _Text;
        gameObject.SetActive(true);
    }
    public void HideStatToolTip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }
}
