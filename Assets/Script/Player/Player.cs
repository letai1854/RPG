using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Detail")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed=12f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpSpeed;
    private float defaultDashSpeed;
    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    [SerializeField] public float dashDir;
    //[SerializeField] public float dashUsageTimer;
    //[SerializeField] public float dashCooldown;
    public SkillManager skill;

 
    #region states
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackHoleState blackHole { get; private set; }  
    public GameObject sword {  get; private set; }
    public PlayerDeadState deadState { get; private set; }  
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this,stateMachine,"Idle");
        moveState = new PlayerMoveState(this,stateMachine,"Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this,stateMachine,"WallSlide");
        wallJumpState = new PlayerWallJumpState(this,stateMachine,"Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHole = new PlayerBlackHoleState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }
   protected override void Start()
    {
        base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
        defaultMoveSpeed = moveSpeed;
        defaultJumpSpeed = jumpForce;
        defaultDashSpeed = dashSpeed;
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F))
        {
            skill.crystal.CanUseSkill();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseFlask();
        }
    }
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpSpeed;
        dashSpeed = defaultDashSpeed;
    }
    public void AssignNewSword(GameObject _newSword)
    {
        sword= _newSword;
    }
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }
    public IEnumerable BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }
    public void AnimationTrigger()=> stateMachine.currentState.AnimationFinishTrigger();
    public void CheckForDashInput()
    {
        if (isWallDetected())
            return;
        if(skill.dash.dashUnlocked == false)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)&&SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;
            stateMachine.ChangeState(dashState);
        }
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }


}
