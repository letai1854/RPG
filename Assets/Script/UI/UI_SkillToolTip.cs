using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;
    public void ShowToolTip(string _skillDescpription, string _skillName)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescpription;
        gameObject.SetActive(true);
    }
    public void HideToolTip() => gameObject.SetActive(false);   
}
