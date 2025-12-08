public class Beholder : CommonMonsterBase
{

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 250;
        currentHealth = maxHealth;
        moveSpeed = 3.5f;
        attackDamage = 15;
        attackSpeed = 0.5f;
        experiencePoints = 50;
    }
    protected override void Start()
    {
        base.Start();

        StateMachine.ChangeState(StateMachine.MoveState);
    }

    protected override void Update()
    {
        base.Update();
    }
}