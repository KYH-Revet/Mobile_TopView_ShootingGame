using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class Enemy : Character, IObserver<Player>
{
    // <summary>target = player</summary>
    protected Transform target;
    // NavMeshAgent Component
    protected NavMeshAgent navMeshAgent;

    [Header("Enemy")]
    public Transform firstDestination;
    public bool positioning;

    [Header("Attack Setting")]
    [Range(1f, 100f)]
    [Tooltip("공격 사거리")]
    public float range = 50f;

    [Tooltip("공격 간 딜레이 시간")]
    public float attackDelay = 3f;
    protected float attackDelay_Cur;

    // Unity Functions
    protected override void Awake()
    {
        // Base
        base.Awake();

        // Attack delay timer
        attackDelay_Cur = attackDelay;
    }

    // System Funtions
    /// <summary>공격 후 다음 공격을 위한 대기 시간</summary>
    /// <returns> true = Ready to attack, false = Waiting</returns>
    protected bool DelayTimer()
    {
        // Timer
        attackDelay_Cur += Time.deltaTime;
        // Time over
        if (attackDelay <= attackDelay_Cur)
        {
            attackDelay_Cur = 0f;
            return true;
        }
        return false;
    }
    // <summary>Target(Player)를 바라보고 조준하는 함수</summary>
    protected void AimToTarget()
    {
        // Look at the target(player)
        transform.LookAt(target.transform.position);
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

        // Aim to target
        if (gunPos != null)
        {
            gunPos.LookAt(target.transform.position);
            gunPos.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        }
    }

    // Event Function
    public virtual void AttackEnd()
    {
        // Reset timer
        attackDelay_Cur = 0f;

        // StateMachine
        ChangeState(_StateMachine.Idle);
    }

    // NavMeshAgent Funstions
    protected void SetNavMeshAgentOption()
    {
        try
        {
            // GetComponent
            navMeshAgent = GetComponent<NavMeshAgent>();

            // Setting option
            navMeshAgent.autoBraking = false;               // autoBracking
            navMeshAgent.stoppingDistance = 0;              // stoppingDistacne
            navMeshAgent.speed = stat.speed;                // speed

            // Set fist destination
            navMeshAgent.SetDestination(positioning ? target.position : firstDestination.position);
        }
        catch (NullReferenceException n)
        {
            Debug.LogError(gameObject.name + ".Enemy.SetNavMeshAgentOption() Error : " + n.Message);
        }
    }

    // Calculation the distance
    public float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    // Default setting when spawn
    public void SpawnSetting()
    {
        // Get target(Player)
        target = Player.instance.transform;

        // Have a first destination?
        positioning = firstDestination == null;

        // NavMeshAgent
        SetNavMeshAgentOption();

        // StateMachine
        ChangeState(_StateMachine.Move);

        // Observer Pattern
        Subscribe();
    }

    // Player tracking
    protected void PlayerTracking()
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
    }

    // StateMachine
    public abstract override void StateMachine();

    // Actions
    public abstract override void Idle();   
    public abstract override bool Move();
    public abstract override void Attack();
    public override void Dead()
    {
        // Observer Pattern
        UnSubscribe();

        // Destroy
        WaveManager.instance.EnemyCountDecrease(transform);
    }

    // Set Strategy
    protected abstract override void SetDefaultStrategy();

    // Observer Pattern Observe : GameManager.GameState, Player Move Input
    public override void Subscribe()
    {
        // GameManager.GameState for animation(GameState.Lose)
        base.Subscribe();

        // Player Move Input
        Player.instance.Subscribe(this);
    }
    public override void UnSubscribe()
    {
        // GameManager.GameState for animation(GameState.Lose)
        base.UnSubscribe();

        // Player Move Input
        Player.instance.UnSubscribe(this);
    }
    // Observer : Position of playerd
    public void OnNext(Player value)
    {
        // Update destination
        if (positioning)
            navMeshAgent.SetDestination(target.position);
    }
}
