public class ChestMonster : CommonMonsterBase
{

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 400;
        currentHealth = maxHealth;
        moveSpeed = 1.2f;
        attackDamage = 10;
        attackSpeed = 2.5f;
        experiencePoints = 60;
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