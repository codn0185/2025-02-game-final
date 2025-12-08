public class Slime : CommonMonsterBase
{
    protected override void Awake()
    {
        base.Awake();

        // maxHealth = 30;
        // currentHealth = maxHealth;
        // moveSpeed = 2.0f;
        // attackDamage = 3;
        // attackSpeed = 1.5f;
        // experiencePoints = 2;
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