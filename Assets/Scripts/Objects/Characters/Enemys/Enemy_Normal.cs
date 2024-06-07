using UnityEngine;

public abstract class Enemy_Normal : Enemy
{
    // StateMachine
    public override void StateMachine()
    {
        // Target(Player)가 사거리를 벗어남. 추격 재개
        if (positioning && DistanceToPlayer() > range)
        {
            // NavMesh start
            navMeshAgent.isStopped = false;

            // Reset dely timer
            attackDelay_Cur = 0f;

            // State Change
            ChangeState(_StateMachine.Move);
        }

        // StateMachine
        switch (state)
        {
            case _StateMachine.Idle:
                Idle();
                break;
            case _StateMachine.Move:
                Move();
                break;
            case _StateMachine.Attack:
                Attack();
                break;
        }
    }
    public override void Idle()
    {
        // Look at Target
        AimToTarget();

        // Waiting attack delay
        if (DelayTimer())
            ChangeState(_StateMachine.Attack);
    }
    public override bool Move()
    {
        // Not reached first destination
        if (!positioning)
        {
            // Reach to first destination
            if (navMeshAgent.remainingDistance <= 1f)
            {
                positioning = true;
                navMeshAgent.stoppingDistance = range;
                return false;
            }
            return true;
        }

        // Target in range
        if (DistanceToPlayer() <= range)
        {
            // NavMesh option
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;

            // State change
            ChangeState(_StateMachine.Idle);
            return false;
        }
        return true;
    }
    public override void Attack()
    {
        //Look at Target
        AimToTarget();
    }
    protected override void SetDefaultStrategy()
    {
        //Bullet option
        AddBulletDamageType(new DamageType_Single());
    }
}
