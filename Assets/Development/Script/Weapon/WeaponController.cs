using UnityEngine;

public abstract class WeaponController: Controller<WeaponFSM>
{   
    private void Awake()
    {
        StateMachine = new WeaponFSM(this);
    }

    private void Start()
    {
        StateMachine.Initialize(StateMachine.DisabledState);
    }

    private void Update()
    {
        StateMachine.Update();
    }

    public abstract bool IsEnabled();

    public abstract bool IsReady();

    public abstract bool IsReadyToAttack();

    public abstract bool IsAttackFinished();

    public abstract void SetActive(bool enable);

    public abstract void Attack();
}