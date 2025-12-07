public abstract class BossMonsterBase : MonsterController
{
    protected override void Die()
    {
        base.Die();
        GameProgressManager.Instance.CompleteStage();
    }
}