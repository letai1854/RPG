using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    protected override void Die()
    {
        base.Die();
        player.Die();
        player.GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
    protected override void DecreaseHealthby(int _damage)
    {
        base.DecreaseHealthby(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EqiupmentType.Armor);

        if (currentArmor != null)
        {
            currentArmor.Effect(player.transform);
        }
    }

}
