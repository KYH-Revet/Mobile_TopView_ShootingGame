using UnityEngine;

public class Enemy_Normal : Enemy
{
    // StateMachine
    public override void StateMachine()
    {
        // StateMachine
        switch (_state)
        {
            case _StateMachine.Idle:
                // Target(Player)가 사거리를 벗어남. 추격 재개
                PlayerTracking();

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
            if (Vector3.Distance(transform.position, firstDestination.position) <= 1f)
            {
                positioning = true;
                navMeshAgent.SetDestination(target.position);
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
    /// <summary> Default strategy = Single damage </summary>
    protected override void SetDefaultStrategy()
    {
        //Bullet option
        AddBulletDamageType(new DamageType_Single());
    }
    
}
