using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enenies effect", menuName = "Data/Freeze enemies effect")]

public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {

        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();    
        if(playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
        {
            return;
        }

        if (!Inventory.instance.CanUseArmor())
        {
            return;
        }

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_transform.position,2);
        foreach (var hit in collider2Ds)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration) ;
   
        }
    }
}
