using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool canCreateCloneOnCounterAttack;
    [Header("Clone can duplicate")]
    [SerializeField] private float chanceToDuplicate;
    [SerializeField] private bool canDuplicateClone;
    [Header("Crystal instead of clone")]
    public bool crystalInstradOfClone;
    public void CreateClone(Transform _clonePosition,Vector3 _offset)
    {
        if (crystalInstradOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();  
            return;
        }
        GameObject newClone= Instantiate(clonePrefab);
        //transform.position = _clonePosition.position;
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition,cloneDuration,canAttack,_offset, FindClosestEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,player);
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }
    private IEnumerator CreateCloneWithDelay(Transform _transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}
