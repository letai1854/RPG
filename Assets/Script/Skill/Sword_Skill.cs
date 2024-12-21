using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;
    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;
    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;
    [Header("Spin info")]
    [SerializeField] private float hitCooldown=0.35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity=1;
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freeTimeDuration;
    [SerializeField] private float returnSpeed;
    private Vector2 finalDir;
    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBeetwenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsparent;
    private GameObject[] dots;
    protected override void Start()
    {
        base.Start();
        GenereateDots();
        SetupGravity();
    }
    private void SetupGravity()
    {
        if(swordType == SwordType.Bounce) {
            swordGravity = bounceGravity;
        }
        else if(swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x*launchForce.x,AimDirection().normalized.y*launchForce.y);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i=0;i<dots.Length;i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBeetwenDots);
                
            }
        }
    }
    public void createSword()
    {
        GameObject newSword= Instantiate(swordPrefab,player.transform.position,transform.rotation);
        newSword.SetActive(true);
        Sword_Skill_Control newSwordScript = newSword.GetComponent<Sword_Skill_Control>();
        if (swordType == SwordType.Bounce)
        {
            newSwordScript.SetupBounce(true, bounceAmount,bounceSpeed);
        }
        else if (swordType == SwordType.Pierce)
        {
            newSwordScript.SetupPierce(pierceAmount);
        }
        else if (swordType == SwordType.Spin)
        {
            newSwordScript.SetupSpin(true,maxTravelDistance,spinDuration,hitCooldown);
        }
        newSwordScript.SetUpSword(finalDir,swordGravity,player,freeTimeDuration,returnSpeed);
        player.AssignNewSword(newSword);
        DotsActive(false);
    }
    #region Aim region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition-playerPosition;
        return direction;
    }
    public void DotsActive(bool _isActive)
    {
        for(int i = 0; i < dots.Length;i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void GenereateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i=0;i<numberOfDots;i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsparent);
            dots[i].SetActive(false);
        }
    }
    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position +new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y *  launchForce.y) * t
            + .5f * (Physics2D.gravity * swordGravity)*(t*t);
        return position;
    }
    #endregion
}
