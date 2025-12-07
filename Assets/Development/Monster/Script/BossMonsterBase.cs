public abstract class BossMonsterBase : MonsterController
{
    public override void Die()
    {
        base.Die();
        GameProgressManager.Instance.CompleteStage();
    }
}