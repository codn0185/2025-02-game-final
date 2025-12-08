public class MiniGolem : CommonMonsterBase
{

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 50;
        currentHealth = maxHealth;
        moveSpeed = 3.0f;
        attackDamage = 5;
        attackSpeed = 2f;
        experiencePoints = 2;
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