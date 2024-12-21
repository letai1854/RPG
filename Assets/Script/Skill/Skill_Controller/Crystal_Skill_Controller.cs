using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();    
    private Player player;
    private float crystalExistTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;
    private Transform closesetTarget;
    [SerializeField] private LayerMask WhatIsEnemy;
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove,float _moveSpeed,Transform _closestTarget, Player _player)
    {
        player = _player;
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closesetTarget = _closestTarget;
    }
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();    
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, radius,WhatIsEnemy);
        if(collider2Ds.Length>0)
           closesetTarget = collider2Ds[Random.Range(0,collider2Ds.Length)].transform;
    }
    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCcrystal();
        }
        if (canMove)
        {
            if (closesetTarget == null)
                return;
            transform.position = Vector2.MoveTowards(transform.position, closesetTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closesetTarget.position) < 1)
            {
                FinishCcrystal();
                canMove = false;
            }
        }
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector3(3, 3), growSpeed*Time.deltaTime);
    }

    public void FinishCcrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }
    private void AnimationExplodeEvent()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EqiupmentType.Amulet);

                if(equipedAmulet != null)
                {
                    equipedAmulet.Effect(hit.transform);
                }
                            
            }
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
