using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;
    private GameObject currentCrystal;
    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;
    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> cryStalLeft = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();
        if (CanUseMultiCrystal())
            return;
        if (currentCrystal == null)
        {
            CreateCrystal();
        }

        else
        {
            if (canMoveToEnemy)
                return;
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCcrystal();
            }


        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        currentCrystal.SetActive(true);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform),player);
            
        
    }
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy(); 
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (cryStalLeft.Count > 0)
            {
                if (cryStalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);
                cooldown = 0;
                GameObject crystalToSpawn = cryStalLeft[cryStalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                newCrystal.SetActive(true);
                cryStalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<Crystal_Skill_Controller>().
                SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);
        
                if(cryStalLeft.Count <=0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
            
            return true;
            }
        }
        return false;
    }
    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - cryStalLeft.Count;
        for (int i =0; i< amountToAdd;i++)
        {
            cryStalLeft.Add(crystalPrefab);
        }
    }
    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;
        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
